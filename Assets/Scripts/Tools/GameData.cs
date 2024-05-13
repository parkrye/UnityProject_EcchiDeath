using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static List<JudgeElement> JudgeElements = new List<JudgeElement>();

    public static int[] GetRandomJudgeElementIndex(int count)
    {
        var result = new int[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = Random.Range(0, JudgeElements.Count);
            var isSame = false;
            for (int j = 0; j < count; j++)
            {
                if (result[i] == result[j])
                {
                    isSame = true;
                    break;
                }
            }
            if (!isSame)
                i--;
        }
        return result;
    }
}

public class JudgeElement
{
    public string Name;
    public int EcchiPoint;

    public JudgeElement(string name, int ecchiPoint)
    {
        Name = name;
        EcchiPoint = ecchiPoint;
    }
}