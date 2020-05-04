var TokenHandler = require("../TokenHandler");
const chai = require("chai");
const chaiHttp = require("chai-http");
const { expect } = require("chai");
const correctDateFormat = require("../util/validate");

chai.use(chaiHttp);

const TEST_RELEASE_1 = {
  ProductVersionId: 100,
  Title: "Release 2.5 - Vannkanon",
  IsPublic: false,
  ReleaseNotesId: [3, 2],
};

const TEST_RELEASE_2 = {
  ProductVersionId: 101,
  Title: "Release 2.6 - Vannkanon",
  IsPublic: true,
  ReleaseNotesId: [3],
};

const TEST_RELEASE_NEW_RELEASE_NOTE = {
  isPublic: false,
  title: "Release-7",
  ProductVersionId: 100,
  releaseNotes: [
    {
      AuthorEmail: "markuran@ntnu.no",
      AuthorName: "Markus Randa",
      ClosedDate: "2020-03-26T18:38:58.993Z",
      WorkItemDescriptionHtml:
        '<div>Wireframe:</div><div><a href="https://confluence.uials.no/pages/viewpage.action?pageId=57378685">https://confluence.uials.no/pages/viewpage.action?pageId=57378685</a></div><div><br></div><div>ERD:</div><div><a href="https://confluence.uials.no/pages/viewpage.action?pageId=57377417">https://confluence.uials.no/pages/viewpage.action?pageId=57377417</a><br></div>',
      WorkitemId: 235,
      WorkItemTitle: "Front-end: Oppdater state med azure-projects",
    },
  ],
};

const TEST_RELEASE_3 = {
  IsPublic: false,
};

var accessToken;
const AZURE = "azure";
const addressReleases = "/api/releases";
const addressPut = "/api/releases/101";
const addressCheckAfterCreate = "/api/releases/103";
const addressGet = "/api/releases/102";
const addressGetFail = "/api/releases/1002";
const addressDelete = "/api/releases/105";
const addressDeleteFail = "/api/releases/1003";

const init = () => {
  accessToken = TokenHandler.getAccessToken();
};

describe("Releases POST", () => {
  before(() => init());

  // should return 401 unauth
  it("CREATE | Tries to create a release without being logged in", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleases)
      .send(TEST_RELEASE_1)
      .end((err) => {
        err.should.have.status(401);
        done();
      });
  });

  // should return a 401 unauth
  it("PUT | Tries to update a release without being logged in", (done) => {
    chai
      .request(process.env.APP_URL)
      .put(addressPut)
      .send(TEST_RELEASE_3)
      .end((err) => {
        err.should.have.status(401);
        done();
      });
  });

  // should return 200 OK
  it("CREATE | Successfully creating a release", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleases)
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

  // should return 200 OK
  it("CREATE | Successfully creating a release with non existing Release Note", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleases)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_NEW_RELEASE_NOTE)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        res.body.releaseNotes.should.be.a("array");
        res.body.releaseNotes.should.not.be.empty;
        res.body.releaseNotes.should;
        done();
      });
  });

  // should return 400 Bad Request
  it("CREATE | Tries to create a already created release", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleases)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_1)
      .end((err) => {
        err.should.have.status(400);
        done();
      });
  });

  it("POST | Should create release with raw workItems", (done) => {
    chai
      .request(process.env.APP_URL)
      .post(addressReleases + "/" + AZURE)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .send(TEST_RELEASE_RAW_WORKITEMS)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        expect(res.body.title).to.equal(TEST_RELEASE_RAW_WORKITEMS.title);
        expect(res.body.productVersion.id).to.equal(
          TEST_RELEASE_RAW_WORKITEMS.ProductVersionId
        );
        expect(res.body.isPublic).to.equal(TEST_RELEASE_RAW_WORKITEMS.isPublic);
        console.log(res.body);

        // Test for release notes
        const releaseNote = res.body.releaseNotes[0];
        // expect(releaseNote.title).to.equal(
        //   TEST_RELEASE_RAW_WORKITEMS.releaseNotes[0].fields["System.Title"]
        // );
        expect(releaseNote.workItemTitle).to.equal(
          TEST_RELEASE_RAW_WORKITEMS.releaseNotes[0].fields["System.Title"]
        );
        // expect(releaseNote.ingress);
        // expect(releaseNote.description);
        expect(releaseNote.workItemId).to.equal(
          TEST_RELEASE_RAW_WORKITEMS.releaseNotes[0].id
        );
        expect(releaseNote.workItemDescriptionHtml).to.be.a("string");
        expect(correctDateFormat(releaseNote.closedDate)).to.be.false;
        expect(releaseNote.isPublic).to.equal(false);

        done();
      });
  });

  it("PUT | Should return same object as trying to put ", (done) => {
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

  it("PUT | Should return non null fields", (done) => {
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

  it("PUT | Should handle unknown values", (done) => {
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
        expect(res.body.releaseNotes.length).to.equal(1);
        expect(res.body.releaseNotes[0].id).to.equal(
          TEST_RELEASE_2.ReleaseNotesId[0]
        );
        done();
      });
  });
});
describe("Releases GET", () => {
  before(() => init());

  it("Should return all releases", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(addressReleases)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.should.have.status(200);
        res.body.should.be.a("array").that.is.not.empty;
        res.body[0].productVersion.should.exist;
        res.body[0].productVersion.product.should.exist;
        res.body[0].releaseNotes.should.exist;
        res.body[0].title.should.exist;
        done();
      });
  });

  it("GET | Attempting to get a non-existant release", (done) => {
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

  // success case for getting a single release
  it("GET | Should return a specific release", (done) => {
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
        expect(res.body.isPublic).to.equal(true);
        expect(res.body.productVersion).to.be.not.empty;
        expect(res.body.releaseNotes).to.be.a("array");
        expect(res.body.releaseNotes).to.be.empty;
        expect(correctDateFormat(res.body.date)).to.be.false;
        done();
      });
  });

  it("GET | Only retrieve Releases that's public", (done) => {
    chai
      .request(process.env.APP_URL)
      .get(addressReleases)
      .end((err, res) => {
        if (err) {
          done(err.response.text);
        }
        res.body.map((r) => {
          if (r.productVersion) {
            expect(r.productVersion.isPublic).to.equal(true);
          }
          expect(r.isPublic).to.equal(true);
        });
        done();
      });
  });
});

describe("Releases DELETE", () => {
  before(() => init());
  it("DELETE | Tries to delete a release without logging in", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDelete)
      .end((err) => {
        expect(err.should.have.status(401));
        done();
      });
  });

  it("DELETE | Tries to delete a non-existant release", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(addressDeleteFail)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err) => {
        expect(err.should.have.status(400));
        done();
      });
  });

  // success case: deletes a release
  it("DELETE | Should delete a release and return a copy of the deleted release", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(addressCheckAfterCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err, res) => {
        if (err) {
          done(err.response);
        }
        res.should.have.status(200);
        expect(res.body.id).to.equal(103);
        expect(res.body.title).to.equal(TEST_RELEASE_2.Title);
        expect(res.body.isPublic).to.equal(TEST_RELEASE_3.IsPublic);
        expect(res.body.productVersion.id).to.equal(
          TEST_RELEASE_2.ProductVersionId
        );
        expect(res.body.releaseNotes[0].id).to.equal(
          TEST_RELEASE_2.ReleaseNotesId[0]
        );
        done();
      });
  });

  // failure case: Tries to delete a already deleted release
  it("DELETE | Tries to delete a already deleted release", (done) => {
    chai
      .request(process.env.APP_URL)
      .delete(addressCheckAfterCreate)
      .set("Content-Type", "application/json")
      .set("Authorization", "Bearer " + accessToken)
      .end((err) => {
        expect(err.should.have.status(400));
        done();
      });
  });
});

const TEST_RELEASE_RAW_WORKITEMS = {
  isPublic: false,
  title: "Release-6",
  ProductVersionId: 100,
  releaseNotes: [
    {
      id: 327,
      rev: 5,
      fields: {
        "System.AreaPath": "Release Note System",
        "System.TeamProject": "Release Note System",
        "System.IterationPath": "Release Note System\\Sprint 7",
        "System.WorkItemType": "Task",
        "System.State": "In Progress",
        "System.Reason": "Work started",
        "System.AssignedTo": {
          displayName: "Lars Øyvind Ous",
          url:
            "https://spsprodweu1.vssps.visualstudio.com/A025f0995-ba99-4e30-998a-739dc40106c6/_apis/Identities/c09ed437-6953-687c-91e6-fa55bef8daf7",
          _links: {
            avatar: {
              href:
                "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
            },
          },
          id: "c09ed437-6953-687c-91e6-fa55bef8daf7",
          uniqueName: "larsous@ntnu.no",
          imageUrl:
            "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
          descriptor: "aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
        },
        "System.CreatedDate": "2020-04-21T11:26:52.2Z",
        "System.CreatedBy": {
          displayName: "Markus Randa",
          url:
            "https://spsprodweu1.vssps.visualstudio.com/A025f0995-ba99-4e30-998a-739dc40106c6/_apis/Identities/daab4d46-973a-64b4-9795-742508e96bfc",
          _links: {
            avatar: {
              href:
                "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.ZGFhYjRkNDYtOTczYS03NGI0LTk3OTUtNzQyNTA4ZTk2YmZj",
            },
          },
          id: "daab4d46-973a-64b4-9795-742508e96bfc",
          uniqueName: "markuran@ntnu.no",
          imageUrl:
            "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.ZGFhYjRkNDYtOTczYS03NGI0LTk3OTUtNzQyNTA4ZTk2YmZj",
          descriptor: "aad.ZGFhYjRkNDYtOTczYS03NGI0LTk3OTUtNzQyNTA4ZTk2YmZj",
        },
        "System.ChangedDate": "2020-04-23T12:57:21.443Z",
        "System.ChangedBy": {
          displayName: "Lars Øyvind Ous",
          url:
            "https://spsprodweu1.vssps.visualstudio.com/A025f0995-ba99-4e30-998a-739dc40106c6/_apis/Identities/c09ed437-6953-687c-91e6-fa55bef8daf7",
          _links: {
            avatar: {
              href:
                "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
            },
          },
          id: "c09ed437-6953-687c-91e6-fa55bef8daf7",
          uniqueName: "larsous@ntnu.no",
          imageUrl:
            "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
          descriptor: "aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
        },
        "System.CommentCount": 0,
        "System.Title":
          "Back-end: Når en ReleaseNote opprettes blir mappingTable brukt til mapping",
        "Microsoft.VSTS.Common.StateChangeDate": "2020-04-22T08:55:20.503Z",
        "Microsoft.VSTS.Common.ActivatedDate": "2020-04-22T08:55:20.503Z",
        "Microsoft.VSTS.Common.ActivatedBy": {
          displayName: "Lars Øyvind Ous",
          url:
            "https://spsprodweu1.vssps.visualstudio.com/A025f0995-ba99-4e30-998a-739dc40106c6/_apis/Identities/c09ed437-6953-687c-91e6-fa55bef8daf7",
          _links: {
            avatar: {
              href:
                "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
            },
          },
          id: "c09ed437-6953-687c-91e6-fa55bef8daf7",
          uniqueName: "larsous@ntnu.no",
          imageUrl:
            "https://dev.azure.com/ReleaseNoteSystem/_apis/GraphProfile/MemberAvatars/aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
          descriptor: "aad.YzA5ZWQ0MzctNjk1My03ODdjLTkxZTYtZmE1NWJlZjhkYWY3",
        },
        "Microsoft.VSTS.Common.Priority": 2,
        "System.Description": "<div>Testnes<br></div>",
      },
      url:
        "https://dev.azure.com/ReleaseNoteSystem/399f705f-cd58-45f2-becb-f890cb50f774/_apis/wit/workItems/327",
    },
  ],
};
