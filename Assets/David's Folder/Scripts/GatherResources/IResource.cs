using System;
using UnityEngine;

public interface IResource
{
    public Action<int> OnAmountChanged { get; set; }
    public int GetAmount();
    protected void SetAmount(int amount);
    public bool CanConsume(int checkedAmount)
    {
        return checkedAmount < GetAmount();
    }
    public bool TryConsume(int consumedAmount)
    {
        if (CanConsume(consumedAmount))
        {
            Consume(consumedAmount);
            return true;
        }
        return false;
    }
    public void Add(int addedAmount);
    protected void Consume(int consumedAmount)
    {
        SetAmount(GetAmount() - consumedAmount);
    }
}
