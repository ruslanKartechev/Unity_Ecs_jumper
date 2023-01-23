using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements.Impl
{
    public class SoundButton : MonoBehaviour, ISoundButton
    {
        [SerializeField] private Image _on;
        [SerializeField] private Image _off;

        private bool _state;

        public bool State
        {
            get => _state;
            set
            {
                _state = value;
                _on.enabled = value;
                _off.enabled = !value;
                Debug.Log($"SOUND BTN State set to: {value}");
            }
        }
    }
}

