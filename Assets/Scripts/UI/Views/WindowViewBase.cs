using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public abstract class WindowViewBase : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected GraphicRaycaster _raycaster;
        protected bool _open = true;

        public bool IsOpen
        {
            get => _open;
            set
            {
                if(value == _open)
                    return;
                _open = value;
                _canvas.enabled = _open;
                _raycaster.enabled = _open;
                if(!value)
                    StopAnimators();
            }
        }

        protected abstract void StopAnimators();
    }
}