using System.Collections.Generic;
using Data.Impl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace Game.Level.Impl
{
    public class LevelView : MonoBehaviour
    {
        public float spawnDelay;
        
        public int gridSize = 3;
        public float cellSize = 2;
        public SpawnDelayData spawnDelayData;
        public GameObject _pointPrefab;
        public List<Transform> _spawnPoints;
        public TearBlockMatData tearBlockMatData;
        [SerializeField] NumbersBlockView _numberBlocks;

        public INumberBlockView NumberBlock => _numberBlocks;
        
        
        
        #if UNITY_EDITOR
         public void GenerateGrid()
         {
             var offsetVector = new Vector3(1f, 0f, 1f) * ((int)(gridSize / 2) * cellSize);
             for (int i = 0; i < gridSize; i++)
             {
                 for (int k = 0; k < gridSize; k++)
                 {
                     var pos = new Vector3(i, 0f, k) * cellSize - offsetVector;
                     var point = PrefabUtility.InstantiatePrefab(_pointPrefab) as GameObject;
                     point.gameObject.name = $"pos {i}, {k}";
                     point.transform.parent = transform;
                     point.transform.position = pos;
                     _spawnPoints.Add(point.transform);
                 }
             }
         }

         public void Clear()
         {
             for (int i = _spawnPoints.Count - 1; i >= 0; i--)
             {
                 DestroyImmediate(_spawnPoints[i].GameObject());
                 _spawnPoints.RemoveAt(i);
             }
         }
         #endif
    }
}
