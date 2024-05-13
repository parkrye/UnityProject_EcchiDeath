using UnityEngine;

public class DataManager : BaseManager
{
    public override void Initialize()
    {
        base.Initialize();

        Application.runInBackground = false;
    }
}