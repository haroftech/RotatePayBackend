using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class IPLocation
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("metro_code")]
        public int MetroCode { get; set; }

        private IPLocation() { }

        public static async Task<IPLocation> QueryGeographicalLocationAsync(string ipAddress)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://api.ipstack.com/" + ipAddress + "?access_key=0f849cb67713e7a446c61ed2935f677f");
            //string result = await client.GetStringAsync("http://api.ipstack.com/197.210.53.200?access_key=0f849cb67713e7a446c61ed2935f677f");

            return JsonConvert.DeserializeObject<IPLocation>(result);
        }
    }
}
