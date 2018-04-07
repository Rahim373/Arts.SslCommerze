using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ARTS.SslCommerze
{
    public class SslRequest
    {
        public static async Task<TransactionSession> GetSessionAsync(Trasnaction trasnaction)
        {
            string requestUrl = Configuration.STORE_URL + "/gwprocess/v3/api.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = Helper.GetSessionRequestParameter(trasnaction);
                HttpContent content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                TransactionSession session = JsonConvert.DeserializeObject<TransactionSession>(responseString);
                return session;
            }
        }

        public static TransactionResponse GetTransactionResponse(FormCollection form)
        {
            //Dictionary<string, string> inDictionary = form.AllKeys.ToDictionary(x=> x, v=> form[v]);
            Dictionary<string, object> inDictionary = new Dictionary<string, object>();
            form.CopyTo(inDictionary);
            string inJson = JsonConvert.SerializeObject(inDictionary);
            TransactionResponse response = JsonConvert.DeserializeObject<TransactionResponse>(inJson);
            return response;
        }

        public static ValidatedTransaction ValidateTransaction(FormCollection form)
        {
            ValidatedTransaction transaction = new ValidatedTransaction();

            if (VerifyHash(form))
            {
                string validationId = form["val_id"];

                if (form["status"].Equals("VALID"))
                {
                    transaction = CheckValidationAsync(validationId).Result;
                }
            }

            return transaction;
        }

        private static async Task<ValidatedTransaction> CheckValidationAsync(string validationId)
        {
            string requestUrl = Configuration.STORE_URL + "/validator/api/validationserverAPI.php";
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = Helper.GetTransactionValidationParameter(validationId);
                HttpContent content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                ValidatedTransaction validateResult = JsonConvert.DeserializeObject<ValidatedTransaction>(responseString);
                return validateResult;
            }
        }

        private static bool VerifyHash(FormCollection form)
        {
            bool isValid = false;

            try
            {

                string[] predefinedKeys = form["verify_key"].Split(',');
                Dictionary<string, string> newData = new Dictionary<string, string>();

                foreach (string key in predefinedKeys)
                {
                    if (form[key] != null)
                        newData.Add(key, form[key]);
                }

                newData.Add("store_passwd", CalculateMD5Hash(Configuration.STORE_PASS));
                newData = newData.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
                List<string> keyData = new List<string>();

                foreach (KeyValuePair<string, string> item in newData)
                    keyData.Add(item.Key + "=" + item.Value);

                string mergedString = string.Join("&", keyData);

                string hashedData = CalculateMD5Hash(mergedString);

                isValid = hashedData.Equals(form["verify_sign"]);
            }
            catch
            {
                throw;
            }

            return isValid;
        }

        private static string CalculateMD5Hash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = MD5.Create().ComputeHash(inputBytes);
            string hashedString = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return hashedString;
        }

        public async Task<TransactionQueryBySessionResponse> TransactionQueryBySessionIdAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentException("SessionId mustn't be empty", nameof(sessionId));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("sessionkey", sessionId);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_ID);
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
                TransactionQueryBySessionResponse refundResult = JsonConvert.DeserializeObject<TransactionQueryBySessionResponse>(responseString);
                return refundResult;
            }
        }

        public async Task<TransactionQueryByTransIdResponse> TransactionQueryByTransIdAsync(string transId)
        {
            if (string.IsNullOrEmpty(transId))
                throw new ArgumentException("TransactionId mustn't be empty", nameof(transId));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("tran_id", transId);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_ID);
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
                TransactionQueryByTransIdResponse refundResult = JsonConvert.DeserializeObject<TransactionQueryByTransIdResponse>(responseString);
                return refundResult;
            }
        }

        public static async Task<RefundResponse> RefundAsync(string bankTransactionID, decimal refundAmount, string refundRemarks, string referenceId = "")
        {
            if (string.IsNullOrEmpty(bankTransactionID))
                throw new ArgumentException("BankTransactionID mustn't be empty", nameof(bankTransactionID));
            if (refundAmount <= 0)
                throw new ArgumentException("refundAmount must be greater than 0", nameof(refundAmount));
            if (string.IsNullOrEmpty(refundRemarks))
                throw new ArgumentException("RefundRemarks mustn't be empty", nameof(refundRemarks));

            string requestUrl = Configuration.STORE_URL + "/validator/api/merchantTransIDvalidationAPI.php";

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("bank_tran_id", bankTransactionID);
                parameters.Add("store_id", Configuration.STORE_ID);
                parameters.Add("store_passwd", Configuration.STORE_PASS);
                parameters.Add("refund_amount", refundAmount.ToString());
                parameters.Add("refund_remarks", refundRemarks);
                if (!string.IsNullOrEmpty(referenceId))
                    parameters.Add("refe_id", referenceId);
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
                RefundResponse refundResult = JsonConvert.DeserializeObject<RefundResponse>(responseString);
                return refundResult;
            }
        }
    }
}
