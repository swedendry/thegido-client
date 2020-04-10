using UnityEngine;

namespace Extension.UI
{
    public static class GameObjectExtension
    {
        public static void Set(this GameObject element, bool value)
        {
            if (element == null)
                return;

            element.SetActive(value);
        }
    }
}