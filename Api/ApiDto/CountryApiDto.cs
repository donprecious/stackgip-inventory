using System.Collections.Generic;
using Newtonsoft.Json;

namespace StackgipInventory.ApiDto
{
 
    public class CountryApiDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alpha2Code")]
        public string Alpha2Code { get; set; }

        [JsonProperty("alpha3Code")]
        public string Alpha3Code { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("subregion")]
        public string SubRegion { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }
    }

    public class CountryApiResponse : RefitApiResponse
    {
        public new List<CountryApiDto> Data  {get;set;}
    }
}
