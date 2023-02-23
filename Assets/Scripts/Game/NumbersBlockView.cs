using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game
{
    public partial class NumbersBlockView : MonoBehaviour, INumberBlockView
    {
        [SerializeField] private TextMeshPro _reference;
        [SerializeField] private int _count;
        [SerializeField] private int _spacing;
        [SerializeField] private int _startValue;
        [SerializeField] private int _veryFirstValue;
        [SerializeField] private float _threshold;
        [SerializeField] private List<Data> _texts;
        private Sequence _highlightSequence;
        
        private float _scaleDur = 0.22f;
        private float _upScale = 1.15f;
        private int _bounceCount = 2;
        private float _bounceDur = 0.2f;
        private float _bounceScale = 1.05f;

        public void StartTest()
        {
            gameObject.SetActive(true);
            StartCoroutine(Test());
        }
        
#if UNITY_EDITOR
        public void Spawn()
        {
            Clear();
            _reference.gameObject.SetActive(true);
            for (int i = 0; i < _count; i++)
            {
                var value = _startValue + _spacing * i;
                var instance = Instantiate(_reference, transform, true);
                instance.gameObject.SetActive(true);
                if (i == 0)
                    value = _veryFirstValue;
                instance.SetText($"{value:N1}");
                _texts.Add(new Data(instance, value));
                instance.transform.position = _reference.transform.position + Vector3.up * (i * _spacing);
            }
            _reference.gameObject.SetActive(false);
        }

        public void Clear()
        {
            foreach (var instance in _texts)
            {
                DestroyImmediate(instance.Text.gameObject);
            }
            _texts.Clear();
        }
        #endif
        
        
        public void Show(bool animated)
        {
            gameObject.SetActive(true);
            if (animated)
            {
                StartCoroutine(ShowScaling());
            }
            
        }

        public void Hide(bool animated)
        {
            if (animated)
            {
                foreach (var instance in _texts)
                {
                    instance.Text.transform.DOPunchScale(Vector3.zero, 0.25f, 1, 0.1f);
                }   
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Highlight(float height)
        {
            var data = _texts.Find(t =>
            {
                var d = t.Value - height;
                return d >= 0 && d <= _threshold;
            });
            if (data != null)
            {
                _highlightSequence?.Kill();
                _highlightSequence = DOTween.Sequence();
                var tt = data.Text.transform;
                var eulers1 = tt.eulerAngles;
                eulers1.z = 180;
                var eulers2 = eulers1;
                eulers2.z = 360f;
                var duration = 0.2f;
                var endScale = 0.2f;
                _highlightSequence.Append(tt.DORotate(eulers1, duration))
                    .Join(tt.DOScale(Vector3.one * endScale, duration))
                    .Append(tt.DORotate(eulers2, duration))
                    .Join(tt.DOScale(Vector3.one * 1f, duration));
            }
            else
            {
                Debug.Log($"cannot find text for value: {height}");
            }
        }

        private IEnumerator HighlightScaling(Transform target)
        {
            var dur = _bounceDur;
            var startScale = 1f;
            var endScale = _bounceScale;
            for (int i = 0; i < _bounceCount; i++)
            {
                var elapsed = 0f;
                while (elapsed < dur)
                {
                    var scale = Mathf.Lerp(startScale, endScale, elapsed / dur);
                    target.localScale = scale * Vector3.one;
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                elapsed = 0f;
                while (elapsed < dur)
                {
                    var scale = Mathf.Lerp(endScale, startScale, elapsed / dur);
                    target.localScale = scale * Vector3.one;
                    elapsed += Time.deltaTime;
                    yield return null;
                } 
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ShowScaling()
        {
            yield return ScaleAll(0f, _upScale, _scaleDur * 0.8f);
            yield return null;
            yield return ScaleAll(_upScale, 1, _scaleDur * 0.2f);
        }
        
        private IEnumerator ScaleAll(float start, float end, float dur)
        {
            var elapsed = 0f;
            while (elapsed < dur)
            {
                var scale = Mathf.Lerp(start, end, elapsed / dur);
                foreach (var text in _texts)
                {
                    text.Text.transform.localScale = Vector3.one * scale;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
            foreach (var text in _texts)
            {
                text.Text.transform.localScale = Vector3.one * end;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator Test()
        {
            yield return new WaitForSeconds(0.2f);
            Show(true);
            yield return new WaitForSeconds(2f);
            Highlight(5);
            yield return new WaitForSeconds(2f);
            Highlight(10);

        }
    }
}