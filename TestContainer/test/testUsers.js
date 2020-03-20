var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const should = chai.should();

chai.use(chaiHttp);

const CORRECT_INPUT_USER_ID = 1;
const CORRECT_INPUT_USER = {
  email: "frank@ungspiller.no",
  password: "123345678"
};

const CORRECT_INPUT_USER_NEW_PASSWORD = {
  email: "frank@ungspiller.no",
  password: "1233456789"
};

const WRONG_INPUT_USER = {
  email: "frank@ungspiller.no",
  password: "1234"
};

var accessToken;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Users POST", () => {
  before(() => init());

  it("POST | Should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/users")
      .send(CORRECT_INPUT_USER)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  it("POST | Should return created", done => {
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

  it("POST | Should return email already in use", done => {
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

describe("Users PUT", () => {
  before(() => init());

  it("PUT | Password got changed successfully", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + CORRECT_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_USER_NEW_PASSWORD)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        done();
      });
  });

  it("PUT | The user doesn't exist", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + WRONG_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_USER_NEW_PASSWORD)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });

  it("PUT | Can't use the same password", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + CORRECT_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_USER_NEW_PASSWORD)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });

  it("PUT | Atleast 5 characters in new password", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + CORRECT_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(WRONG_INPUT_USER)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });
});
