using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;

namespace rmxwebapi.Providers
{
    public class TokenReceiver
    {
        private string _TOKEN_SERVICE_URL;
        private MemoryCache _tokenCache = MemoryCache.Default;
        private CacheItemPolicy _tokenCachePolicy;

        public TokenReceiver()
        {
            _tokenCachePolicy = new CacheItemPolicy();
            _tokenCachePolicy.AbsoluteExpiration = DateTime.Now.AddDays(7);
            _TOKEN_SERVICE_URL = "http://localhost:2124/api/";
        }

        public string GetRMXToken()
        {
            if(_tokenCache.Get("TokenCache") == null)
            {
                var response = GetResponseFromTokenService("values");
                if (response == null)
                    return null;

                var token = response.Content.ReadAsAsync<string>().Result;
                if (string.IsNullOrEmpty(token))
                    return null;

                _tokenCache.Add("TokenCache", token, _tokenCachePolicy);

                return token;
            }
            var cacheToken = _tokenCache.Get("TokenCache").ToString();
            return cacheToken;
        }

        private HttpResponseMessage GetResponseFromTokenService(string method)
        {
            try
            {
                var client = new HttpClient() { BaseAddress = new Uri(_TOKEN_SERVICE_URL) };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.GetAsync(method).Result;
                if (message.IsSuccessStatusCode)
                    return message;
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}