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
            new JudgeElement("{0} 소지", 1, "야한 책"),
            new JudgeElement("{0} 소지", -1, "성경"),
            new JudgeElement("{0}, {1} 소지", 0, "야한 책", "성경"),
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