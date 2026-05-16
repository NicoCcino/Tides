using Tides.Resources;
using TMPro;
using UnityEngine;

public class GatherPointInformationsDisplayer : MonoBehaviour
{
    [SerializeField] private GatherPointBehaviour gatherPointBehaviour;
    [SerializeField] private TMP_Text textAmount;

    private void Update()
    {
        textAmount.text = $"{gatherPointBehaviour.Resource.GetAmount()}";
    }
}
