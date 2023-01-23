using UnityEngine;

namespace Services.Money
{
    public interface IMoneyDropManager
    {
        void Init();
        bool IsEnabled { get; set; }
        void DropMoney(Vector3 fromPosition, int dropCount, int moneyPerDrop);
    }
}