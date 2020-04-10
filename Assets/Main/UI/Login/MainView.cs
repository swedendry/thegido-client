using Network;
using Network.LobbyServer;
using UnityEngine;

namespace UI.Login
{
    public class MainView : View
    {
        private LoginComponet loginComponet;

        protected override void Awake()
        {
            base.Awake();

            loginComponet = GetUIComponent<LoginComponet>();
        }

        private void OnEnable()
        {
            loginComponet?.Upsert();
        }

        protected override void Event(string param)
        {
            switch (param)
            {
                case "login":
                    {
                        Login();
                    }
                    break;
            }
        }

        private void Login()
        {
            var id = SystemInfo.deviceUniqueIdentifier;
            var name = SystemInfo.deviceName;

            LobbyServer.sInstance.GetUser(id).Callback(
            success: (data) =>
            {
                Router.CloseAndOpen("LobbyView", "LobbyView/VideoView");
            },
            fail: (code) =>
            {
                switch (code)
                {
                    case PayloadCode.DbNull:
                        {
                            Register(id, name);
                        }
                        break;
                }
            });
        }

        private void Register(string id, string name)
        {
            LobbyServer.sInstance.CreateUser(new CreateUserBody() { Id = id, Name = name }).Callback(
            success: (data) =>
            {
                Router.CloseAndOpen("LobbyView", "LobbyView/VideoView");
            });
        }
    }
}