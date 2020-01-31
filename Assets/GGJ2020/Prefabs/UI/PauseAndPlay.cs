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

        public void Toggle()
        {
            this.SetIsPlaying(!IsPlaying);
        }

        // Update is called once per frame
        void Start()
        {
            image = GetComponent<Image>();
            this.SetIsPlaying(IsPlaying);
        }
        void SetIsPlaying(bool isPlaying)
        {
            IsPlaying = isPlaying;
            image.sprite = IsPlaying ? PauseImage : PlayImage;
            Time.timeScale = IsPlaying ? 1 : 0;
        }
    }
}
