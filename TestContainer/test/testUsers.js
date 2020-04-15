var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");

chai.use(chaiHttp);

const CORRECT_INPUT_USER_ID = 2;
const CORRECT_INPUT_USER = {
  email: "frank@ungspiller.no",
  password: "123345678"
};

const CORRECT_INPUT_USER_NEW_PASSWORD = {
  email: "admin@ungspiller.no",
  password: "1233456789"
};

const CORRECT_INPUT_NEW_PASSWORD = {
  password: "1233456789"
};

const WRONG_INPUT_USER_ID = 420;
const WRONG_INPUT_PASSWORD = {
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

describe("Change password", () => {
  before(() => init());

  it("PUT | Password got changed successfully", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + CORRECT_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_NEW_PASSWORD)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        done();
      });
  });

  it("POST | Logging in with new password", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/login")
      .send(CORRECT_INPUT_USER_NEW_PASSWORD)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        res.should.have.status(200);
        res.body.should.have.property("accessToken");
        done();
      });
  });

  it("PUT | The user doesn't exist", done => {
    chai
      .request(process.env.APP_URL)
      .put("/api/users/" + WRONG_INPUT_USER_ID)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(CORRECT_INPUT_NEW_PASSWORD)
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
      .send(CORRECT_INPUT_NEW_PASSWORD)
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
      .send(WRONG_INPUT_PASSWORD)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });
});

describe("Azure Info", () => {
  before(() => init());

  it("should return info", done => {
    chai
      .request(process.env.APP_URL)
      .get("/api/users/azure")
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        res.should.have.status(200);
        res.body.userId.should.not.be.empty;
        res.body.pat.should.not.be.empty;
        res.body.organization.should.not.be.empty;
        done();
      });
  });

  it("should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .get("/api/users/azure")
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + "asdasdasdqweqwe")
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });
});
