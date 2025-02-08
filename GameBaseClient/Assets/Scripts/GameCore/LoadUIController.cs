using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
public class LoadScope : IDisposable
{
    private bool _isDisposed;

    private LoadScope() { }

    public static async UniTask<LoadScope> CreateAsync()
    {
        await LoadUIController.OpenAsync();
        return new LoadScope();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        LoadUIController.Close();
    }
}
public class LoadUIController : MonoSingletonBase<LoadUIController>
{
    private WindowCanvasController _windowCanvasController;
    public override void OnAwake()
    {
        base.OnAwake();
        _windowCanvasController = GetComponent<WindowCanvasController>();

        gameObject.SetActive(false);
    }

    public static async UniTask<Unit> OpenAsync()
    {
        await Instance.UntilOpenAsync();
        return Unit.Default;
    }
    private async UniTask<Unit> UntilOpenAsync()
    {
        gameObject.SetActive(true);
        await UniTask.WaitUntil(() => gameObject.activeSelf);
        await UniTask.WaitForSeconds(0.5f);
        return Unit.Default;
    }
    public static void Close()
    {
        Instance.ThroughCloseAsync().Forget();
    }
    private async UniTask<Unit> ThroughCloseAsync()
    {
        gameObject.SetActive(false);
        await UniTask.WaitUntil(() => !gameObject.activeSelf);
        return Unit.Default;
    }
}
