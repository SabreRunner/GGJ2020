using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GGJ2020.Events;

namespace GGJ2020
{
    public class BaseResource : BaseMonoBehaviour
    {
        public ContinentBlockController.ResourceType TheType;

        private Image image;

        public void UpdateSelected(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            image = GetComponent<Image>();
            image.color = new Color(1, 1, 1, (resourceDrop.resource == TheType) ? 1 : 0.5f);

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
