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
            IsPlaying = !IsPlaying;
            image.sprite = IsPlaying ? PauseImage : PlayImage;
        }

        // Update is called once per frame
        void Start()
        {
            image = GetComponent<Image>();
            image.sprite = IsPlaying ? PauseImage : PlayImage;
        }
    }
}
