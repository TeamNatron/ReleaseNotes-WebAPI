var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");

chai.use(chaiHttp);

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

const ADDRESS_MAPPED_FIELDS = "/api/mappablefields/";
const ID_PUT_MAPPED_FIELD = "1";

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
        expect(res.should.have.status(200));
        expect(res.body.entity).to.be.a("array");
        expect(res.body.entity[0].id).to.be.a("number");
        expect(res.body.entity[0].mappableFieldId).to.be.a("number");
        expect(res.body.entity[0].mappableField).to.be.a("object");

        const mappableFieldObject = res.body.entity[0].mappableField;
        expect(mappableFieldObject.id).to.be.a("number");
        expect(mappableFieldObject.name).to.be.a("string").that.is.not.empty;
        done();
      });
  });
});

//-----------------------------------------------------------------------------------------------------------------

describe("Mappings PUT", () => {
  before(() => init());

  it("should return unauthorized", (done) => {
    chai
      .request(process.env.APP_URL)
      .put(ADDRESS_MAPPED_FIELDS + ID_PUT_MAPPED_FIELD)
      .send(MAPPED_FIELD_OBJECT)
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  it("should return success with a newly mapped field", (done) => {
    chai
      .request(process.env.APP_URL)
      .put(ADDRESS_MAPPED_FIELDS + ID_PUT_MAPPED_FIELD)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(MAPPED_FIELD_OBJECT)
      .end((err, res) => {
        if (err) {
          done(err);
        }
        console.log(res.body);
        expect(res.should.have.status(200));
        expect(res.body.entity.id).to.be.a("number");
        expect(res.body.entity.azureDevOpsField).to.equal(
          MAPPED_FIELD_OBJECT.azureDevOpsField
        );
        expect(res.body.entity.mappableFieldId).to.equal(
          parseInt(ID_PUT_MAPPED_FIELD)
        );

        expect(res.body.entity.mappableField).to.be.a("object");
        const mappableFieldObject = res.body.entity.mappableField;
        expect(mappableFieldObject.id).to.be.a("number");
        expect(mappableFieldObject.name).to.be.a("string").that.is.not.empty;
        done();
      });
  });
});
