using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ARTS.SslCommerzeCore
{
    public class SslQuery
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static async Task<TransactionQueryBySessionResponse> TransactionQueryBySessionIdAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentException("SessionId mustn't be empty", nameof(sessionId));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("sessionkey", sessionId);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_PASS);
                parameters.Add("format", "json");

                StringBuilder sb = new StringBuilder();
                sb.Append(requestUrl + "?");
                foreach (KeyValuePair<string, string> item in parameters)
                {
                    sb.Append(item.Key + "=" + item.Value + "&");
                }
                requestUrl = sb.ToString().TrimEnd('&');

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                TransactionQueryBySessionResponse refundResult = JsonConvert.DeserializeObject<TransactionQueryBySessionResponse>(responseString,settings);
                return refundResult;
            }
        }

        public static async Task<TransactionQueryByTransIdResponse> TransactionQueryByTransIdAsync(string transId)
        {
            if (string.IsNullOrEmpty(transId))
                throw new ArgumentException("TransactionId mustn't be empty", nameof(transId));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("tran_id", transId);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_PASS);
                parameters.Add("format", "json");

                StringBuilder sb = new StringBuilder();
                sb.Append(requestUrl + "?");
                foreach (KeyValuePair<string, string> item in parameters)
                {
                    sb.Append(item.Key + "=" + item.Value + "&");
                }
                requestUrl = sb.ToString().TrimEnd('&');
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                TransactionQueryByTransIdResponse refundResult = JsonConvert.DeserializeObject<TransactionQueryByTransIdResponse>(responseString,settings);
                return refundResult;
            }
        }

        public static async Task<RefundQueryResponse> RefundQueryAsync(string refundRefId)
        {
            if (string.IsNullOrEmpty(refundRefId))
                throw new ArgumentException("RefundRefId mustn't be empty", nameof(refundRefId));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("refund_ref_id", refundRefId);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_PASS);
                parameters.Add("format", "json");

                StringBuilder sb = new StringBuilder();
                sb.Append(requestUrl + "?");
                foreach (KeyValuePair<string, string> item in parameters)
                {
                    sb.Append(item.Key + "=" + item.Value + "&");
                }
                requestUrl = sb.ToString().TrimEnd('&');

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                RefundQueryResponse refundResult = JsonConvert.DeserializeObject<RefundQueryResponse>(responseString,settings);
                return refundResult;
            }
        }
    }
}
