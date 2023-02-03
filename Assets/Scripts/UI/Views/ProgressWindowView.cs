using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public enum ControlButton
    {
        Up,
        Down,
        Right,
        Left
    }

    public class ProgressWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _jumpsCount;
        [SerializeField] private List<Button> _controlButtons;

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

        public void InitControl(Action<ControlButton> onControlButton)
        {
            _controlButtons[0].onClick.AddListener(() => { onControlButton.Invoke(ControlButton.Up);});
            _controlButtons[1].onClick.AddListener(() => { onControlButton.Invoke(ControlButton.Right);});
            _controlButtons[2].onClick.AddListener(() => { onControlButton.Invoke(ControlButton.Down);});
            _controlButtons[3].onClick.AddListener(() => { onControlButton.Invoke(ControlButton.Left);});
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
        
        protected override void StopAnimators()
        {
        }
    }
}