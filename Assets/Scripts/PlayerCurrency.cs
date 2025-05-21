using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MoneyChangeEvent : UnityEvent<int, int> { }

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int currentMoney = 0;

    public MoneyChangeEvent OnMoneyChanged = new MoneyChangeEvent();
    public UnityEvent<int> OnMoneyAdded = new UnityEvent<int>();
    public UnityEvent<int> OnMoneySpent = new UnityEvent<int>();

    public int CurrentMoney
    {
        get { return currentMoney; }
        private set
        {
            int oldValue = currentMoney;
            currentMoney = value;
            OnMoneyChanged?.Invoke(currentMoney - oldValue, currentMoney);
        }
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        CurrentMoney += amount;
        OnMoneyAdded?.Invoke(amount);
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return true;

        if (currentMoney >= amount)
        {
            CurrentMoney -= amount;
            OnMoneySpent?.Invoke(amount);
            return true;
        }
        return false;
    }
}