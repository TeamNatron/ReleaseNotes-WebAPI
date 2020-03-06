var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);

const TEST_RELEASE_1 = {
  ProductVersionId: 100,
  Title: "Release 2.5 - Vannkanon",
  IsPublic: false,
  ReleaseNotesId: [1, 2]
};

const TEST_RELEASE_2 = {
  ProductVersionId: 101,
  Title: "Release 2.6 - Vannkanon",
  IsPublic: true,
  ReleaseNotesId: [1]
};

const TEST_RELEASE_3 = {
  IsPublic: false
};

var accessToken;
const addressCreate = "/api/releases";
const addressPut = "/api/releases/101";
const addressCheckAfterCreate = "/api/releases/103";
const addressGet = "/api/releases/102";
const addressGetFail = "/api/releases/1002";
const addressDelete = "/api/releases/103";
const addressDeleteFail = "/api/releases/1003";

const init = () => {
  accessToken = TokenHandler.getAccessToken();
  console.log(addressPut);
};

describe("Releases POST", () => {
  before(() => init());

  it("CREATE | Should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .post(addressCreate)
      .send(TEST_RELEASE_1)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  it("PUT | Should return unauthorized", done => {
    chai
      .request(process.env.APP_URL)
      .put(addressPut)
      .send(TEST_RELEASE_3)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  it("CREATE | Should return created", done => {
    chai
      .request(process.env.APP_URL)
      .post(addressCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_1)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        done();
      });
  });

  it("CREATE | Should return release name already in use", done => {
    chai
      .request(process.env.APP_URL)
      .post(addressCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_1)
      .end(err => {
        err.should.have.status(400);
        done();
      });
  });

  it("PUT | Should return same object as trying to put ", done => {
    chai
      .request(process.env.APP_URL)
      .put(addressCheckAfterCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_2)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        expect(res.body.productVersion.id).to.equal(
          TEST_RELEASE_2.ProductVersionId
        );
        expect(res.body.title).to.equal(TEST_RELEASE_2.Title);
        expect(res.body.isPublic).to.equal(TEST_RELEASE_2.IsPublic);
        expect(res.body.releaseNotes[0].id).to.equal(
          TEST_RELEASE_2.ReleaseNotesId[0]
        );
        done();
      });
  });

  it("PUT | Should return non null fields", done => {
    chai
      .request(process.env.APP_URL)
      .put(addressCheckAfterCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_3)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        expect(res.body.productVersion.id).to.equal(
          TEST_RELEASE_2.ProductVersionId
        );
        expect(res.body.title).to.equal(TEST_RELEASE_2.Title);
        expect(res.body.isPublic).to.equal(TEST_RELEASE_3.IsPublic);
        expect(res.body.releaseNotes[0].id).to.equal(
          TEST_RELEASE_2.ReleaseNotesId[0]
        );
        done();
      });
  });
});
describe("Releases GET", () => {
  before(() => init());

  it("Should return all releases", done => {
    chai
      .request(process.env.APP_URL)
      .get(address)
      .end((err, res) => {
        console.log(["RECEIVED DATA: ", res.text]);
        res.should.have.status(200);
        res.body.should.be.a("array").that.is.not.empty;
        res.body[0].productVersion.should.exist;
        res.body[0].productVersion.product.should.exist;
        res.body[0].releaseNotes.should.exist;
        res.body[0].title.should.exist;
        done();
      });
  });

  // failure case for getting a single release due to not logging in, 401 unauth
  it("GET | Should return a 401 unauth", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .end(err => {
        err.should.have.status(401);
        done();
      });
  });

  // failure case: Logged in and attempting to get a release that does not exist
  it("GET | Should return a 204 No Content", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetFail)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(204);
        done();
      });
  });

  // failure case: not logged in and attempting to get a release that does not exist
  it("GET | Should return a 401 unauth", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGetFail)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // success case for getting a single release
  it("GET | Should return a specific release", done => {
    chai
      .request(process.env.APP_URL)
      .get(addressGet)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        expect(res.body.id).to.equal(102);
        expect(res.body.title).to.equal("Chief Keef");
        expect(res.body.isPublic).to.equal(true);
        expect(res.body.productVersion).to.be.not.empty;
        expect(res.body.releaseNotes).to.be.a("array");
        expect(res.body.releaseNotes).to.be.empty;
        done();
      });
  });
});

describe("Releases DELETE", () => {
  before(() => init());
  // failure case: Tries to delete without logging in
  it("DELETE | Should return a 401 unauth", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // failure case: Tries to delete a non-existant release
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

  // failure case: Tries to delete a non-existant release without logging in
  it("DELETE | Should return a 401 unath", done => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDeleteFail)
      .end(err => {
        expect(err.should.have.status(401));
        done();
      });
  });

  // success case: deletes a release
  it("DELETE | Should return a ok 200, and a copy of the deleted release", done => {
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
        expect(res.body.id).to.equal(103);
        expect(res.body.title).to.equal("Release 2.6 - Vannkanon");
        expect(res.body.isPublic).to.equal(false);
        expect(res.body.productVersion).to.be.not.empty;
        expect(res.body.releaseNotes).to.be.a("array").that.is.not.empty;
        done();
      });
  });

  // failure case: Tries to delete a already deleted release
  it("DELETE | Should return 400 and a error message", done => {
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
