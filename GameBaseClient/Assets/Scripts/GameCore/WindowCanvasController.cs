using UnityEngine;
using UnityEngine.UI;

public class WindowCanvasController : MonoBehaviour
{
    public Canvas _canvas;
    public CanvasScaler _canvasScaler;
    readonly Vector2Int PanelRefereceResolution = new Vector2Int(3080, 2160);
    private static int NextSortOrder = 1;
    private int ThisSortOrder = 0;
    private void Awake()
    {
        TryInitialize();
    }
    private void OnEnable()
    {
        ThisSortOrder = NextSortOrder;
        NextSortOrder++;
        _canvas.sortingOrder = ThisSortOrder;
    }
    public void TryInitialize()
    {
        if (_canvas == null)
        {
            _canvas = GetComponent<Canvas>();
        }

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = CameraRootController.GetFieldCamera();

        if (_canvasScaler == null)
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = PanelRefereceResolution;
    }
}
