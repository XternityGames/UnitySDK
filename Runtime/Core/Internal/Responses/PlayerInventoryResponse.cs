namespace Xternity.REST.Responses
{
    using System;
    using System.Net;
    using Newtonsoft.Json;

    public partial record PlayerInventoryResponse : IRESTResponse<PlayerInventoryResponse>
    {
        // [JsonProperty("status", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public static PlayerInventoryResponse FromJson(string json) => JsonFacade.FromJson<PlayerInventoryResponse>(json);
        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);


        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public PlayerInventoryResponse_Data Data { get; set; }

        [JsonProperty("meta")]
        public PlayerInventoryResponse_Meta Meta { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public partial record PlayerInventoryResponse_Data
        {
            [JsonProperty("results")]
            public PlayerInventoryResponse_Result[] Results { get; set; }
        }

        public partial record PlayerInventoryResponse_Result
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("gameId")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long GameId { get; set; }

            [JsonProperty("tokenId")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long TokenId { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("createdAt")]
            public DateTimeOffset CreatedAt { get; set; }

            [JsonProperty("gameTokenId")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long GameTokenId { get; set; }

            [JsonProperty("attributes")]
            public PlayerInventoryResponse_Attributes Attributes { get; set; }

            [JsonProperty("transactionHash")]
            public string TransactionHash { get; set; }

            [JsonProperty("marketUrl")]
            public Uri MarketUrl { get; set; }

            [JsonProperty("chainName")]
            public string ChainName { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("imageUrl")]
            public Uri ImageUrl { get; set; }
        }

        public partial record PlayerInventoryResponse_Attributes
        {
            [JsonProperty("Power")]
            public long Power { get; set; }

            [JsonProperty("Class", NullValueHandling = NullValueHandling.Ignore)]
            public string Class { get; set; }

            [JsonProperty("Rarity", NullValueHandling = NullValueHandling.Ignore)]
            public string Rarity { get; set; }

        }

        public partial record PlayerInventoryResponse_Meta
        {
        }
    }
}
