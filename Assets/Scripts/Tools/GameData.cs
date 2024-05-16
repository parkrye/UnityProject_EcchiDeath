using System.Collections.Generic;
using UnityEngine;

public enum SimpleTalkEnum
{
    JudgeDeath,
    JudgePass,
    JudgeTimeout,
}

public static class GameData
{
    public static readonly List<JudgeElement> JudgeElements = new List<JudgeElement>();

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

    public static (Talker talker, string content)[] SimpleTalk = new (Talker talker, string content)[]
    {
        (Talker.Koharu, "���� �� �ȵ�! ����!"),
        (Talker.Koharu, "�̰�...�� ������ ����..."),
        (Talker.Koharu, "��...����...! ����!"),
    };
}

public class JudgeElement
{
    public string Description;
    public int EcchiPoint;
    public string[] Parameters;

    public JudgeElement(string description, int ecchiPoint, params string[] parameters)
    {
        Description = description;
        EcchiPoint = ecchiPoint;
        Parameters = parameters;
    }

    public string GetDescription()
    {
        if (Parameters == null)
            return Description;
        return string.Format(Description, Parameters);
    }
}