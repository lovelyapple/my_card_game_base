using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class TitleBackGroundView : MonoBehaviour
{
    [SerializeField] Image BgImage;
    public void Initialize()
    {
        LoadBackGroundImageAsycn(destroyCancellationToken).Forget();
    }
    private async UniTask<Unit> LoadBackGroundImageAsycn(CancellationToken token)
    {
        var texture = await AssetLoader.LoadTextureAsync("Texture.jpg", token);
        var spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        BgImage.sprite = spr;
        return Unit.Default;
    }
}
