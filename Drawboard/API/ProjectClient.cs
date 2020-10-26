using Drawboard.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace Drawboard.API
{
    /// <summary>
    /// Project client implementation using Drawboard REST endpoints.
    /// </summary>
    public class ProjectClient : IProjectClient
    {
        private readonly IHttpClient _client;

        /// <summary>
        /// HTTP REST host.
        /// </summary>
        public Uri Host { get; set; } = new Uri("https://preprod-api.bullclip.com", UriKind.Absolute);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client">Http client used to query REST endpoints.</param>
        public ProjectClient(IHttpClient client)
        {
            if (null == client)
                throw new ArgumentNullException("client");
            _client = client;
        }

        /// <summary>
        /// Query for the authenticated users projects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>> GetUserProjectsAsync()
        {
            try
            {
                // query for the project data
                var getAsyncOut = await _client.GetAsync(new Uri(Host, "/api/v1/project/my"));
                getAsyncOut.EnsureSuccessStatusCode();

                // read response content and decode JSON array
                var jsonStr = await getAsyncOut.Content.ReadAsStringAsync();
                JsonArray json;
                if (!JsonArray.TryParse(jsonStr, out json))
                    throw new FormatException("invalid JSON");

                // map each item to a project entity.
                return new List<Project>(json.Select(toProject).Where(x => null != x));
            }
            catch (Exception e)
            {
                throw new Exception("failed reading user projects", e);
            }
        }

        private static Project toProject(IJsonValue x)
        {
            if (x.ValueType != JsonValueType.Object)
                return null;
            var obj = x.GetObject();
            var p = new Project
            {
                ID = obj.GetNamedString("id"),
                Name = obj.GetNamedString("name"),
                Description = obj.GetNamedString("description"),
            };
            IJsonValue ownerId;
            if (obj.TryGetValue("ownerId", out ownerId) && ownerId.ValueType == JsonValueType.String)
            {
                p.OwnerID = ownerId.GetString();
            }
            return p;
        }

        public async Task<Uri> GetProjectLogo(string parentProjectID)
        {
            try
            {
                // query for the project data
                var buffer = await _client.GetBufferAsync(new Uri(Host, $"/api/v1/project/{parentProjectID}/logo.png"));

                // read PNG
                var base64 = Convert.ToBase64String(buffer.ToArray());
                return new Uri($"data:image/png;base64,{base64}");
            }
            catch (Exception e)
            {
                throw new Exception("failed reading project logo", e);
            }
        }
    }
}
