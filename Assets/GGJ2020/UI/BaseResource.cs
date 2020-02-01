namespace GGJ2020.UI
{
    using Events;
    using UnityEngine;
    using UnityEngine.UI;

    public class BaseResource : BaseMonoBehaviour
    {
        public ContinentBlockController.ResourceType Resource;

        private Image image;

        void Start()
        {
            this.image = this.GetComponent<Image>();
        }

        public void UpdateDroppedResource(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            this.image.fillAmount = 0;
            // this.image = this.GetComponent<Image>();
            // this.image.color = new Color(1, 1, 1, (resourceDrop.resource == this.Resource) ? 1 : 0.5f);
        }

        public void UpdateGrabbedResource(double resourcePrecentage)
        {
            if (GameManager.Instance?.grabbedResource == this.Resource)
            {
                this.image.fillAmount = (float)resourcePrecentage;
            } else
            {
                this.image.fillAmount = 0;
            }
        }
    }
}
