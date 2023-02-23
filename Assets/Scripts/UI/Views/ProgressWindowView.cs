using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ProgressWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _jumpsCount;
        [SerializeField] private List<ControlButtonData> _controlButtons;
        [SerializeField] private TextMeshProUGUI _tierText;
        [SerializeField] private Image _highlightImage;
        [SerializeField] private float _moveTime, _moveDist;
        
        private Action _onShown;
        private Action _onClosed;
        private Sequence _highlightSequence;
        
        public void ShowView(Action onShown)
        {
            IsOpen = true;
            onShown?.Invoke();
        }

        public void CloseView(Action onClosed)
        {
            IsOpen = false;
            onClosed?.Invoke();
        }

        public void InitControl(Action<ControlButton> onControlButton)
        {
            foreach (var button in _controlButtons)
            {
                button.Init();
                button.MoveDist = _moveDist;
                button.MoveTime = _moveTime;
                button.Btn.onClick.AddListener(() =>
                {
                    onControlButton.Invoke(button.ButtonType);
                    button.Press();
                });
            }
        }
        

        public void UpdatePlayerHeight(float value)
        {
            if (IsOpen)
            {
                var downScale = 0.9f;
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpsCount.transform.DOScale(downScale, 0.1f).OnComplete(() =>
                {
                    _jumpsCount.text = $"{value:N1}m";
                })).Append(_jumpsCount.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpsCount.text = $"{value:N1}m";
            }
        }
        
        public void OnCloseAnimEvent()
        {
            _onClosed?.Invoke();
            _onClosed = null;
        }
        
        public void OnShowAnimEvent()
        {
            _onShown?.Invoke();
            _onShown = null;
        }

        public void UpdateTier(string text)
        { 
            _tierText.text = text;
            if(_open)
            {
                _highlightSequence.Kill();
                _highlightSequence = DOTween.Sequence();
                _highlightSequence.Append(_highlightImage.DOFade(1f, 0.2f))
                    .Append(_highlightImage.DOFade(0f, 0.35f));
            }
        }
        
        protected override void StopAnimators()
        {
        }
    }
}