using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public enum Talker
{
    Koharu,
    Hanako,
    Hasumi,
}

public class MainUI : SceneUI
{
    private (Sprite icon, string name)[] _talker;
    private Animator _animator;

    public UnityEvent OnTalkEvent = new UnityEvent();
    public UnityEvent<bool> OnJudgeEvent = new UnityEvent<bool>();

    private bool _isShowEnd = false;
    private bool _isDanger = false;

    protected override void AwakeSelf()
    {
        base.AwakeSelf();

        _talker = new(Sprite icon, string name)[]
        {
            (GameManager.Resource.Load<Sprite>("Sprites/Main/Koharu"), "코하루"),
            (GameManager.Resource.Load<Sprite>("Sprites/Main/Hanako"), "하나코"),
            (GameManager.Resource.Load<Sprite>("Sprites/Main/Hasumi"), "하스미"),
        };

        _animator = GetComponent<Animator>();

        if (GetButton("PassButton", out var pButton))
        {
            pButton.onClick.AddListener(() =>
            {
                pButton.interactable = false;
                OnJudgeEvent?.Invoke(false);
                pButton.interactable = true;
            });
        }
        if (GetButton("DeathButton", out var dButton))
        {
            dButton.onClick.AddListener(() =>
            {
                dButton.interactable = false;
                OnJudgeEvent?.Invoke(true);
                dButton.interactable = true;
            });
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

        if (GetButton("QuitButton", out var qButton))
        {
            qButton.onClick.AddListener(() =>
            {
                Time.timeScale = 0f;
                var spUI = GameManager.UI.ShowPopupUI<SelectPopUpUI>("SelectPopUpUI");
                spUI.Init(
                    "잠깐 쉬었다 할까?",
                    () =>
                    {
                        Time.timeScale = 1f;
                        GameManager.Scene.LoadScene("TitleScene");
                    },
                    () =>
                    {
                        Time.timeScale = 1f;
                        GameManager.UI.ClosePopupUI();
                    }
                );
            });
        }
    }

    public void InitUI()
    {
        _animator.Play("InitAnim");

        if (GetText("DayText", out var dText))
        {
            dText.text = $"{GameManager.Data.PlayData.Date}일";
        }
    }

    public void OnStartGame(int targetCount, bool isDanger = false)
    {
        _animator.Play("StartAnim");
        _isDanger = isDanger;

        if (GetImage("TalkerIcon", out var tImage))
        {
            tImage.enabled = true;
            tImage.sprite = _talker[(int)Talker.Koharu].icon;
        }
        if (GetText("TalkText", out var tText))
        {
            tText.text = string.Empty;
        }

        ModifyCounter(0, 0, targetCount);
    }

    public void OnEndGame()
    {
        _animator.Play("EndAnim");
    }

    public void Talk((Talker talker, string content) data)
    {
        if (GetImage("TalkerIcon", out var tImage))
        {
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

    public void ShowNextTarget(TargetData targetData, bool playShowAnim = false)
    {
        if (playShowAnim)
            _animator.Play("ShowAnim");

        if (GetText("TargetNameText", out var tnText))
        {
            tnText.text = targetData.Name;
        }

        if (GetImage("TargetIcon", out var tImage))
        {
            tImage.sprite = GameManager.Data.Sprites[targetData.Icon];
        }
        if (playShowAnim == false)
        {
            if (GetImage("TargetIconSub", out var tsImage))
            {
                tsImage.sprite = GameManager.Data.Sprites[targetData.Icon];
            }
        }
        StartCoroutine(ShowTargetEvent(targetData.Icon));

        if (GetText("TargetContentText", out var tcText))
        {
            var result = new StringBuilder();
            foreach( var element in targetData.Elements)
            {
                if (_isDanger == false)
                {
                    result.AppendLine(GameData.JudgeElements[element].GetDescription());
                    continue;
                }

                if (GameData.JudgeElements[element].EcchiPoint > 0)
                    result.AppendLine($"<color=red>{GameData.JudgeElements[element].GetDescription()}</color>");
                else if (GameData.JudgeElements[element].EcchiPoint == 0)
                    result.AppendLine($"<color=black>{GameData.JudgeElements[element].GetDescription()}</color>");
                else
                    result.AppendLine($"<color=blue>{GameData.JudgeElements[element].GetDescription()}</color>");
            }
            tcText.text = result.ToString();
        }

        if (GetScroll("TargetScroll", out var tScroll))
        {
            tScroll.normalizedPosition = Vector2.one;
        }
    }

    private IEnumerator ShowTargetEvent(int icon)
    {
        yield return new WaitUntil(() => _isShowEnd);
        _isShowEnd = false;

        if (GetImage("TargetIconSub", out var tsImage))
        {
            tsImage.sprite = GameManager.Data.Sprites[icon];
        }
    }

    private void OnShowEndEvent()
    {
        _isShowEnd = true;
    }

    public void OnModifyTimer(float ratio)
    {
        if (GetSlider("TimeSlider", out var slider))
        {
            slider.value = ratio;
        }
    }
}
