var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const should = chai.should();

chai.use(chaiHttp);

const CORRECT_INPUT_RELEASE = {
  ProductVersionId: 100,
  Title: "Release 2.5 - Vannkanon",
  IsPublic: true,
  ReleaseNotesId: [1, 2]
};

var accessToken;
const address = "/api/releases";

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Releases", () => {
  before(() => init());

  it("Should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .post(address)
      .send(CORRECT_INPUT_RELEASE)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  it("Should return created", done => {
    chai
      .request(process.env.APP_URL)
      .post(address)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_RELEASE)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        done();
      });
  });

  it("Should return release name already in use", done => {
    chai
      .request(process.env.APP_URL)
      .post(address)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_RELEASE)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });
});
