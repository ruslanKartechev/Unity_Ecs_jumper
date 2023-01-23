using System.Collections;
using System.Collections.Generic;
using Services.MonoHelpers;
using UnityEngine;
using Zenject;

namespace Services.Input.Impl
{
    public class InputService : IInputService
    {
        [Inject] private ICoroutineService _coroutineService;
        private List<ITouchListener> _listeners = new List<ITouchListener>();
        private Coroutine _checking;
        private Vector2 _oldPos;
        private Vector2 _newPos;
        private Vector2 _delta;
        private bool _enabled;

        public bool IsEnabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
                if (_enabled)
                {
                    _checking = _coroutineService.StartCor(Checking());
                }
                else
                {
                    _coroutineService.StopCor(_checking);
                }
            }
        }

        public Vector2 MousePosition
        {
            get => UnityEngine.Input.mousePosition;
        }

        public Vector2 Delta => _delta;
        

        public void AddTouchListener(ITouchListener listener)
        {
            if (_listeners == null)
                _listeners = new List<ITouchListener>();
            if(_listeners.Contains(listener) == false)
                _listeners.Add(listener);   
        }

        public void RemoveTouchListener(ITouchListener listener)
        {
            _listeners.Remove(listener);   
        }

        private IEnumerator Checking()
        {
            _newPos = UnityEngine.Input.mousePosition;
            _oldPos = _newPos;
            while (true)
            {
                _delta = Vector2.zero;
                _newPos = UnityEngine.Input.mousePosition;

                
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    _oldPos = _newPos;
                    foreach (var listener in _listeners)
                    {
                        listener.Touch(UnityEngine.Input.mousePosition, 0);
                    }
                } 
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    foreach (var listener in _listeners)
                    {
                        listener.Release(0);
                    }   
                } 
                
                if (UnityEngine.Input.GetMouseButton(0))
                {
                    _delta = _newPos - _oldPos;
                    foreach (var listener in _listeners)
                    {
                        listener.Hold(  UnityEngine.Input.mousePosition, 0);
                    }
                }
                
                _oldPos = _newPos;
                yield return null;
            }
        }
        
        
    }
}