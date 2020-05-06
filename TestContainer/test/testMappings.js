var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

const WORK_ITEM_TYPE_TASK = "task";
const ADDRESS_MAPPED_FIELDS = "/api/mappablefields/";
const NAME_PUT_MAPPED_FIELD = "Title";

var mappedFields = [];

const MAPPED_FIELD_OBJECT = {
  azureDevOpsField: "SuperPower",
};

//-----------------------------------------------------------------------------------------------------------------

describe("Mappings GET", () => {
  before(() => init());

  it("should return unauthorized", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_MAPPED_FIELDS)
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  it("should return unauthorized", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_MAPPED_FIELDS)
      .query({ mapped: true })
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  it("should return all mappable fields", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_MAPPED_FIELDS)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        expect(res.should.have.status(200));
        expect(res.body.entity).to.be.a("array");
        expect(res.body.entity[0].id).to.be.a("number");
        expect(res.body.entity[0].name).to.be.a("string").that.is.not.empty;
        done();
      });
  });

  it("should return mapped fields", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(ADDRESS_MAPPED_FIELDS)
      .query({ mapped: true })
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        // Get original mapping for later use
        mappedFields = res.body.entity;

        expect(res.should.have.status(200));
        expect(res.body.entity).to.be.a("array");
        expect(res.body.entity[0].mappableField).to.be.a("string");
        done();
      });
  });
});

//-----------------------------------------------------------------------------------------------------------------

describe("Mappings PUT", () => {
  before(() => init());
  const url =
    ADDRESS_MAPPED_FIELDS + WORK_ITEM_TYPE_TASK + "/" + NAME_PUT_MAPPED_FIELD;

  it("should return unauthorized", (done) => {
    chai
      .request(process.env.APP_URL)
      .put(url)
      .send(MAPPED_FIELD_OBJECT)
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  it("should return success with a newly mapped field", (done) => {
    chai
      .request(process.env.APP_URL)
      .put(url)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(MAPPED_FIELD_OBJECT)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        expect(res.should.have.status(200));
        expect(res.body.entity.azureDevOpsField).to.equal(
          MAPPED_FIELD_OBJECT.azureDevOpsField
        );
        expect(res.body.entity.mappableField).to.equal(NAME_PUT_MAPPED_FIELD);
        expect(res.body.entity.mappableType).to.equal(WORK_ITEM_TYPE_TASK);
        done();
      });
  });

  it("should dispose of title mapping", (done) => {
    // Retrieves
    const obj = mappedFields.find(
      (obj) =>
        obj.mappableField.toUpperCase() === "TITLE" &&
        obj.mappableType.toUpperCase() === WORK_ITEM_TYPE_TASK.toUpperCase()
    );
    const MAPPED_FIELD_OBJECT_ORIGINAL = {
      azureDevOpsField: obj.azureDevOpsField,
    };
    chai
      .request(process.env.APP_URL)
      .put(url)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(MAPPED_FIELD_OBJECT_ORIGINAL)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        expect(res.should.have.status(200));
        expect(res.body.entity.azureDevOpsField).to.equal(
          MAPPED_FIELD_OBJECT_ORIGINAL.azureDevOpsField
        );
        expect(res.body.entity.mappableField).to.equal(NAME_PUT_MAPPED_FIELD);
        expect(res.body.entity.mappableType).to.equal(WORK_ITEM_TYPE_TASK);
        done();
      });
  });
});
