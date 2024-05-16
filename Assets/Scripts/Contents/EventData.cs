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
    DayEnd_Good,
    DayEnd_Bad_Good,
    DayEnd_Bad_Normal,
    DayEnd_Bad_Bad,
    Ending_Good,
    Ending_Normal,
    Ending_Bad,
}
