using Services.Money.Listeners;

namespace Services.Money
{
    public interface IMoneyService
    {
        int Money { get; }
        void AddMoney(int count);
        void RemoveMoney(int count);
        void AddMoneyCountListener(IMoneyCountListener listener);
    }
}