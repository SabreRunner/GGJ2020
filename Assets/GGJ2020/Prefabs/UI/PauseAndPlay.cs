using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2020
{
    public class PauseAndPlay : BaseMonoBehaviour
    {
        private Image image;
        private bool IsPlaying = true;

        public Sprite PauseImage;
        public Sprite PlayImage;

        public CanvasRenderer PausePanel;

        public void Toggle()
        {
            // TODO: Use event listeners instead.
            if (GameManager.Instance != null && !GameManager.Instance.IsPaused)
            {
                GameManager.Instance?.Pause();
                image.sprite = PlayImage;
                PausePanel.SetActive(true);
            } else
            {
                GameManager.Instance?.Resume();
                image.sprite = PauseImage;
                PausePanel.SetActive(false);
            }
        }

        void Start()
        {
            image = GetComponent<Image>();
            image.sprite = (GameManager.Instance != null && !GameManager.Instance.IsPaused) ? PauseImage : PlayImage;
        }
    }
}
