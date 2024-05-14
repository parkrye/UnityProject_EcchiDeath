using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        var rect = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;

        var minAnchor = safeArea.position;
        var maxAnchor = minAnchor + safeArea.size;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;
        rect.anchorMax = maxAnchor;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        rect.anchorMin = minAnchor;
    }
}