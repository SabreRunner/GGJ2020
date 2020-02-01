namespace GGJ2020.UI
{
    using UnityEngine;

    public class UIManager : BaseMonoBehaviour
    {
        public Texture2D HandImage;

        public Texture2D GrabbingImage;

        void Start()
        {
            SetCursor(false);
        }

        public void SetCursor(double grabbingProgress)
        {
            // this.Temp("SetCursor", "is grabbing " + grabbingProgress);
            Cursor.SetCursor(grabbingProgress > 0.03 ? GrabbingImage : HandImage, Vector2.zero, CursorMode.Auto);
        }
        public void SetCursor(bool isGrabbing)
        {
            // this.Temp("SetCursor", "is grabbing " + grabbingProgress);
            Cursor.SetCursor(isGrabbing ? GrabbingImage : HandImage, Vector2.zero, CursorMode.Auto);
        }
    }
}
