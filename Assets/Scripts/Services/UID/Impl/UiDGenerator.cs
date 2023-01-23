using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.UID.Impl
{
    public class UiDGenerator : IUiDGenerator
    {
        private HashSet<int> _generated = new HashSet<int>();
        private static UiDGenerator _instance;
        
        public UiDGenerator()
        {
            if(_instance == null)
                _instance = this;
        }

        public static int GetNewUid(int characterCount)
        {
            return _instance.GetUid(characterCount);
        }
        
        public int GetUid(int characterCount)
        {
            if (characterCount < 3)
                throw new SystemException($"[UiDGenerator] CharacterCount < 3 is not allowed");
            var result = int.MinValue;
            do
            {
                result = GenerateUid(characterCount);
            } while (_generated.Contains(result));

            _generated.Add(result);
            return result;
        }

        private int GenerateUid(int count)
        {
            var result = 0;
            for (int i = 0; i < count; i++)
            {
                var n = UnityEngine.Random.Range(0, 10);
                result += n * (int)Mathf.Pow(10, i);
            }
            return result;
        }
    }
}