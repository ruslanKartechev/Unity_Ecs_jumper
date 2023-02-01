using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Helpers;
using UI.Elements;
using UI.Elements.Impl;
using VFX.Animations.Impl;

namespace UI.Views
{
    public class StartWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private SoundButton _soundElement; 
        [SerializeField] private Button _startBtn;
        [SerializeField] private Animator _animator;
        [SerializeField] private PulsingAnimator _pulsingAnimator;

        private Action _startAction;
        private Action _soundAction;
        private Action _onShown;
        private Action _onClosed;
        
        public string HeaderText
        {
            get => _levelText.text;
            set => _levelText.text = value;
        }
        
        public ISoundButton SoundButton => _soundElement;
        
        public void ShowView(Action onShown)
        {
            IsOpen = true;
            _onShown = onShown;
            OnShowAnimEvent();  
            // _animator.enabled = true;
            // _animator.Play("FadeIn");
        }

        public void CloseView(Action onClosed)
        {
            _pulsingAnimator.StopScaling();
            // _animator.enabled = true;
            // _animator.Play("FadeOut");
            _onClosed = onClosed;
            OnCloseAnimEvent();
        }

        public void InitButtons(Action startAction, Action soundAction)
        {
            _startAction = startAction;
            _soundAction = soundAction;
            _startBtn.onClick.AddListener(() => { _startAction?.Invoke();});
            // _soundBtn.onClick.AddListener(() => { _soundAction?.Invoke();});
        }

        public void OnCloseAnimEvent()
        {
            IsOpen = false;
            _onClosed?.Invoke();
            _onClosed = null;
        }
        
        public void OnShowAnimEvent()
        {
            // _animator.enabled = false;
            _pulsingAnimator.StartScaling();
            _onShown?.Invoke();
            _onShown = null;
        }

        protected override void StopAnimators()
        {
            _pulsingAnimator.StopScaling();
        }
    }
}