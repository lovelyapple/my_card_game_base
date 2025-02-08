using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool SetDonDestroyOnLoad = true;
    private static T _instance;

    // シングルトンインスタンスを取得するためのプロパティ
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // インスタンスがまだ作成されていない場合は、新しいインスタンスを作成する
                _instance = FindObjectOfType<T>(includeInactive: true);
            }
            return _instance;
        }
    }

    // Awakeメソッドでの初期化
    public void Awake()
    {
        _instance = this as T;

        if (SetDonDestroyOnLoad)
        {
            GameObject.DontDestroyOnLoad(_instance.gameObject);
        }

        OnAwake();
    }

    public virtual void OnAwake()
    {

    }
}
