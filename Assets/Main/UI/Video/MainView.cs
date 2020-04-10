using Network.LobbyServer;
using Video.Youtube;

namespace UI.Video
{
    public class MainView : View
    {
        public SlotView slotView;

        private VideoPlayerComponent videoPlayerComponent;

        protected override void Awake()
        {
            base.Awake();

            slotView.OnEventProps = Event;
            videoPlayerComponent = GetUIComponent<VideoPlayerComponent>();
        }

        private void OnEnable()
        {
            GetVideos();
        }

        private void GetVideos()
        {
            LobbyServer.sInstance.GetVideos().Callback(
            success: (data) =>
            {
                Router.Open("LobbyView/VideoView/SlotView");
                slotView.Upsert();
            });
        }

        private void Event(bool isSelected, Props<YoutubeViewModel> props)
        {
            Router.Open("LobbyView/VideoView/PopupView");
            videoPlayerComponent.Upsert(props.data);
        }

        protected override void Event(string param)
        {
            switch (param)
            {
                case "refresh":
                    {
                        GetVideos();
                    }
                    break;
                case "close":
                    {
                        Router.Close("LobbyView/VideoView/PopupView");
                    }
                    break;
            }
        }

        public void Refresh()
        {

        }
    }
}