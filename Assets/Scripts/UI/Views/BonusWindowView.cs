using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Views
{
    public class BonusWindowView : WindowViewBase, IWindowView
    {

        [System.Serializable]
        public class ButtonData
        {
            public Button Button;
            public TextMeshProUGUI Text;
            public TextMeshProUGUI CountText;
            public Image Icon;
            

        }
        [SerializeField] private ButtonData _jumpHeightBonus;
        [SerializeField] private ButtonData _jumpToTopBonus;

        private float _downPunchScale = 0.6f;
        private float _highlightScale = 0.8f;
        private float _highlightPeriod = 0.05f;
        private int _highlightLoops = 2;
        private Sequence _highlightToTopSeq;
        private Sequence _highlightJumpHeightSeq;

        
        public void Init(Action onJumpCountButton, Action onJumpToTopBonus)
        {
            _jumpHeightBonus.Button.onClick.AddListener(() => { onJumpCountButton?.Invoke();});
            _jumpToTopBonus.Button.onClick.AddListener(() => { onJumpToTopBonus?.Invoke();});
        }

        public void SetJumpHeightBonusCount(int count)
        {
            if (IsOpen)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpHeightBonus.Icon.transform.DOScale(_downPunchScale, 0.1f).OnComplete(() =>
                {
                    _jumpHeightBonus.CountText.text = $"x{count}";
                })).Append(_jumpHeightBonus.Icon.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpHeightBonus.CountText.text = $"x{count}";
            }
        }
        
        public void SetJumpToTopBonusCount(int count)
        {
            if (IsOpen)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpToTopBonus.Icon.transform.DOScale(_downPunchScale, 0.1f).OnComplete(() =>
                {
                    _jumpToTopBonus.CountText.text = $"x{count}";
                })).Append(_jumpToTopBonus.Icon.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpToTopBonus.CountText.text = $"x{count}";
            }
        }
        
        protected override void StopAnimators()
        {
        }   

        public void ShowView(Action onShown)
        {
            IsOpen = true;
        }

        public void CloseView(Action onClosed)
        {
            IsOpen = false;
        }
        
        public void HighlightJumpHeightBonus()
        {
            _highlightToTopSeq?.Kill();
            _highlightToTopSeq = DOTween.Sequence();
            _highlightToTopSeq.Append(_jumpHeightBonus.Icon.transform.DOScale(_highlightScale, _highlightPeriod)).SetEase(Ease.Linear)
                .Append(_jumpHeightBonus.Icon.transform.DOScale(1, _highlightPeriod)).SetEase(Ease.Linear)
                .SetLoops(_highlightLoops);
        }

        public void HighlightJumpToTopBonus()
        {
            _highlightJumpHeightSeq?.Kill();
            _highlightJumpHeightSeq = DOTween.Sequence();
            _highlightJumpHeightSeq.Append(_jumpToTopBonus.Icon.transform.DOScale(_highlightScale, _highlightPeriod)).SetEase(Ease.Linear)
                .Append(_jumpToTopBonus.Icon.transform.DOScale(1, _highlightPeriod)).SetEase(Ease.Linear)
                .SetLoops(_highlightLoops);
        }
        
    }
}