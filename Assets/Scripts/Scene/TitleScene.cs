using System.Collections;

public class TitleScene : BaseScene
{
    private bool _isLoaded = false;

    private void Start()
    {
        if (_isLoaded)
            return;
        StartCoroutine(LoadingRoutine());
    }

    protected override IEnumerator LoadingRoutine()
    {
        if (_isLoaded)
            yield break;

        _isLoaded = true;
        yield return null;

        Progress = 1f;

    }
}
