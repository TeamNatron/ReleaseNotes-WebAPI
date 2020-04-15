var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);

var accessToken;
const ADDRESS_GET_RELEASE_NOTE = "/api/releasenote";
const ADDRESS_GET_RELEASE_NOTE_1 = ADDRESS_GET_RELEASE_NOTE + "/1";
const ADDRESS_GET_RELEASE_NOTE_NON_EXISTING =
  ADDRESS_GET_RELEASE_NOTE + "/23123";

const RELEASE_NOTE_WITHOUT_DESCRIPTION = {
  title: "Ny dag, ny release note",
  ingress:
    "Det er en tirsdag i 2020. Det neste du leser vil absolutt blåse minnet ditt.",
  isPublic: "false",
  authorName: "postman@ungspiller.no",
};

const CORRECT_DATE_FILTER =
  "?startDate=2019-11-04T09:50:00.000Z&endDate=2020-03-11T09:50:33.789Z";
const startDate = new Date("2019-11-04T09:50:00.000Z");
const endDate = new Date("2020-03-11T09:50:33.789Z");

var noteToDelete;

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Release notes GET", () => {
  before(() => init());

  // should return 200 OK and a array of release notes
  it("GET | Get all release notes without logging in", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE)
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
        expect(res.body[0].isPublic).to.equal(true);
        expect(res.body[0].workItemId).to.be.a("number");
        res.should.have.status(200);
        done();
      });
  });

  // should return 200 OK and  a singe release note
  it("GET | Successfully gets a single release note without logging in", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE_1)
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

  // should return 200 OK and  a singe release note
  it("GET | Successfully gets a single release note with associated releases", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE_1)
      .query("includeReleases")
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
        expect(res.body.releases).to.be.a("array").that.is.not.empty;
        done();
      });
  });

  // should return a 204 No Content
  it("GET | requests a non-existant release note", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE_NON_EXISTING)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(204);
        done();
      });
  });

  it("GET | Get Release Notes from a set date interval", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE + CORRECT_DATE_FILTER)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        // Checking if each object's date corresponds to filter
        res.body.map((obj) => {
          const date = new Date(obj.closedDate);
          expect(date.getTime()).to.satisfy((num) => {
            return num >= startDate.getTime() && num <= endDate.getTime();
          });
        });
        done();
      });
  });

  it("GET | Only retrieve Release Notes that's public", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_GET_RELEASE_NOTE)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.body.map((rn) => {
          expect(rn.isPublic).to.equal(true);
        });
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

  it("POST | Tries to create release note without providing a description", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(ADDRESS_GET_RELEASE_NOTE)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(RELEASE_NOTE_WITHOUT_DESCRIPTION)
      .end((err) => {
        expect(err.should.have.status(400));
        done();
      });
  });
});

describe("Release notes DELETE", () => {
  before(() => init());

  // failure case: Tries to delete a release note without being logged in
  // should return a 401 unauth
  it("DELETE | Tries to delete a release note without being logged in", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(ADDRESS_GET_RELEASE_NOTE_1)
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // failure case: Tries to delete a non-existant release note
  // should return a 400 Bad Request
  it("DELETE | Tries to delete a non-existant release note", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(ADDRESS_GET_RELEASE_NOTE_NON_EXISTING)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err) => {
        expect(err.should.have.status(400));
        done();
      });
  });

  // succes case: deletes a release note
  // should return a 200 OK and a copy of the newly deleted release note
  it("DELETE | Should return a OK 200 and a copy of the deleted release note", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(ADDRESS_GET_RELEASE_NOTE_1)
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
  it("DELETE | Tries to delete a already deleted release note", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(ADDRESS_GET_RELEASE_NOTE_1)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err) => {
        expect(err.should.have.status(400));
        done();
      });
  });
});
