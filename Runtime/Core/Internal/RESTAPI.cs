using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xternity.REST.Responses;

namespace Xternity.REST
{
    internal class RESTAPI : IDisposable
    {

        RestClient _client;
        JObject _json;

        private static RESTAPI _shared;
        public static RESTAPI Shared
        {
            get
            {
                if (_shared == null)
                {
                    throw new InvalidOperationException("Could not perform API action, please check you have initialized the SDK.");
                }

                return _shared;
            }
        }

        internal RESTAPI(string apiUrl)
        {
            _client = new RestClient(apiUrl);
            _shared = this;
        }

        public async Task<T> ExecuteRequest<T>(Method method, string endpoint, JObject body = null, Dictionary<string, string> headers = null, Dictionary<string, string> segments = null) where T : IRESTResponse<T>
        {
            var request = new RestRequest(endpoint, method);

            if (headers != null)
                request.AddHeaders(headers);

            if (body != null)
                request.AddStringBody(body.ToString(), ContentType.Json);

            if (segments != null)
            {
                foreach (var segment in segments)
                {
                    request.AddUrlSegment(segment.Key, segment.Value);
                }
            }

            RestResponse<T> response = await _client.ExecuteAsync<T>(request);

            if (response.Data == null)
            {
                // log error
            }

            return response.Data;
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        #region API

        public async Task<T> PlayerLogin<T>(string authProvider, string token) where T : IRESTResponse<T>
        {
            _json = new JObject
            {
                { "authProvider", authProvider },
                { "token", token }
            };

            return await ExecuteRequest<T>(Method.Post, "/v1/auth/login", _json,
                new Dictionary<string, string>() { { "Content-Type", "application/json" } });
        }

        public async Task<T> GetPlayerInfo<T>() where T : IRESTResponse<T>
        {
            return await ExecuteRequest<T>(Method.Get, "/v1/players/me",
                headers: new Dictionary<string, string>() { { "Authorization", "Bearer " + Auth.PlayerAuthToken } });
        }

        public async Task<T> GetPlayerInventory<T>() where T : IRESTResponse<T>
        {
            return await ExecuteRequest<T>(Method.Get, "/v1/players/me/assets?gName=robo_rollers&cached=true",
                headers: new Dictionary<string, string>() { { "Authorization", "Bearer " + Auth.PlayerAuthToken } });
        }

        public async Task<T> SendAssetToUser<T>(string identifier, string value, string assetID, string tokenID) where T : IRESTResponse<T>
        {
            return await ExecuteRequest<T>(
                Method.Get,
                "/v1/game-tokens/{assetID}/tokens/{tokenID}/send",
                headers: new Dictionary<string, string>() { { "Authorization", "Bearer " + Auth.PlayerAuthToken } },
                body: new JObject()
                {
                    {"identifier", identifier },
                    {"value", value }
                },
                segments: new Dictionary<string, string>() 
                { 
                    { "assetID", assetID },
                    { "tokenID", tokenID }
                }
                );
        }

        #endregion
    }
}
