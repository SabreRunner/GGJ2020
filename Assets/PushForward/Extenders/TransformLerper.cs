using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soroka
{
    public class TransformLerper : BaseMonoBehaviour
    {
        [Serializable]
        public class TransformLerp
        {
            public Transform transform;
            public Transform sourceRectTransform, targetRectTransform;
            public float durationInSeconds;
            public AnimationCurve animationCurve;
            public UnityEvent onFinish;
        }

        [SerializeField] private TransformLerp[] transformLerps;
        private int lerpIndex;
        private TransformLerp IndexedLerp => this.transformLerps[this.lerpIndex];

        private void LerpTransformSubroutine(float currentTime)
        {
            float fraction = currentTime / this.IndexedLerp.durationInSeconds;
            float evaluatedFraction = this.IndexedLerp.animationCurve.Evaluate(fraction);

            this.IndexedLerp.transform.localPosition = Vector3.Lerp(this.IndexedLerp.sourceRectTransform.localPosition, this.IndexedLerp.targetRectTransform.localPosition, evaluatedFraction);
            this.IndexedLerp.transform.localRotation = Quaternion.Lerp(this.IndexedLerp.sourceRectTransform.localRotation, this.IndexedLerp.targetRectTransform.localRotation, evaluatedFraction);
            this.IndexedLerp.transform.localScale = Vector3.Lerp(this.IndexedLerp.sourceRectTransform.localScale, this.IndexedLerp.targetRectTransform.localScale, evaluatedFraction);
        }

        public void LerpTransform(int lerpIndex)
        {
            this.lerpIndex = lerpIndex;
            this.ActionEachFrameForSeconds(this.LerpTransformSubroutine, this.IndexedLerp.durationInSeconds);
            this.ActionInSeconds(()=>this.LerpTransformSubroutine(this.IndexedLerp.durationInSeconds),
                                    this.IndexedLerp.durationInSeconds);
            this.ActionInSeconds(this.IndexedLerp.onFinish.Invoke, this.IndexedLerp.durationInSeconds);
        }
    }
}
