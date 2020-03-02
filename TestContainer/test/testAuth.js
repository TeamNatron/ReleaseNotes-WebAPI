var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const should = chai.should();

var tokenHandler;
var accessToken;

chai.use(chaiHttp);

const AUTH_USER = {
  email: "admin@ungspiller.no",
  password: "12345678"
};

const UN_AUTH_USER = {
  email: "admin@ungspiller.se",
  password: "12345678"
};

const init = () => {
  this.tokenHandler = TokenHandler;
};

describe("Login", () => {
  before(() => init());

  it("Should return bad request", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/login")
      .send(UN_AUTH_USER)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });

  it("Should return token", done => {
    chai
      .request(process.env.APP_URL)
      .post("/api/login")
      .send(AUTH_USER)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        res.should.have.status(200);
        res.body.should.have.property("accessToken");
        this.tokenHandler.setAccessToken(res.body.accessToken);
        done();
      });
  });
});
