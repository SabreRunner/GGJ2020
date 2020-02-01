
namespace PushForward.Extenders
{
    using Base;
    using ExtensionMethods;
    using UnityEngine;
    using UnityEngine.Events;

    public class GenericLerper : BaseMonoBehaviour
    {
        [SerializeField] private float source;
        [SerializeField] private float timeInSeconds;
        [SerializeField] private FloatEvent lerpEvent;
        [SerializeField] private UnityEvent triggerAtLerpEnd;

        public float Source { get => this.source; set => this.source = value; }

        public float TimeInSeconds { get => this.timeInSeconds; set => this.timeInSeconds = value; }

        public void LerpTo(float destination)
        {
            this.ActionEachFrameForSeconds(
                secondsPassed => this.lerpEvent?.Invoke(
                    this.Source + (secondsPassed / this.TimeInSeconds).Clamp01() * (destination - this.Source)),
                this.TimeInSeconds);
            this.ActionInSeconds(()=>this.triggerAtLerpEnd?.Invoke(), this.TimeInSeconds);
        }
    }
}
