using UnityEngine;

public class TargetData
{
    public string Name { get; private set; }
    public int[] Elements { get; private set; }

    public void Init(string name, int count = 0, int rand = 0)
    {
        Name = name;
        if (rand > 0)
            count += Random.Range(0, rand);

        Elements = new int[count];
        var selects = GameData.GetRandomJudgeElementIndex(count);
        for (int i = 0; i < selects.Length; i++)
        {
            Elements[i] = selects[i];
        }
    }
}
