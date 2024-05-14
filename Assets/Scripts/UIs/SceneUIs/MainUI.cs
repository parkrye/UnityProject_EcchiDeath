using System.Text;
using UnityEngine;
using UnityEngine.Events;

public enum Talker
{
    Koharu,
    Hanako,
}

public class MainUI : SceneUI
{
    private (Sprite icon, string name)[] _talker;
    private Animator _targetAnimation;

    public UnityEvent OnTalkEvent = new UnityEvent();
    public UnityEvent<bool> OnJudgeEvent = new UnityEvent<bool>();

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

        if (GetButton("PassButton", out var pButton))
        {
            pButton.onClick.AddListener(() => OnJudgeEvent?.Invoke(false));
        }
        if (GetButton("DeathButton", out var dButton))
        {
            dButton.onClick.AddListener(() => OnJudgeEvent?.Invoke(true));
        }
        if (GetButton("TalkButton", out var tButton))
        {
            tButton.onClick.AddListener(() =>
            {
                tButton.interactable = false;
                OnTalkEvent?.Invoke();
                tButton.interactable = true;
            });
        }
    }

    public void InitUI()
    {
        if (GetRect("StateSection", out var state))
        {
            state.gameObject.SetActive(false);
        }

        if (GetRect("Timer", out var timer))
        {
            timer.gameObject.SetActive(false);
        }

        _targetAnimation.Play("Next");

        if (GetRect("TargetSection", out var target))
        {
            target.gameObject.SetActive(false);
        }

        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = false;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = string.Empty;
        }

        if (GetButton("TalkButton", out var tButton))
        {
            tButton.gameObject.SetActive(true);
        }
    }

    public void OnStartGame(int targetCount)
    {
        if (GetRect("StateSection", out var state))
        {
            state.gameObject.SetActive(true);
        }
        if (GetText("DayText", out var dText))
        {
            dText.text = $"{GameManager.Data.PlayData.Date}일";
        }
        if (GetText("PassCountText", out var pcText))
        {
            pcText.text = "보류 0";
        }
        if (GetText("DeathCountText", out var dcText))
        {
            dcText.text = "사형 0";
        }
        if (GetText("RemainCountText", out var rcText))
        {
            rcText.text = $"잔여 {targetCount}";
        }

        if (GetRect("Timer", out var timer))
        {
            timer.gameObject.SetActive(true);
        }

        if (GetRect("TargetSection", out var target))
        {
            target.gameObject.SetActive(true);
        }

        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = true;
            tImage.sprite = _talker[(int)Talker.Koharu].icon;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = string.Empty;
        }

        if (GetButton("TalkButton", out var tButton))
        {
            tButton.gameObject.SetActive(false);
        }
    }

    public void OnEndGame()
    {
        if (GetRect("StateSection", out var state))
        {
            state.gameObject.SetActive(false);
        }

        if (GetRect("Timer", out var timer))
        {
            timer.gameObject.SetActive(false);
        }

        _targetAnimation.Play("Next");

        if (GetRect("TargetSection", out var target))
        {
            target.gameObject.SetActive(false);
        }

        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = false;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = string.Empty;
        }

        if (GetButton("TalkButton", out var tButton))
        {
            tButton.gameObject.SetActive(true);
        }
    }

    public void Talk((Talker talker, string content) data)
    {
        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = true;
            tImage.sprite = _talker[(int)data.talker].icon;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = $"{_talker[(int)data.talker].name} : {data.content}";
        }
    }

    public void ModifyCounter(int pass, int death, int remain)
    {
        if (GetText("PassCountText", out var pcText))
        {
            pcText.text = $"보류 {pass}";
        }
        if (GetText("DeathCountText", out var dcText))
        {
            dcText.text = $"사형 {death}";
        }
        if (GetText("RemainCountText", out var rcText))
        {
            rcText.text = $"잔여 {remain}";
        }
    }

    public void ShowTarget(TargetData targetData)
    {

        if (GetText("TargetNameText", out var tnText))
        {
            tnText.text = targetData.Name;
        }

        if (GetImage("TargetIcon", out var tImage))
        {
            tImage.sprite = GameManager.Data.Sprites[targetData.Icon];
        }

        if (GetText("TargetContentText", out var tcText))
        {
            var result = new StringBuilder();
            foreach( var element in targetData.Elements)
            {
                result.AppendLine(GameData.JudgeElements[element].Name);
            }
            tcText.text = result.ToString();
        }

        _targetAnimation.Play("Show");
    }
}
