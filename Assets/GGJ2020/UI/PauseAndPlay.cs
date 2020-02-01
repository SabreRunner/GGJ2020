namespace GGJ2020.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PauseAndPlay : BaseMonoBehaviour
    {
        private Image image;
        // private bool IsPlaying = true;

        public Sprite PauseImage;
        public Sprite PlayImage;

        public CanvasRenderer PausePanel;

        public void Toggle()
        {
            // TODO: Use event listeners instead.
            if (GameManager.Instance != null && !GameManager.Instance.IsPaused)
            {
                GameManager.Instance?.Pause();
                this.image.sprite = this.PlayImage;
                this.PausePanel.SetActive(true);
            } else
            {
                GameManager.Instance?.Resume();
                this.image.sprite = this.PauseImage;
                this.PausePanel.SetActive(false);
            }
        }

        void Start()
        {
            this.image = this.GetComponent<Image>();
            this.image.sprite = (GameManager.Instance != null && !GameManager.Instance.IsPaused) ? this.PauseImage : this.PlayImage;
        }
    }
}
