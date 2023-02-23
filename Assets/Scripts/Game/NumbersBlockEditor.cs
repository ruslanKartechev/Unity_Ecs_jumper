#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(NumbersBlockView))]
    public class NumbersBlockEditor : Editor
    {
        private NumbersBlockView me;
        private void OnEnable()
        {
            me = target as NumbersBlockView;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn"))
            {
                me.Spawn();
                EditorUtility.SetDirty(me);
            }

            if (GUILayout.Button("Clear"))
            {
                me.Clear();
                EditorUtility.SetDirty(me);
            }

            if (GUILayout.Button("Test"))
            {
                EditorUtility.SetDirty(me);
                me.StartTest();
            }
        }
    }
}
#endif