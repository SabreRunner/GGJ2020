using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2020
{
    public class GameOverScreen : MonoBehaviour
    {
        public Sprite WinImage;
        public Sprite LoseImage;

        private Image screenImage;

        public void SetScreen(bool vicroty)
        {
            this.screenImage.sprite = vicroty ? this.WinImage : this.LoseImage;
        }

        void Start()
        {
            this.screenImage = this.GetComponent<Image>();
        }
    }
}
