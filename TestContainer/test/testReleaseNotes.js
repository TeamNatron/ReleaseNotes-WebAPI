var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);

var accessToken;
const addressGet = "/api/releasenote";
const addressGetSingle = "/api/releasenote/5";
const addressGetSingleFail = "/api/releasenote/23123";
const addressPut = "/api/releasenote/100";
const addressPutFail = "api/releasenote/123123125";
const addressDelete = "/api/releasenote/5";
const addressDeleteFail = "/api/releasenote/897324";

var noteToDelete;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Release notes GET", () => {
  before(() => init());

  // should return 200 OK and a array of release notes
  it("GET | Returns all the release notes", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        expect(res.body).to.be.a("array");
        expect(res.body[0].title).to.be.a("string").that.is.not.empty;
        expect(res.body[0].ingress).to.be.a("string").that.is.not.empty;
        expect(res.body[0].ingress).to.be.a("string").that.is.not.empty;
        expect(res.body[0].description).to.be.a("string").that.is.not.empty;
        expect(res.body[0].authorName).to.be.a("string").that.is.not.empty;
        expect(res.body[0].authorEmail).to.be.a("string").that.is.not.empty;
        expect(res.body[0].workItemTitle).to.be.a("string").that.is.not.empty;
        expect(res.body[0].isPublic).to.equal(false);
        expect(res.body[0].workItemId).to.be.a("number");
        res.should.have.status(200);
        done();
      });
  });

  // should return a 401 unauth
  it("GET | Tries to get a single release without auth", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingle)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  // should return 200 OK and  a singe release note
  it("GET | Successfully gets a single release note", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingle)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response);
        }
        res.should.have.status(200);
        expect(res.body.id).to.be.a("number");
        expect(res.body.title).to.be.a("string").that.is.not.empty;
        expect(res.body.ingress).to.be.a("string").that.is.not.empty;
        expect(res.body.authorName).to.be.a("string").that.is.not.empty;
        expect(res.body.authorEmail).to.be.a("string").that.is.not.empty;
        expect(res.body.isPublic).to.be.a("boolean");
        expect(res.body.workItemId).to.be.a("number");
        noteToDelete = res.body;
        done();
      });
  });

  // should return a 204 No Content
  it("GET | requests a non-existant release note", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingleFail)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(204);
        done();
      });
  });
});

describe("Release note PUT", () => {
  before(() => init());
  // TODO: create tests
});

describe("Release notes DELETE", () => {
  before(() => init());

  // failure case: Tries to delete a release note without being logged in
  // should return a 401 unauth
  it("DELETE | Tries to delete a release note without being logged in", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // failure case: Tries to delete a non-existant release note
  // should return a 400 Bad Request
  it("DELETE | Tries to delete a non-existant release note", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDeleteFail)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end(err => {
        expect(err.should.have.status(400));
        done();
      });
  });

  // succes case: deletes a release note
  // should return a 200 OK and a copy of the newly deleted release note
  it("DELETE | Should return a OK 200 and a copy of the deleted release note", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        expect(res.body).to.deep.equal(noteToDelete);
        done();
      });
  });

  // failure case: Tries to delete a already deleted release note
  // should return a 400 Bad Request
  it("DELETE | Tries to delete a already deleted release note", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end(err => {
        expect(err.should.have.status(400));
        done();
      });
  });
});
