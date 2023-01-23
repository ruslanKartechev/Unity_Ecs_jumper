using System;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class ProgressWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _enemiesCountText;

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

        public void SetEnemiesCount(int count)
        {
            _enemiesCountText.text = $"ENEMIES: {count}";
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