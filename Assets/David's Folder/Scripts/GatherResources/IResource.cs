using System;
using UnityEngine;

public interface IResource
{
    public Action<int> OnAmountChanged { get; set; }
    public Action OnFailedConsumed { get; set; }
    public int GetAmount();
    protected void SetAmount(int amount);
    public bool CanConsume(int checkedAmount)
    {
        return checkedAmount <= GetAmount();
    }
    public bool TryConsume(int consumedAmount)
    {
        if (CanConsume(consumedAmount))
        {
            Consume(consumedAmount);
            return true;
        }
        OnFailedConsumed?.Invoke();
        return false;
    }
    public void Add(int addedAmount);
    protected void Consume(int consumedAmount)
    {
        SetAmount(GetAmount() - consumedAmount);
    }
}
