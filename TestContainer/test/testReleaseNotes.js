var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);
var accessToken;
const addressDelete = "/api/releasenote/1";
const addressDeleteFail = "/api/releasenote/897324";

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

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
