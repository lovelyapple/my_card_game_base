using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AssetLoader : SingletonBase<AssetLoader>
{
    public string ACCESS_KEY = "";
    public string SECRET_KEY = "";
    private const string BucketName = "my-game-backet";
    private readonly RegionEndpoint region = RegionEndpoint.APSoutheast2;
    private AmazonS3Client _amazonS3Client;
    public static async UniTask<Texture2D> LoadTextureAsync(string filePah, CancellationToken token)
    {
        var bytes = await GetInstance().LoadByteAsync(filePah, token);

        if(token.IsCancellationRequested)
        {
            return null;
        }

        var texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        return texture;
    }
    private async UniTask<byte[]> LoadByteAsync(string filePah, CancellationToken token)
    {
        if(_amazonS3Client == null)
        {
            _amazonS3Client = new AmazonS3Client(ACCESS_KEY, SECRET_KEY, region);
        }

        try
        {
            var request = new GetObjectRequest { BucketName = BucketName, Key = filePah };

            using GetObjectResponse response = await _amazonS3Client.GetObjectAsync(request, cancellationToken: token);
            using Stream responseStream = response.ResponseStream;
            using MemoryStream memoryStream = new MemoryStream();

            await responseStream.CopyToAsync(memoryStream);
            byte[] data = memoryStream.ToArray();
            return data;
        }
        catch (AmazonS3Exception e)
        {
            Debug.LogError($"S3 エラー: {e.Message}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"エラー: {e.Message}");
        }

        return null;
    }
}
