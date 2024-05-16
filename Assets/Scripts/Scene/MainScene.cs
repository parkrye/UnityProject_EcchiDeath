using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    private MainUI _mainUI;

    private int _eventIndex;
    private int _contentIndex;

    private TargetData[] _targets;
    private int _targetIndex;

    private int _todayPassCount = 0;
    private int _todayDeathCount = 0;
    private int _todayScore = 0;

    protected override IEnumerator LoadingRoutine()
    {
        _mainUI = FindFirstObjectByType<MainUI>();

        _mainUI.OnJudgeEvent.AddListener(JudgeAction);
        _mainUI.OnTalkEvent.AddListener(TalkAction);

        if (GameManager.Data.PlayData.Date == 0)
        {
            GameManager.Data.SetPlayerData(date: 1);
            _eventIndex = (int)EventType.Opening;
        }
        else if (GameManager.Data.PlayData.Date <= 30)
        {
            GameManager.Data.AddPlayerData(date: 1);
            _eventIndex = (int)EventType.DayStart;
        }
        else
        {
            GameManager.Data.SetPlayerData(date: 31);
            _eventIndex = (int)EventType.DayStart;
        }
        _mainUI.InitUI();

        _contentIndex = 0;
        _targetIndex = 0;

        yield return new WaitForSeconds(.5f);
        Progress = 1f;
    }

    public override void StartScenePlayables()
    {
        base.StartScenePlayables();

        if (GameManager.Data.PlayData.Date > 30)
        {
            var scoreRatio = (GameManager.Data.PlayData.Score * 100f) / (GameManager.Data.PlayData.DeathCount + GameManager.Data.PlayData.PassCount);

            var resultUI = GameManager.UI.ShowPopupUI<ResultUI>("ResultUI");
            resultUI.Init(GameManager.Data.PlayData.DeathCount, GameManager.Data.PlayData.PassCount, scoreRatio);
            if (scoreRatio > 80f)
                _eventIndex = (int)EventType.Ending_Good;
            else if (scoreRatio >= 40f)
                _eventIndex = (int)EventType.Ending_Normal;
            else
                _eventIndex = (int)EventType.Ending_Bad;

            TalkAction();
            return;
        }

        var date = GameManager.Data.PlayData.Date;
        if (date <= 5)
            date = 5;
        else if (date <= 10)
            date = 10;
        else if (date <= 15)
            date = 15;
        else if (date <= 20)
            date = 20;
        else if (date <= 25)
            date = 25;
        else
            date = 30;

        _targets = new TargetData[date];
        for (int i = 0; i < date; i++)
        {
            _targets[i] = new TargetData(date);
        }

        TalkAction();
    }

    private void TalkAction()
    {
        if (_eventIndex < 0 || _eventIndex >= GameManager.Data.Events.Length ||
            _contentIndex < 0 || _contentIndex >= GameManager.Data.Events[_eventIndex].Contents.Length)
        {

            if (GameManager.Data.PlayData.Date > 30)
            {
                GameManager.UI.ClosePopupUI();
                GameManager.Data.ResetData();
                GameManager.Scene.LoadScene("TitleScene");
                return;
            }

            if ((EventType)_eventIndex != EventType.Opening && (EventType)_eventIndex != EventType.DayStart)
            {
                GameManager.UI.ClosePopupUI();
                GameManager.Data.AddPlayerData(date: 1);
                GameManager.Scene.LoadScene("MainScene");
                return;
            }

            var scoreRatio = (GameManager.Data.PlayData.Score * 100f) / (GameManager.Data.PlayData.DeathCount + GameManager.Data.PlayData.PassCount);
            _mainUI.OnStartGame(_targets.Length, scoreRatio < 40f);
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
            EndToday();
            return;
        }

        if (isGuilty)
        {
            _todayDeathCount++;
            GameManager.Data.AddPlayerData(deathCount: 1);
            _mainUI.Talk(GameData.SimpleTalk[(int)SimpleTalkEnum.JudgeDeath]);
        }
        else
        {
            _todayPassCount++;
            GameManager.Data.AddPlayerData(passCount: 1);
            _mainUI.Talk(GameData.SimpleTalk[(int)SimpleTalkEnum.JudgePass]);
        }
        GameManager.Data.AddPlayerData(score: isGuilty == _targets[_targetIndex].IsGuilty ? 1 : 0);
        _todayScore += isGuilty == _targets[_targetIndex].IsGuilty ? 1 : 0;

        _targetIndex++;
        _mainUI.ModifyCounter(_todayPassCount, _todayDeathCount, _targets.Length - _targetIndex);

        if (_targetIndex < 0 || _targetIndex >= _targets.Length)
        {
            EndToday();
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

    private void EndToday()
    {
        var scoreRatio = (GameManager.Data.PlayData.Score * 100f) / (GameManager.Data.PlayData.DeathCount + GameManager.Data.PlayData.PassCount);

        var resultUI = GameManager.UI.ShowPopupUI<ResultUI>("ResultUI");
        resultUI.Init(GameManager.Data.PlayData.DeathCount, GameManager.Data.PlayData.PassCount, scoreRatio);
        if (_todayScore < _targets.Length * 0.5f)
        {
            if (scoreRatio > 80f)
                _eventIndex = (int)EventType.DayEnd_Bad_Good;
            else if (scoreRatio >= 40f)
                _eventIndex = (int)EventType.DayEnd_Bad_Normal;
            else
                _eventIndex = (int)EventType.DayEnd_Bad_Bad;
        }
        else
        {
            _eventIndex = (int)EventType.DayEnd_Good;
        }
        _mainUI.OnEndGame();
        TalkAction();
    }
}
