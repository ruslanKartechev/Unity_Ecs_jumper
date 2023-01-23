using System.Collections;
using UnityEngine;

namespace VFX.Animations.Impl
{
    public class PulsingAnimator : MonoBehaviour
    {
        public bool AutoStart = true;
        public Transform Target;
        
        [SerializeField] private Vector3 _scaleUp;
        [SerializeField] private Vector3 _normalScale;
        [SerializeField] private Vector3 _scaleDown;
        [SerializeField] private float _scaleUpTime;
        [SerializeField] private float _scaleDownTime;
        [SerializeField] private AnimationCurve _curveUp;
        [SerializeField] private AnimationCurve _curveDown;

        
        private Coroutine _scaling;
        
        private void OnEnable()
        {
            if (AutoStart )
            {
                StartScaling();
            }
        }

        private void OnDisable()
        {
            StopScaling();
        }

        public void StartScaling()
        {
            if (Target == null)
                Target = transform;
            Target.localScale = _normalScale;
            StopScaling();
            _scaling = StartCoroutine(ScalingAnimation());
        }

        public void StopScaling()
        {
            if (_scaling != null)
                StopCoroutine(_scaling);
        }

        private IEnumerator ScalingAnimation()
        {
            while (true)
            {
                yield return Stage_1();
                yield return Stage_2();
                yield return Stage_3();
                yield return Stage_4();

            }    
        }

        private IEnumerator Stage_1()
        {
            var time = _scaleUpTime;
            var start = Target.localScale;
            var end = _scaleUp;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                Target.localScale = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime * _curveUp.Evaluate(elapsed / time * 0.5f + 0.5f);
                yield return null;
            }
            Target.localScale = end;
        }

        private IEnumerator Stage_2()
        {
            var time = _scaleDownTime;
            var start = Target.localScale;
            var end = _normalScale;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                Target.localScale = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime * _curveDown.Evaluate(elapsed / time * 0.5f);
                yield return null;
            }
            Target.localScale = end;
        }
        
        private IEnumerator Stage_3()
        {
            var time = _scaleDownTime;
            var start = Target.localScale;
            var end = _scaleDown;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                Target.localScale = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime * _curveDown.Evaluate(elapsed / time * 0.5f + 0.5f);
                yield return null;
            }
            Target.localScale = end;
        }
        
        private IEnumerator Stage_4()
        {
            var time = _scaleUpTime;
            var start = Target.localScale;
            var end = _normalScale;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                Target.localScale = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime * _curveUp.Evaluate(elapsed / time * 0.5f);
                yield return null;
            }
            Target.localScale = end;
        }
    }
}