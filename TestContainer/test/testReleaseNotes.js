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

var jasdhlasjd;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Release notes GET", () => {
  before(() => init());
  // Should return a 401 unauth
  it("GET | get all without auth", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  // should return 200 OK and a array of release notes
  it("GET | Returns all the release note dummy data", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err);
        }
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
  it("GET | Successfully get a single release note", done => {
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
        expect(res.body.id).to.equal(5);
        jasdhlasjd = res.body;
        done();
      });
  });

  // should return a 204 No Content
  // tries to get a single release note with a non-existant id
  it("GET | requests a non-existant release note", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetSingleFail)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end(err => {
        if (err === null) {
          done();
        }
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
  it("DELETE | Should return a 401 unauth", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // failure case: Tries to delete a non-existant release note
  it("DELETE | Should return a 400 Bad Request", done => {
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

  // failure case: Tries to delete a non-existant release note without being logged in
  it("DELETE | should return a 401 unauth", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDeleteFail)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // succes case: deletes a release note
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
        expect(res.body).to.deep.equal(jasdhlasjd);
        done();
      });
  });

  // failure case: Tries to delete a already deleted release note
  it("DELETE | should return 400 Bad Reques", done => {
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
