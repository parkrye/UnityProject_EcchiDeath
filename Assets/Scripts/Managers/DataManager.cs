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
}