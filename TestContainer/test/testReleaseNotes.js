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
const addressReleaseNote = "/api/releasenote";

const RELEASE_NOTE_WITHOUT_DESCRIPTION = {
  title: "Ny dag, ny release note",
  ingress:
    "Det er en tirsdag i 2020. Det neste du leser vil absolutt blåse minnet ditt.",
  isPublic: "false",
  authorName: "postman@ungspiller.no"
};

var noteToDelete;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Release notes GET", () => {
  before(() => init());

  // should return 200 OK and a array of release notes
  it("GET | Get all release notes", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        expect(res.body).to.be.a("array");
        expect(res.body[0].title).to.be.a("string").that.is.not.empty;
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

  // should return a 200 OK and specified release note
  it("GET | Get a single release", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingle)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        expect(res.body.title).to.be.a("string").that.is.not.empty;
        expect(res.body.ingress).to.be.a("string").that.is.not.empty;
        expect(res.body.description).to.be.a("string").that.is.not.empty;
        expect(res.body.authorName).to.be.a("string").that.is.not.empty;
        expect(res.body.authorEmail).to.be.a("string").that.is.not.empty;
        expect(res.body.workItemTitle).to.be.a("string").that.is.not.empty;
        expect(res.body.isPublic).to.equal(false);
        expect(res.body.workItemId).to.be.a("number");
        res.should.have.status(200);
        done();
      });
  });

  // should return 200 OK and  a singe release note
  it("GET | Successfully gets a single release note", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingle)
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

describe("Release note POST", () => {
  before(() => init());

  it("POST | Tries to create release note without providing a description", done => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleaseNote)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(RELEASE_NOTE_WITHOUT_DESCRIPTION)
      .end(err => {
        console.log("err");
        console.log(err);
        expect(err.should.have.status(400));
        done();
      });
  });
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
