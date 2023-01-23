#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Services.Money
{
    [CustomEditor(typeof(MoneyDubber))]
    public class MoneyDubberEditor : Editor
    {
        private MoneyDubber me;

        private void OnEnable()
        {
            me = target as MoneyDubber;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Clear"))
            {
                me.SetCount(0);
            }

            if (GUILayout.Button("Set_1000"))
            {
                me.SetCount(1000);
            }
        }
        
    }
}
#endif