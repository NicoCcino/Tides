using System;
using UnityEngine;

public class WoodResource : IResource
{
    protected int woodAmount;

    public WoodResource(int amount)
    {
        woodAmount = amount;
    }

    public Action<int> OnAmountChanged { get; set; }
    public Action OnFailedConsumed { get; set; }

    public void Add(int addedAmount)
    {
        woodAmount += addedAmount;
        OnAmountChanged?.Invoke(woodAmount);
    }

    public int GetAmount()
    {
        return woodAmount;
    }

    void IResource.SetAmount(int amount)
    {
        woodAmount = amount;
        OnAmountChanged?.Invoke(woodAmount);
    }
}
