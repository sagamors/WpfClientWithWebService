using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WpfClient.CustomException;

namespace WpfClient
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestClient
    {
        private readonly JsonSerializerSettings JsonSerializerSettings  = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All};
        private const string DEFAULT_CONTENT_TYPE = "application/json";
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }
        
        public RestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = DEFAULT_CONTENT_TYPE;
            PostData = "";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = DEFAULT_CONTENT_TYPE;
            PostData = "";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = DEFAULT_CONTENT_TYPE;
            PostData = "";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = DEFAULT_CONTENT_TYPE;
            PostData = postData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Ответ</returns>
        public T MakeRequest<T>()
        {
            return MakeRequest<T>("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Ответ</returns>
        public string Post(string parameters, object postObject)
        {
            try
            {
                PostData = JsonConvert.SerializeObject(postObject, JsonSerializerSettings);
                Method = HttpVerb.POST;
                return MakeRequest(parameters);
            }
            finally
            {
                PostData = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Ответ</returns>
        public TOut Post<TOut>(string parameters, object postObject)
        {
            try
            {
                PostData = JsonConvert.SerializeObject(postObject, JsonSerializerSettings);
                Method = HttpVerb.POST;
                return JsonConvert.DeserializeObject<TOut>(MakeRequest(parameters), JsonSerializerSettings);
            }
            finally
            {
                PostData = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Ответ</returns>
        public string MakeRequest(string parameters)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(EndPoint + parameters);

                request.Method = Method.ToString();
                request.ContentLength = 0;
                request.ContentType = ContentType;

                if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
                {
                    var encoding = new UTF8Encoding();
                    var bytes = encoding.GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                        throw new RestException(message);
                    }

                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }

                    return responseValue;
                }
            }
            catch (Exception ex)
            {
                throw new RestException(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Ответ</returns>
        public T MakeRequest<T>(string parameters)
        {
             return JsonConvert.DeserializeObject<T>(MakeRequest(parameters), JsonSerializerSettings);
        }
    }
}
