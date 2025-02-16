using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoaderResult<TLoadObject> : IDisposable
{
    private AssetLoadHandler<TLoadObject> _handler;
    private TLoadObject _loadedObject;
    public AssetLoaderResult(string filePah)
    {
        _handler = new AssetLoadHandler<TLoadObject>(filePah);
    }
    public async UniTask<TLoadObject> LoadAsync(CancellationToken token)
    {
        _loadedObject = await _handler.LoadAsycn(token);
        return _loadedObject;
    }
    public void Dispose()
    {
        _handler.Dispose();
    }
}
public class AssetLoadHandler<TObject> : IDisposable
{
    private AsyncOperationHandle<TObject> _asyncOperationHandle;
    private string _key;
    private bool _isReleased;
    public AssetLoadHandler(string key)
    {
        _key = key;
    }
    public async UniTask<TObject> LoadAsycn(CancellationToken token)
    {
        _asyncOperationHandle =  Addressables.LoadAssetAsync<TObject>(_key);

        try
        {
            var obj = await _asyncOperationHandle.ToUniTask(cancellationToken: token);
            return obj;
        }
        catch (OperationCanceledException)
        {
            // キャンセルされたら解放
            Release();
            return default;
        }
    } 
    public void Dispose()
    {
        Release();
    }
    private void Release()
    {
        if (!_isReleased && _asyncOperationHandle.IsValid())
        {
            Addressables.Release(_asyncOperationHandle);
            _isReleased = true;
        }
    }
}
