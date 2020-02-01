namespace GGJ2020.UI
{
    using Events;
    using UnityEngine;
    using UnityEngine.UI;

    public class BaseResource : BaseMonoBehaviour
    {
        public ContinentBlockController.ResourceType TheType;

        private Image image;

        public void UpdateSelected(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            this.image = this.GetComponent<Image>();
            this.image.color = new Color(1, 1, 1, (resourceDrop.resource == this.TheType) ? 1 : 0.5f);
        }
    }
}
