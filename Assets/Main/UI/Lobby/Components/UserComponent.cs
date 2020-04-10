using Extension.UI;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class UserComponent : UIComponent
    {
        public Text id_text;
        public Text name_text;

        public override void Upsert()
        {
            base.Upsert();

            id_text.Set(ServerInfo.User.Id);
            name_text.Set(ServerInfo.User.Name);
        }
    }
}