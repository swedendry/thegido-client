using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Network
{
    public class PayloadHttp : Http
    {
        public Payloader<T> Send<T>(HttpMethod method, string uri, object data = null)
        {
            var payloader = new Payloader<T>();

            Send(method, uri, data, (output) =>
            {
                OnPayload(payloader, output.response);
            });

            return payloader;
        }

        public Payloader<T> Get<T>(string uri)
        {
            return Send<T>(HttpMethod.Get, uri);
        }

        public Payloader<T> Post<T>(string uri, object data)
        {
            return Send<T>(HttpMethod.Post, uri, data);
        }

        public Payloader<T> Put<T>(string uri, object data)
        {
            return Send<T>(HttpMethod.Put, uri, data);
        }

        public Payloader<T> Delete<T>(string uri)
        {
            return Send<T>(HttpMethod.Delete, uri);
        }

        private void OnPayload<T>(Payloader<T> payloader, HttpResponseMessage response)
        {
            try
            {
                if (response == null)
                {
                    payloader.OnError("OnPayload HTTPResponse null");
                    return;
                }

                var result = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeObject<Payload<T>>(result);
                    if (obj.data != null)
                    {   //성공
                        payloader.OnSuccess(obj.data);
                    }
                    else
                    {   //실패
                        payloader.OnFail(obj.code);
                    }

                    payloader.OnComplete(obj.data);
                }
                else
                {   //에러
                    payloader.OnError(result);
                }
            }
            catch (Exception ex)
            {
                payloader.OnError(ex.Message);
            }
        }
    }
}
