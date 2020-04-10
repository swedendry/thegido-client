using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace Network
{
    public class Http
    {
        public class QueueData
        {
            public HttpRequestMessage request { get; set; }
            public HttpResponseMessage response { get; set; }
            public Action<QueueData> callback { get; set; }
        }

        private readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) };
        private readonly Queue<QueueData> queues = new Queue<QueueData>();

        public IEnumerator CheckQueue()
        {
            while (true)
            {
                if (queues.Count > 0)
                {
                    var queue = queues.Dequeue();

                    if (queue != null)
                    {
                        queue.callback(queue);
                        yield break;
                    }
                }

                yield return null;
            }
        }

        public void UpdateQueue()
        {
            while (queues.Count > 0)
            {
                var queue = queues.Dequeue();

                if (queue != null)
                    queue.callback(queue);
            }
        }

        public void Send(HttpMethod method, string uri, object data = null, Action<QueueData> callback = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, uri);

                if (data != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                client.SendAsync(request).ContinueWith((response) =>
                {
                    queues.Enqueue(new QueueData() { request = request, response = response.Result, callback = callback });
                });
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);

                throw;
            }
        }
    }
}