public class ResultUI : PopUpUI
{
    public void Init(int totalDeathCount, int totalPassCount, float scoreRatio)
    {
        if (GetText("ResultCountText", out var rcText))
        {
            rcText.text = $"�ѿ� {totalDeathCount + totalPassCount}";
        }
        if (GetText("ResultDeathCountText", out var rdcText))
        {
            rdcText.text = $"���� {totalDeathCount}";
        }
        if (GetText("ResulPassCountText", out var rpcText))
        {
            rpcText.text = $"�ؼ� { totalPassCount}";
        }
        if (GetText("ResultRatioText", out var rrText))
        {
            rrText.text = $"��Ȯ��\n\n{(int)scoreRatio}% ";
        }
    }
}
