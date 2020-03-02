var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const should = chai.should();

chai.use(chaiHttp);

const CORRECT_INPUT_USER = {
  email: "frank@ungspiller.no",
  password: "123345678"
};

var accessToken;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Users", () => {
  before(() => init());

  it("Should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/users")
      .send(CORRECT_INPUT_USER)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  it("Should return created", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/users")
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_USER)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        done();
      });
  });

  it("Should return email already in use", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/users")
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_USER)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });
});
