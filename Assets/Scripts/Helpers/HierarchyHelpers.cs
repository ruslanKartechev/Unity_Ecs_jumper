using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public delegate bool Condition<T>(T target);
    
    public static class HierarchyHelpers
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static List<T> GetFromAllChildren<T>(Transform parent, Condition<T> condition = null)
        {
            var list = new List<T>();
            var t = parent.GetComponent<T>();
            if (t != null)
            {
                if((condition != null && condition(t)) 
                   || condition == null)
                    list.Add(t);
            }
            GetFromChildrenAndAdd(list, parent.transform, condition);
            
            return list;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void GetFromChildrenAndAdd<T>(ICollection<T> list, Transform parent, Condition<T> condition = null)
        {
            if (parent.childCount == 0)
                return;
            for (var i = 0; i < parent.childCount; i++)
            {
                var target = parent.GetChild(i).GetComponent<T>();
                if (target != null )
                {
                    if((condition != null && condition(target)) 
                       || condition == null)
                        list.Add(target);
       
                }
                GetFromChildrenAndAdd<T>(list, parent.GetChild(i), condition);
            }
        }
    }
}