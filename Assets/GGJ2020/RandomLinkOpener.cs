
namespace GGJ2020
{
    using System;
    using PushForward.ExtensionMethods;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    public class RandomLinkOpener : BaseMonoBehaviour
    {
        [Serializable]
        public class NameAndLink
        {
            public string name;
            public string link;
        }

        [SerializeField] private Text display;
        [SerializeField] private NameAndLink[] namesAndLinks;
        private int currentIndex;

        public void RotateLink()
        {
            this.currentIndex = this.currentIndex.CircleAdd(1, 0, this.namesAndLinks.Length - 1);
            this.display.text = this.namesAndLinks[this.currentIndex].name;

            this.ActionInSeconds(this.RotateLink, 4f);
        }

        public void OpenRandomLink()
        {
            Application.OpenURL(this.namesAndLinks[this.currentIndex].link);
        }

        public void Start()
        {
            this.currentIndex = Random.Range(0, this.namesAndLinks.Length - 1);
            this.RotateLink();
        }
    }
}
