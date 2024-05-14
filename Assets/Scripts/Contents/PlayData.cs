using UnityEngine;

[CreateAssetMenu (fileName = "PlayData")]
public class PlayData : ScriptableObject
{
    public int Date;
    public int PassCount;
    public int DeathCount;
    public int Score;
}