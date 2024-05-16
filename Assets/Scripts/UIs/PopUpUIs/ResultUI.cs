public class ResultUI : PopUpUI
{
    public void Init(int totalDeathCount, int totalPassCount, float scoreRatio)
    {
        if (GetText("ResultCountText", out var rcText))
        {
            rcText.text = $"총원 {totalDeathCount + totalPassCount}";
        }
        if (GetText("ResultDeathCountText", out var rdcText))
        {
            rdcText.text = $"위반 {totalDeathCount}";
        }
        if (GetText("ResulPassCountText", out var rpcText))
        {
            rpcText.text = $"준수 { totalPassCount}";
        }
        if (GetText("ResultRatioText", out var rrText))
        {
            rrText.text = $"정확도\n\n{(int)scoreRatio}% ";
        }
    }
}
