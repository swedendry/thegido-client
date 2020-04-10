using System.Collections.Generic;

namespace Network.LobbyServer
{
    public partial class LobbyServer
    {
        public Payloader<List<VideoViewModel>> GetVideos()
        {
            var url = string.Format("api/videos");

            return http.Get<List<VideoViewModel>>(GetUri(url)).Callback(
                success: (data) =>
                {
                    ServerInfo.Videos = data;
                });
        }
    }
}