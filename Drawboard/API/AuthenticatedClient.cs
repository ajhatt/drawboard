using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.AtomPub;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Drawboard.API
{
    /// <summary>
    /// Authenticated HTTP client.
    ///
    /// Lazily authenticates a wrapped HttpClient instance against the Drawboard API.
    /// </summary>
    public class AuthenticatedClient : IHttpClient
    {
        private readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// HTTP REST host.
        /// </summary>
        public Uri Host { get; set; } = new Uri("https://preprod-api.bullclip.com", UriKind.Absolute);

        /// <summary>
        /// Username to authenticate with.
        /// </summary>
        public string Username { get; set; } = "interview-test@drawboard.com";

        /// <summary>
        /// Password to authenticate with.
        /// </summary>
        public string Password { get; set; } = "drawboard-test";

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            var resp = await _client.GetAsync(uri);
            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                await refreshAccessToken();
                return await _client.GetAsync(uri);
            }
            return resp;
        }

        public async Task<IBuffer> GetBufferAsync(Uri uri)
        {
            try
            {
                return await _client.GetBufferAsync(uri);
            }
            catch (Exception)
            {
                await refreshAccessToken();
                return await _client.GetBufferAsync(uri);
            }
        }


        private async Task refreshAccessToken()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // form json to POST
                    var json = new JsonObject();
                    json.SetNamedValue("username", JsonValue.CreateStringValue(Username));
                    json.SetNamedValue("password", JsonValue.CreateStringValue(Password));

                    // hit the endpoint
                    var postAsyncResult = await client.PostAsync(
                        new Uri(this.Host, "/api/v1/auth/login"),
                        new HttpStringContent(
                            json.ToString(),
                            Windows.Storage.Streams.UnicodeEncoding.Utf8,
                            "application/json"));
                    postAsyncResult.EnsureSuccessStatusCode();

                    // parse json & get access token
                    var resultJsonStr = await postAsyncResult.Content.ReadAsStringAsync();
                    JsonObject jsonResp;
                    if (!JsonObject.TryParse(resultJsonStr, out jsonResp))
                        throw new FormatException("failed to parse authentication token");
                    var accessToken = jsonResp.GetNamedString("accessToken");

                    // update clients bearer token
                    _client.DefaultRequestHeaders.Authorization =
                        new HttpCredentialsHeaderValue("Bearer", accessToken);
                }

            }
            catch (Exception e)
            {
                throw new Exception("failed to authenticate client", e);
            }
        }
    }
}
