using System.Threading.Tasks;

namespace Video.Youtube
{
    public interface IYoutubeClient
    {
        Task<YoutubeViewModel> GetInfo(string videoId, string el = "embedded");
    }
}
