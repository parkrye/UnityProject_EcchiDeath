using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : SceneUI
{
    private (Sprite icon, string name)[] _talker;
    private Animator _targetAnimation;

    protected override void AwakeSelf()
    {
        base.AwakeSelf();

        _talker = new(Sprite icon, string name)[]
        {
            (GameManager.Resource.Load<Sprite>("Sprites/Main/Koharu"), "코하루"),
            (GameManager.Resource.Load<Sprite>("Sprites/Main/Hanako"), "하나코"),
        };

        if (GetRect("TargetSection", out var tSection))
        {
            _targetAnimation = tSection.GetComponent<Animator>();
        }
    }

    public void InitUI(int date)
    {
        if (GetText("DayText", out var dText))
        {
            dText.text = $"{date}일째";
        }
        if (GetText("PassCountText", out var pcText))
        {
            pcText.text = "보류 0";
        }
        if (GetText("DeathCountText", out var dcText))
        {
            dcText.text = "사형 0";
        }

        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = false;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = string.Empty;
        }

        _targetAnimation.Play("Next");
    }

    public void OnStartGame()
    {

    }

    public void OnEndGame()
    {

    }
}
