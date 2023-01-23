using System;
using System.Collections;
using Game.Sound;
using Services.MonoHelpers;
using UnityEngine;
using Zenject;

namespace Services.Time.Impl
{
    public class SlowMotionService : ISlowMotionService
    {
        private const float c_defaultChangeDuration = 0.5f;
        private const float c_normalScale = 1f;
        private const float c_slowScale = 0.2f;
        
        [Inject] private ICoroutineService _coroutineService;
        [Inject] private ISoundManager _soundManager;
        
        private Coroutine _scaleChanging;
        private float _currentScale = 1f;
        private float _fixedDeltaMultiplier = 1;
        
        
        public float CurrentScale => _currentScale;
        public float CurrentFixedDeltaMultiplier => _fixedDeltaMultiplier;
        
        public void SetNormalScale(Action onEnd)
        {
            _coroutineService.StopCor(_scaleChanging);
            _coroutineService.StartCor(ScaleChange(c_normalScale, c_defaultChangeDuration, onEnd));
            UnityEngine.Time.fixedDeltaTime = 1f / 50f;
            _soundManager.ApplyTimeScale(c_normalScale);
        }

        public void SetSlowScale(Action onEnd)
        {
            _coroutineService.StopCor(_scaleChanging);
            _coroutineService.StartCor(ScaleChange(c_slowScale, c_defaultChangeDuration, onEnd));
            _fixedDeltaMultiplier = 0.5f;
            UnityEngine.Time.fixedDeltaTime = 1f / 50f * _fixedDeltaMultiplier;
            _soundManager.ApplyTimeScale(c_slowScale);
        }

        public void SetScale(float scale, float time, Action onEnd)
        {
            _coroutineService.StopCor(_scaleChanging);
            _coroutineService.StartCor(ScaleChange(scale, c_defaultChangeDuration, onEnd));
            _fixedDeltaMultiplier = scale;
            UnityEngine.Time.fixedDeltaTime = 1f / 50f * scale;
            _soundManager.ApplyTimeScale(scale);
        }

        public void Refresh()
        {
            _currentScale = 1f;
            _fixedDeltaMultiplier = 1;
            UnityEngine.Time.timeScale = _currentScale;
            UnityEngine.Time.fixedDeltaTime = 1f / 50f * _fixedDeltaMultiplier;
        }
        

        private IEnumerator ScaleChange(float endScale, float duration, Action onEnd)
        {
            var elapsed = 0f;
            var startScale = _currentScale;
            while (elapsed < duration)
            {
                _currentScale = Mathf.Lerp(startScale, endScale, elapsed / duration);

                UnityEngine.Time.timeScale = _currentScale;
                elapsed += UnityEngine.Time.unscaledDeltaTime;
                yield return null;
            }
            _currentScale = endScale;
            UnityEngine.Time.timeScale = _currentScale;
            onEnd?.Invoke();
        }


    
    }
}