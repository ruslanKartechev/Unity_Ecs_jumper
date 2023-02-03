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
        [SerializeField] private PulsingAnimator _pulsingAnimator;

        private Action _nextLevelAction;

        private Action _onShown;
        private Action _onClosed;
        
        public string HeaderText
        {
            get => _levelText.text;
            set => _levelText.text = value;
        }

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
            IsOpen = true;
            _onShown = onShown;
        }

        public void CloseView(Action onClosed)
        {
            _onClosed = onClosed;
        }
        
        public void OnCloseAnimEvent()
        {
            IsOpen = false;
            _onClosed?.Invoke();
            _onClosed = null;
        }
        
        public void OnShowAnimEvent()
        {
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