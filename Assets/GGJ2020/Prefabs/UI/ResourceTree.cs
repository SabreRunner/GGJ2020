using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GGJ2020.Events;

namespace GGJ2020
{
    public class ResourceTree : BaseMonoBehaviour
    {
        public ContinentBlockController.ResourceType TheType;

        private Image image;

        public void UpdateSelected(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            this.Temp("UpdateSelected", "Tree");
            image = GetComponent<Image>();
            this.Temp("UpdateSelected", "Tree alpha " + image.color.a);
            this.Temp("UpdateSelected", "Tree diff " + (resourceDrop.resource == TheType));
            image.color = new Color(1, 1, 1, (resourceDrop.resource == TheType) ? 1 : 0.5f);

        }
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            this.Temp("UpdateSelected", "Tree alpha " + image.color.a);
            this.Temp("UpdateSelected", "Tree diff " + (ContinentBlockController.ResourceType.None == TheType));
            image.color = new Color(1, 1, 1, (ContinentBlockController.ResourceType.None == TheType) ? 1 : 0.5f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
