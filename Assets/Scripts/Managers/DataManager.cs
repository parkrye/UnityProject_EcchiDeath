using System.Linq;
using UnityEngine;

public class DataManager : BaseManager
{
    public PlayData PlayData { get; private set; }
    public EventData[] Events { get; private set; }
    public Sprite[] Sprites { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        Application.runInBackground = false;

        var judgeElements = new JudgeElement[] 
        { 
            new JudgeElement("테스트용 처형 대상", 1),
            new JudgeElement("테스트용 보류 대상", -1),
            new JudgeElement("테스트용 무의미 대상", 0),
        };
        GameData.JudgeElements.AddRange(judgeElements);

        PlayData = GameManager.Resource.Load<PlayData>("PlayData");
        Events = GameManager.Resource.LoadAll<EventData>("Events").OrderBy(t => t.name).ToArray();
        Sprites = GameManager.Resource.LoadAll<Sprite>("Sprites/NPCs").OrderBy(t => t.name).ToArray();
    }

    public void AddPlayerData(int date = 0, int passCount = 0, int deathCount = 0, int score = 0)
    {
        PlayData.Date += date;
        PlayData.PassCount += passCount;
        PlayData.DeathCount += deathCount;
        PlayData.Score += score;
    }

    public void SetPlayerData(int date = 0, int passCount = 0, int deathCount = 0, int score = 0)
    {
        PlayData.Date = date;
        PlayData.PassCount = passCount;
        PlayData.DeathCount = deathCount;
        PlayData.Score = score;
    }
}