using System;
using UnityEngine;

public class FoodResource : IResource
{
    protected int foodAmount;

    public FoodResource(int amount)
    {
        foodAmount = amount;
    }

    public Action<int> OnAmountChanged { get; set; }
    public Action OnFailedConsumed { get; set; }

    public void Add(int addedAmount)
    {
        foodAmount += addedAmount;
        OnAmountChanged?.Invoke(foodAmount);
    }

    public int GetAmount()
    {
        return foodAmount;
    }

    void IResource.SetAmount(int amount)
    {
        foodAmount = amount;
        OnAmountChanged?.Invoke(foodAmount);
    }
}
