using UnityEngine;

public class DataManager : BaseManager
{
    public PlayData PlayData { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        Application.runInBackground = false;

        var judgeElements = new JudgeElement[] 
        { 
            new JudgeElement("Test", 0),
        };
        GameData.JudgeElements.AddRange(judgeElements);

        PlayData = GameManager.Resource.Load<PlayData>("PlayData");
    }

    public void AddPlayerData(int date = 1, int passCount = 0, int deathCount = 0, int score = 0)
    {
        PlayData.Date += date;
        PlayData.PassCount += passCount;
        PlayData.DeathCount += deathCount;
        PlayData.Score += score;
    }
}