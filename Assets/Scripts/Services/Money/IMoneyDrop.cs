using Services.Pool;
using UnityEngine;

namespace Services.Money
{
    public interface IMoneyDrop : IPooledObject<IMoneyDrop>
    {
        bool IsActive { get; set; }
        int MoneyCount { get; set; }
        void InitPosition(Vector3 position);
        void Drop(Vector3 dropForce);
        Vector3 Position { get; }
    }
}