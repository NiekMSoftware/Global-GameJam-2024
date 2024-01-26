using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;
    [SerializeField] private GameObject mainMissionImage;

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetDescription(string text)
    {
        description.text = text;
    }

    public Button GetButton()
    {
        return button;
    }

    public void SetMainMissionImage(bool value)
    {
        mainMissionImage.SetActive(value);
    }
}