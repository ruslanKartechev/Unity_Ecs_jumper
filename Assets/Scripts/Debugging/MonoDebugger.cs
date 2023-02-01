using System;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using Services.MonoHelpers;
using UnityEngine;
using Zenject;
using UnityEditor;

namespace Debugging
{
    public class MonoDebugger : MonoBehaviour
    {
        [Inject] private ICoroutineService _coroutineService;


        public void Start()
        {
            
        }

        public void FailGame()
        {
            Pool.World.AddComponentToNew<FailLevelComponent>();
        }

        public void WinGame()
        {
            Pool.World.AddComponentToNew<WinLevelComponent>();
            
        }
    }
    
    
    
    #if UNITY_EDITOR

    [CustomEditor(typeof(MonoDebugger))]
    public class MonoDebuggerEditor : Editor
    {
        private MonoDebugger me;

        private void OnEnable()
        {
            me = target as MonoDebugger;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fail"))
            {
                me.FailGame();
            }
            if (GUILayout.Button("Win"))
            {
                me.WinGame();
            }
            GUILayout.EndHorizontal();
                        
        }
    }
    
    #endif
}