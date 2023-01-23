using System.Collections.Generic;
using System.Linq;
using Data.Prefabs;
using Helpers;
using Services.Instantiate;
using Services.Pool;
using UnityEngine;
using Zenject;

namespace Services.Money.Impl
{
    public class MoneyDropManager : IMoneyDropManager, IPool<IMoneyDrop>
    {
        [Inject] private IPrefabsRepository _prefabsRepository;
        [Inject] private IInstantiateService _instantiateService;

        private HashSet<IMoneyDrop> _pool = new HashSet<IMoneyDrop>();
        private bool _isEnabled;
        private float _dropForce = 4;
        
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
            } 
        }
        
        public void Init()
        {
            
        }

        public void DropMoney(Vector3 fromPosition, int dropCount, int moneyPerDrop)
        {
            if (_isEnabled == false)
            {
                Dbg.Log("[MoneyDropManager] _isEnabled == false");
                return;
            }

            for (int i = 0; i < dropCount; i++)
            {
                var drop = GetItem();
                drop.IsActive = true;
                drop.InitPosition(fromPosition);
                drop.MoneyCount = moneyPerDrop;
                drop.Drop(UnityEngine.Random.onUnitSphere.normalized * _dropForce);
            }
        }
        

        public IMoneyDrop GetItem()
        {
            if(_pool.Count > 0)
            {
                var res = _pool.FirstOrDefault(t => t != null);
                _pool.Remove(res);
                return res;
            }

            var prefab = _prefabsRepository.GetPrefabGO(PrefabNames.MoneyDrop);
            var drop = _instantiateService.Spawn<IMoneyDrop>(prefab);
            drop.Init(this);
            return drop;
        }

        public void Return(IMoneyDrop target)
        {
            _pool.Add(target);
        }

        public void CollectAllBack()
        {
            foreach (var item in _pool)
            {
                item.CollectBack();
            }
        }
    }
}