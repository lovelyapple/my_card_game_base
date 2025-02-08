using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootController : MonoBehaviour
{
    [SerializeField] Camera FieldCameraCache;
    private static CameraRootController _instanceCache;
    private static CameraRootController _instance {
        get
        {
            if(_instanceCache == null)
            {
                _instanceCache = FindObjectOfType<CameraRootController>();
            }

            return _instanceCache;
        }
    }
    public static Camera GetFieldCamera()
    {
        return _instance.FieldCameraCache;
    }
}
