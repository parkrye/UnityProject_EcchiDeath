using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    private MainUI _mainUI;

    private int _eventIndex;
    private int _contentIndex;

    private TargetData[] _targets;
    private int _targetIndex;
    private int _passCount = 0;
    private int _deathCount = 0;

    protected override IEnumerator LoadingRoutine()
    {
        _mainUI = FindFirstObjectByType<MainUI>();

        _mainUI.OnJudgeEvent.AddListener(JudgeAction);
        _mainUI.OnTalkEvent.AddListener(TalkAction);
        _mainUI.InitUI();

        _contentIndex = 0;
        _targetIndex = 0;

        yield return new WaitForSeconds(.5f);
        Progress = 1f;
    }

    public override void StartScenePlayables()
    {
        base.StartScenePlayables();

        if (GameManager.Data.PlayData.Date == 0)
        {
            GameManager.Data.SetPlayerData(date: 1);
            _eventIndex = (int)EventType.Opening;
        }
        else
        {
            GameManager.Data.AddPlayerData(date: 1);
            _eventIndex = (int)EventType.DayStart;
        }

        var modifier = 0;
        var date = GameManager.Data.PlayData.Date;
        while (date >= 1)
        {
            date = (int)(date * 0.1f);
            modifier++;
        }
        var count = modifier * 10;
        _targets = new TargetData[count];
        for (int i = 0; i < count; i++)
        {
            _targets[i] = new TargetData(modifier * 5);
        }

        TalkAction();
    }

    private void TalkAction()
    {
        if (_eventIndex < 0 || _eventIndex >= GameManager.Data.Events.Length ||
            _contentIndex < 0 || _contentIndex >= GameManager.Data.Events[_eventIndex].Contents.Length)
        {
            if ((EventType)_eventIndex == EventType.DayEnd)
            {
                GameManager.Data.AddPlayerData(date: 1);
                GameManager.Scene.LoadScene("MainScene");
                return;
            }
            _mainUI.OnStartGame(_targets.Length);
            StopAllCoroutines();
            StartCoroutine(Timer());
            _mainUI.ShowNextTarget(_targets[_targetIndex]);
            _contentIndex = 0;
            return;
        }

        _mainUI.Talk(GameManager.Data.Events[_eventIndex].GetTalk(_contentIndex));
        _contentIndex++;
    }

    private void JudgeAction(bool isGuilty)
    {
        if (_targetIndex < 0 || _targetIndex >= _targets.Length)
        {
            _eventIndex = (int)EventType.DayEnd;
            _mainUI.OnEndGame();
            TalkAction();
            return;
        }

        if (isGuilty)
        {
            _deathCount++;
            GameManager.Data.AddPlayerData(deathCount: 1, score: isGuilty == _targets[_targetIndex].IsGuilty ? 1 : -1);
            _mainUI.Talk(GameData.SimpleTalk[(int)SimpleTalkEnum.JudgeDeath]);
        }
        else
        {
            _passCount++;
            GameManager.Data.AddPlayerData(passCount: 1, score: isGuilty == _targets[_targetIndex].IsGuilty ? 1 : -1);
            _mainUI.Talk(GameData.SimpleTalk[(int)SimpleTalkEnum.JudgePass]);
        }

        _targetIndex++;
        _mainUI.ModifyCounter(_passCount, _deathCount, _targets.Length - _targetIndex);

        if (_targetIndex < 0 || _targetIndex >= _targets.Length)
        {
            _eventIndex = (int)EventType.DayEnd;
            _mainUI.OnEndGame();
            TalkAction();
            return;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Timer());
            _mainUI.ShowNextTarget(_targets[_targetIndex], true);
        }
    }

    private IEnumerator Timer()
    {
        var current = _targetIndex;
        var timer = 1f;
        while (current == _targetIndex && timer > 0f)
        {
            if (current == _targetIndex)
                _mainUI.OnModifyTimer(timer);
            timer -= Time.deltaTime * 0.1f;
            yield return null;
        }

        if (current == _targetIndex)
        {
            JudgeAction(true);
            _mainUI.Talk(GameData.SimpleTalk[(int)SimpleTalkEnum.JudgeTimeout]);
        }
    }
}
