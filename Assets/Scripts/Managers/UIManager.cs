using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : BaseManager
{
    private EventSystem _eventSystem;
    private Stack<PopUpUI> _popUpStack = new Stack<PopUpUI>();

    public override void Initialize()
    {
        base.Initialize();

        _eventSystem = GameManager.Resource.Instantiate<EventSystem>("UIs/EventSystem");
        _eventSystem.transform.parent = transform;
    }

    public T ShowPopupUI<T>(T popup) where T : PopUpUI
    {
        T ui = GameManager.Pool.GetUI<T>(popup);
        _popUpStack.Push(ui);
        return ui;
    }

    public T ShowPopupUI<T>(string path) where T : PopUpUI
    {
        T uI = GameManager.Resource.Load<T>(path);
        return ShowPopupUI(uI);
    }

    public void ClosePopupUI()
    {
        GameManager.Pool.ReleaseUI(_popUpStack.Pop());
    }
}