using UnityEngine.UI;

namespace Extension.UI
{
    public static class TextExtension
    {
        public static void Set(this Text element, string text)
        {
            if (element == null)
                return;

            element.text = text;
        }
    }
}