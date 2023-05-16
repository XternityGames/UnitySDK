using System;
using System.Net;
using Newtonsoft.Json;

namespace Xternity.REST.Responses
{
    public partial record PlayerInfoResponse : IRESTResponse<PlayerInfoResponse>
    {
        // [JsonProperty("status", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public static PlayerInfoResponse FromJson(string json) => JsonFacade.FromJson<PlayerInfoResponse>(json);
        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public PlayerInfoResponse_Data Data { get; set; }

        [JsonProperty("meta")]
        public PlayerInfoResponse_Meta Meta { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public partial record PlayerInfoResponse_Data
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("gameId")]
            public string GameId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("wallets")]
            public PlayerInfoResponse_Wallets Wallets { get; set; }

            [JsonProperty("isEmailVerified")]
            public bool IsEmailVerified { get; set; }

            [JsonProperty("externalPlayerId")]
            public string ExternalPlayerId { get; set; }

            [JsonProperty("createdAt")]
            public DateTimeOffset CreatedAt { get; set; }
        }

        public partial record PlayerInfoResponse_Wallets
        {
            [JsonProperty("evm")]
            public PlayerInfoResponse_Evm Evm { get; set; }
        }

        public partial record PlayerInfoResponse_Evm
        {
            [JsonProperty("address")]
            public string Address { get; set; }
        }

        public partial record PlayerInfoResponse_Meta
        {
        }
    }
}