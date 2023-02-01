using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using VFX.Animations.Impl;

namespace UI.Views
{
    public class WinWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Animator _animator;
        [SerializeField] private PulsingAnimator _pulsingAnimator;

        private Action _onShown;
        private Action _onClosed;
        
        public string HeaderText
        {
            get => _levelText.text;
            set => _levelText.text = value;
        }

        private Action _nextLevelAction;
        
        public void InitButtons(Action nextLevel)
        {
            _nextLevelAction = nextLevel;
            _nextButton.onClick.AddListener(() =>
            {
                _nextLevelAction?.Invoke();
            });
        }
        
        public void ShowView(Action onShown)
        {
            // _animator.enabled = true;
            IsOpen = true;
            _onShown = onShown;
            // _animator.Play("FadeIn");

        }

        public void CloseView(Action onClosed)
        {
            // _animator.enabled = true;
            _onClosed = onClosed;
            // _animator.Play("FadeOut");
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