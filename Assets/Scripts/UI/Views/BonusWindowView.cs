using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Views
{
    public class BonusWindowView : WindowViewBase, IWindowView
    {
        [SerializeField] private TextMeshProUGUI _jumpHeightCount;
        [SerializeField] private TextMeshProUGUI _jumpToTopCount;
        [SerializeField] private Button _jumpHeightBonus;
        [SerializeField] private Button _jumpToTopBonus;

        private float _downPunchScale = 0.6f;
        private float _highlightScale = 0.75f;
        private float _highlightPeriod = 0.06f;
        private int _highlightLoops = 2;
        private Sequence _highlightToTopSeq;
        private Sequence _highlightJumpHeightSeq;

        
        public void Init(Action onJumpCountButton, Action onJumpToTopBonus)
        {
            _jumpHeightBonus.onClick.AddListener(() => { onJumpCountButton?.Invoke();});
            _jumpToTopBonus.onClick.AddListener(() => { onJumpToTopBonus?.Invoke();});
        }

        public void SetJumpHeightBonusCount(int count)
        {
            if (IsOpen)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpHeightCount.transform.DOScale(_downPunchScale, 0.1f).OnComplete(() =>
                {
                    _jumpHeightCount.text = $"{count}";
                })).Append(_jumpHeightCount.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpHeightCount.text = $"X{count}";
            }
        }
        
        public void SetJumpToTopBonusCount(int count)
        {
            if (IsOpen)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_jumpToTopCount.transform.DOScale(_downPunchScale, 0.1f).OnComplete(() =>
                {
                    _jumpToTopCount.text = $"{count}";
                })).Append(_jumpToTopCount.transform.DOScale(1, 0.1f));
            }
            else
            {
                _jumpToTopCount.text = $"X{count}";
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
            _highlightToTopSeq.Append(_jumpHeightBonus.transform.DOScale(_highlightScale, _highlightPeriod)).SetEase(Ease.Linear)
                .Append(_jumpHeightBonus.transform.DOScale(1, _highlightPeriod)).SetEase(Ease.Linear)
                .SetLoops(_highlightLoops);
        }

        public void HighlightJumpToTopBonus()
        {
            _highlightJumpHeightSeq?.Kill();
            _highlightJumpHeightSeq = DOTween.Sequence();
            _highlightJumpHeightSeq.Append(_jumpToTopBonus.transform.DOScale(_highlightScale, _highlightPeriod)).SetEase(Ease.Linear)
                .Append(_jumpToTopBonus.transform.DOScale(1, _highlightPeriod)).SetEase(Ease.Linear)
                .SetLoops(_highlightLoops);
        }
        
    }
}