using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using Video.Youtube;

namespace UI.Video
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerComponent : UIComponent<YoutubeViewModel>
    {
        private VideoPlayer videoPlayer;

        private void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Event("close");
            }
        }

        public override void Upsert(YoutubeViewModel data)
        {
            var format = data.formats.Where(x => x.container == "mp4").OrderByDescending(x => x.quality).FirstOrDefault();

            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = format.url;

            videoPlayer.Play();
        }
    }
}
