using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.Views
{
    [System.Serializable]
    public class ControlButtonData
    {
        public Button Btn;
        public ControlButton ButtonType;
        public Vector3 Dir;
        public float MoveDist;
        public float MoveTime;
        public Transform Transf;
        
        private Vector3 _startPos;
        private Sequence _sequence;
        
        public void Init()
        {
            Transf = Btn.transform;
            _startPos = Transf.localPosition;
        }
        
        public void Press()
        {
            
            _sequence?.Kill();
            Transf.localPosition = _startPos;
            _sequence = DOTween.Sequence();
            _sequence.Append(Transf.DOLocalMove(_startPos + Dir * MoveDist, MoveTime))
                .Append(Transf.DOLocalMove(_startPos, MoveTime));

        }
    }
}