using UnityEngine;

[System.Serializable]
public class TideCycle
{
    public TideCycle(int cycleNumber, int tideCoefficient)
    {
        this.cycleNumber = cycleNumber;
        this.tideCoefficient = tideCoefficient;
    }
    public int cycleNumber;
    public int tideCoefficient;
    public int WaveHeight => tideCoefficient;
    public int RestHeight => -tideCoefficient;

}
