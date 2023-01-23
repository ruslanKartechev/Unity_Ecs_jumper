using Services.Money.Impl;
using UnityEngine;

namespace Services.Money
{
    [CreateAssetMenu(fileName = nameof(MoneyDubber), menuName = "SO/Money/" + nameof(MoneyDubber))]
    public class MoneyDubber : ScriptableObject
    {
        public int Count
        {
            get => PlayerPrefs.GetInt(MoneyService.MONEY_KEY, 0);
        }
        
        public void SetCount(int count)
        {
            PlayerPrefs.SetInt(MoneyService.MONEY_KEY, count);
        }

    }
}