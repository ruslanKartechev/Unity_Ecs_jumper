using System.Collections.Generic;
using Services.Money.Listeners;
using UnityEngine;

namespace Services.Money.Impl
{
    public class MoneyService : IMoneyService
    {
        private readonly HashSet<IMoneyCountListener> _moneyCountListeners = new HashSet<IMoneyCountListener>();
        public const string MONEY_KEY = "Money";

        public int Money => PlayerPrefs.GetInt(MONEY_KEY, 0);

        public void AddMoney(int count)
        {
            SetCount(Money + count);
        }

        public void RemoveMoney(int count)
        {
            var nCount = Money - count;
            if (nCount < 0)
                nCount = 0;
            SetCount(nCount);
        }

        public void AddMoneyCountListener(IMoneyCountListener listener)
        {
            _moneyCountListeners.Add(listener);
        }
        
        private void SetCount(int count)
        {
            PlayerPrefs.SetInt(MONEY_KEY, count);
            foreach (var listener in _moneyCountListeners)
            {
                listener.OnMoneyCount(count);
            }
        }
        
        
    }
}