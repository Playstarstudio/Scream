using UnityEngine;

namespace UI
{
    public static class GestureHelper
    {

        public static void CloseGestureUI(GameObject panel)
        {
            // Hide the specific gesture panel directly (in case it wasn't opened via HUDSingleton)
            if (panel != null)
            {
                panel.SetActive(false);
            }

            var hud = HUDSingleton.Instance;
            if (hud != null)
            {
                hud.CloseGestureScreen();
            }
        }
    }
}
