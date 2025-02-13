using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.S3;
using UnityEngine;
using UnityEngine.UIElements;

public class AWSInitailzieClient : MonoSingletonBase<AWSInitailzieClient>
{
    public string ACCESS_KEY = "";
    public string SECRET_KEY = "";
    private AmazonS3Client _amazonS3Client;
    public override void OnAwake()
    {
        _amazonS3Client = new AmazonS3Client(ACCESS_KEY, SECRET_KEY, RegionEndpoint.APSoutheast2);
        base.OnAwake();
    }
}
