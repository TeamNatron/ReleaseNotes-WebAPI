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
  // DELETE | Should return a 401 unauth
  it("DELETE | Deleting a release note without authorization", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // DELETE | Should return a 400 Bad Request
  it("DELETE | Deleting a non-existant release note", done => {
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

  // DELETE | Should return a OK 200 and a copy of the deleted release
  it("DELETE | Deletes a note correctly", done => {
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

  // DELETE | should return 400 Bad Request
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
