using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Video.Youtube
{
    public class YoutubeClient : IYoutubeClient
    {
        private readonly HttpClient httpClient;

        public YoutubeClient()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = false;

            httpClient = new HttpClient(handler, true);
        }

        public async Task<YoutubeViewModel> GetInfo(string videoId, string el = "embedded")
        {
            var info = new YoutubeViewModel();

            var responseJson = await GetJToken(videoId, el);

            foreach (var json in responseJson.SelectToken("videoDetails.thumbnail.thumbnails"))
            {
                var url = json.SelectToken("url").Value<string>();
                var width = json.SelectToken("width").Value<int>();
                var height = json.SelectToken("height").Value<int>();

                info.thumbnails.Add(new YoutubeThumbnail()
                {
                    url = url,
                    width = width,
                    height = height
                });
            }

            foreach (var json in responseJson.SelectToken("streamingData.formats"))
            {
                var itag = json.SelectToken("itag").Value<int>();
                var url = json.SelectToken("url").Value<string>();
                var mime = json.SelectToken("mimeType").Value<string>();
                var contailer = mime.SubstringUntil(";").SubstringAfter("/");
                var quality = json.SelectToken("qualityLabel").Value<string>();

                info.formats.Add(new YoutubeFormat()
                {
                    itag = itag,
                    url = url,
                    container = contailer,
                    quality = quality,
                });
            }

            return info;
        }

        private async Task<JToken> GetJToken(string videoId, string el = "embedded")
        {
            var query = await GetQuery(videoId, el);
            var splitquery = SplitQuery(query);
            var response = splitquery["player_response"];
            var responseJson = JToken.Parse(response);

            var playabilityStatus = responseJson.SelectToken("playabilityStatus.status")?.Value<string>();
            if (!string.Equals(playabilityStatus, "OK", StringComparison.OrdinalIgnoreCase))
            {
                var watchQuery = await GetQuery(videoId).ConfigureAwait(false);
                var configRaw = Regex.Match(watchQuery, @"ytplayer\.config = (?<Json>\{[^\{\}]*(((?<Open>\{)[^\{\}]*)+((?<Close-Open>\})[^\{\}]*)+)*(?(Open)(?!))\})")
                .Groups["Json"].Value;
                var watchJson = JToken.Parse(configRaw);
                var watchResponse = watchJson.SelectToken("args.player_response").Value<string>();
                var watchResponseJson = JToken.Parse(watchResponse);

                responseJson = watchResponseJson;
            }

            return responseJson;
        }

        private async Task<string> GetQuery(string videoId, string el = "embedded")
        {
            var eurl = WebUtility.UrlEncode($"https://youtube.googleapis.com/v/{videoId}");
            var url = $"https://www.youtube.com/get_video_info?video_id={videoId}&el={el}&eurl={eurl}&hl=en";
            return await httpClient.GetStringAsync(url).ConfigureAwait(false);
        }

        private async Task<string> GetQuery(string videoId)
        {
            var url = $"https://www.youtube.com/watch?v={videoId}&disable_polymer=true&bpctr=9999999999&hl=en";
            return await httpClient.GetStringAsync(url).ConfigureAwait(false);
        }

        private Dictionary<string, string> SplitQuery(string query)
        {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var paramsEncoded = query.TrimStart('?').Split('&');
            foreach (var paramEncoded in paramsEncoded)
            {
                var param = WebUtility.UrlDecode(paramEncoded);

                var equalsPos = param.IndexOf('=');
                if (equalsPos <= 0)
                    continue;

                var key = param.Substring(0, equalsPos);
                var value = equalsPos < param.Length
                    ? param.Substring(equalsPos + 1)
                    : string.Empty;

                dic[key] = value;
            }

            return dic;
        }
    }
}