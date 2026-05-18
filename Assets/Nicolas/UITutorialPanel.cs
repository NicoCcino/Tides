using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UITutorialPanel : MonoBehaviour
{
    public Toggle toggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClosePanel()
    {
        IntroCutscene.Instance.TryPlay();
        toggle.isOn = false;
        gameObject.SetActive(false);
    }
}
