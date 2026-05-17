using TMPro;
using UnityEngine;

public class UIDefeatReason : MonoBehaviour
{
    [SerializeField] private TMP_Text textDefeatReason;

    private void OnEnable()
    {
        textDefeatReason.text = GamesRulesManager.Instance.LatestLooseReason;
    }
}
