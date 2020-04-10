using UnityEngine;

namespace Network.LobbyServer
{
    public partial class LobbyServer : MonoBehaviour
    {
        public static LobbyServer sInstance;

        public string BaseUri = "http://thegido-lobby.azurewebsites.net/";
        //public string BaseUri = "http://localhost:44314/";

        private readonly PayloadHttp http = new PayloadHttp();

        private void Awake()
        {
            if (sInstance != null)
            {
                if (sInstance != this)
                {
                    Destroy(gameObject);
                }
                return;
            }
            sInstance = this;

            //StartCoroutine(http.CheckQueue());
        }

        private void OnApplicationQuit()
        {
            sInstance = null;
        }

        private void Update()
        {
            http?.UpdateQueue();
        }

        private string GetUri(string relativeUri)
        {
            return string.Concat(BaseUri, relativeUri);
        }
    }
}