using Network.LobbyServer;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Video.Youtube;

namespace UI.Video
{
	[RequireComponent(typeof(Image))]
	public class SlotComponent : UIComponent<YoutubeViewModel>
	{
		private Image video_image;
		private readonly IYoutubeClient client = new YoutubeClient();

		private void Awake()
		{
			video_image = GetComponent<Image>();
		}

		public async Task Upsert(int index, VideoViewModel data)
		{
			var video = await Setup(data.Uri);
			base.Upsert(index, video);

			var thumbnail = video.thumbnails.FirstOrDefault();
			StartCoroutine(DrawImage(thumbnail.url));
		}

		private async Task<YoutubeViewModel> Setup(string videoUrl)
		{
			var videoId = videoUrl.Url2Id();
			return await client.GetInfo(videoId);
		}

		private IEnumerator DrawImage(string url)
		{
			var www = UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();

			var texture = DownloadHandlerTexture.GetContent(www);
			video_image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
		}
	}
}