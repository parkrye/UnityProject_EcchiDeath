using UnityEngine;

[CreateAssetMenu (fileName = "EventData")]
public class EventData : ScriptableObject
{
    public Talker[] Talkers;
    public string[] Contents;

    public (Talker talker, string content) GetTalk(int index)
    {
        return (Talkers[index], Contents[index]);
    }
}

public enum EventType
{
    Opening,
    DayStart,
    DayEnd,
}
