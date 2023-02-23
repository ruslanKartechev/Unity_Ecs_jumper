#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Level.Impl
{
    [CustomEditor(typeof(LevelView))]
    public class LevelViewEditor : Editor
    {
        private LevelView me;

        private void OnEnable()
        {
            me = target as LevelView;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn"))
            {
                me.GenerateGrid();
                EditorUtility.SetDirty(me);
            }
            if (GUILayout.Button("Clear"))
            {
                me.Clear();
                EditorUtility.SetDirty(me);
            }
        }
    }
}
#endif