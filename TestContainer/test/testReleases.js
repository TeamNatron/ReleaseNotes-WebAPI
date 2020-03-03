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

const init = () => {
  accessToken = TokenHandler.getAccessToken();
  console.log(addressPut);
};

describe("Releases", () => {
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
