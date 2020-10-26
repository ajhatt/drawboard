using Drawboard.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.Web.Http;


namespace Test.API
{

    [TestClass]
    public class ProjectClientTest
    {

        private readonly string projectListJSON = 
            "[{\"id\":\"id-1234\",\"name\":\"name-1234\",\"description\":\"desc-1234\",\"status\":\"None\",\"permissions\":\"string\",\"ownerId\":\"owner-1234\",\"createdDateUtc\":\"2020-10-25T05:01:34.471Z\",\"deletedOn\":\"2020-10-25T05:01:34.471Z\",\"owner\":{\"id\":\"string\",\"email\":\"string\",\"firstName\":\"string\",\"lastName\":\"string\",\"title\":\"string\",\"permissions\":\"string\",\"companyName\":\"string\",\"department\":\"string\",\"dateJoined\":\"string\",\"phone\":\"string\",\"userAlias\":\"string\",\"activationId\":\"00000000-0000-0000-0000-000000000000\",\"isOptInForCommunication\":true,\"hasAcceptedTerms\":true,\"isOptInForProcessing\":true,\"accountActivated\":true,\"persona\":\"Undefined\",\"segmentId\":\"string\"},\"drawingCount\":0,\"documentCount\":0,\"userCount\":0,\"issuesCount\":0,\"organizationId\":\"string\"}]";

        class MockHttpClient : IHttpClient
        {
            public HttpResponseMessage GetAsyncResp { get; set; }

            public Task<HttpResponseMessage> GetAsync(Uri uri)
            {
                return Task.FromResult(this.GetAsyncResp);
            }
        }


        [TestMethod]
        public async Task GetProjects()
        {
            // arrange
            var mockHttpClient = new MockHttpClient();
            mockHttpClient.GetAsyncResp = new HttpResponseMessage(HttpStatusCode.Ok);
            mockHttpClient.GetAsyncResp.Content = new HttpStringContent(projectListJSON, UnicodeEncoding.Utf8, "application/json");
            var client = new ProjectClient(mockHttpClient);

            // act
            var projects = await client.GetUserProjectsAsync();

            // assert
            Assert.AreEqual(1, projects.Count);
            Assert.AreEqual("id-1234", projects[0].ID);
            Assert.AreEqual("name-1234", projects[0].Name);
            Assert.AreEqual("desc-1234", projects[0].Description);
            Assert.AreEqual("owner-1234", projects[0].OwnerID);
        }
    }
}
