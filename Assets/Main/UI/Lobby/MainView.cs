namespace UI.Lobby
{
    public class MainView : View
    {
        private UserComponent userComponent;

        protected override void Awake()
        {
            base.Awake();

            userComponent = GetUIComponent<UserComponent>();
        }

        private void OnEnable()
        {
            userComponent?.Upsert();
        }
    }
}