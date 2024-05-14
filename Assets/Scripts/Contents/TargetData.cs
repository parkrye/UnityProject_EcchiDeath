using UnityEngine;

public class TargetData
{
    public string Name { get; private set; }
    public int[] Elements { get; private set; }
    public bool IsGuilty { get; private set; }

    public TargetData(int count = 0)
    {
        Name = RandomNaming.GetName();

        count = Random.Range(count, count * 2);

        Elements = GameData.GetRandomJudgeElementIndex(count);

        var guiltyCount = 0;
        foreach (var element in Elements)
        {
            guiltyCount += GameData.JudgeElements[element].EcchiPoint;
        }
        IsGuilty = guiltyCount > 0;
    }
}
