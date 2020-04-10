using System.Collections.Generic;

namespace Video.Youtube
{
    public class YoutubeViewModel
    {
        public List<YoutubeFormat> formats { get; set; } = new List<YoutubeFormat>();
        public List<YoutubeThumbnail> thumbnails { get; set; } = new List<YoutubeThumbnail>();
    }

    public class YoutubeFormat
    {
        public int itag { get; set; }
        public string url { get; set; }
        public string container { get; set; }
        public string quality { get; set; }
    }

    public class YoutubeThumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
