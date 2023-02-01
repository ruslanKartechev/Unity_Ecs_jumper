using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VFX.Animations.Impl;

namespace UI.Views
{
    public class FailWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private Button _replauButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Animator _animator;
        [SerializeField] private PulsingAnimator _pulsingAnimator;

        private Action _onShown;
        private Action _onClosed;
        private Action _replayAction;

        public string HeaderText
        {
            get => _levelText.text;
            set => _levelText.text = value;
        }
        

        public void InitButtons(Action onReplay)
        {
            _replayAction = onReplay;
            _replauButton.onClick.AddListener(() => { _replayAction?.Invoke();});
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