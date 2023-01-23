using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DebugOutput : MonoBehaviour
    {
        [SerializeField] private bool _active;
        [SerializeField] private TextMeshProUGUI _speed;
        [SerializeField] private TextMeshProUGUI _torque;
        [SerializeField] private TextMeshProUGUI _break;
        [SerializeField] private TextMeshProUGUI _maxSpeed;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _enemyColor;
        
        
        private static DebugOutput _instance;
        public static void SetSpeed(float value) => _instance.OutputSpeed(value);
        public static void SetTorque(float value) => _instance.OutputTorque(value);
        public static void SetBrake(float value) => _instance.OutputBreakTorque(value);
        public static void SetMaxSpeed(float value) => _instance.OutputMaxSpeed(value);
        
        public static void SetPlayerColor() => _instance.SetPlayerColorOutput();
        public static void SetEnemyColor() => _instance.SetEnemyColorOutput();
        
        

        private void Awake()
        {
            _instance = this;
            if(_active == false)
                gameObject.SetActive(false);
            OutputSpeed(0f);
            OutputTorque(0f);
            OutputBreakTorque(0f);
            OutputMaxSpeed(0f);
        }

        private void OutputSpeed(float value)
        {
            _speed.text = $"Speed: {value:N3}";
        }
        
        private void OutputTorque(float value)
        {
            _torque.text = $"Torque: {value:N3}";
        }
        
        private void OutputBreakTorque(float value)
        {
            _break.text = $"Brake: {value:N3}";
        }

        private void OutputMaxSpeed(float value)
        {
            _maxSpeed.text = $"MaxSpeed: {value:N3}";
        }
        
        private void SetEnemyColorOutput()
        {
            _speed.color = _enemyColor;
            _torque.color = _enemyColor;
            _break.color = _enemyColor;
            _maxSpeed.color = _enemyColor;
        }

        private void SetPlayerColorOutput()
        {
            _speed.color = _playerColor;
            _torque.color = _playerColor;
            _break.color = _playerColor;
            _maxSpeed.color = _playerColor;
        }
    }
}