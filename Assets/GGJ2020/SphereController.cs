using UnityEngine;

namespace GGJ2020
{
    using UnityEngine.EventSystems;

    public class SphereController : BaseMonoBehaviour
    {
        [SerializeField] private GameConfiguration gameConfiguration;

        private Vector3 previousDragPosition;
        private bool dragging;

        private void CheckDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.previousDragPosition = Input.mousePosition;
                this.dragging = true;
            }

            if (this.dragging)
            {
                this.transform.Rotate(0, -(Input.mousePosition - this.previousDragPosition).x, 0);
                this.previousDragPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            { this.dragging = false; }
        }

        private void Update()
        {
            this.CheckDrag();
        }
    }
}
