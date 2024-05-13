using UnityEngine.Events;

public class SelectPopUpUI : PopUpUI
{
    public void Init(string description, UnityAction yesAction, UnityAction noAction)
    {
        if (GetText("DescriptionText", out var dText))
            dText.text = description;
        if (GetButton("YesButton", out var yButton))
        {
            yButton.onClick.RemoveAllListeners();
            yButton.onClick.AddListener(yesAction);
        }
        if (GetButton("NoButton", out var nButton))
        {
            nButton.onClick.RemoveAllListeners();
            yButton.onClick.AddListener(noAction);
        }
    }
}
