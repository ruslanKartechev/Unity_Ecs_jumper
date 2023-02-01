using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class ProgressWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _jumpsCount;

        private Action _onShown;
        private Action _onClosed;
        
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

        public void UpdateJumpsCount(int count)
        {
            if (IsOpen)
            {
                var downScale = 0.8f;
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpsCount.transform.DOScale(downScale, 0.1f).OnComplete(() =>
                {
                    _jumpsCount.text = $"{count}";
                })).Append(_jumpsCount.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpsCount.text = $"{count}";
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
        
        protected override void StopAnimators()
        {
        }
    }
}