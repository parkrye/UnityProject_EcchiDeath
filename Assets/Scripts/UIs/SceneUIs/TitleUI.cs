using UnityEngine;

public class TitleUI : SceneUI
{
    protected override void AwakeSelf()
    {
        base.AwakeSelf();

        if (GetButton("StartButton", out var sButton))
            sButton.onClick.AddListener(() => GameManager.Scene.LoadScene("MainScene"));
        if (GetButton("OptionButton", out var oButton))
            oButton.onClick.AddListener(() => GameManager.Scene.LoadScene("MainScene"));
        if (GetButton("QuitButton", out var qButton))
            qButton.onClick.AddListener(() =>
            {
                var spUI = GameManager.UI.ShowPopupUI<SelectPopUpUI>("UIs/SelectPopUpUI");
                spUI.Init(
                    "정말로 종료할 거야?",
                    () => Application.Quit(),
                    () => GameManager.UI.ClosePopupUI());
            });
    }
}
