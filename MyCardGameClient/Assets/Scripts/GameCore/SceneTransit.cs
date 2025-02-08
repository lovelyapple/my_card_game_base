using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransit : SingletonBase<SceneTransit>
{
    private string _currentScene = null;
    private void RequestGotoScene(string sceneName)
    {
        if (string.IsNullOrEmpty(_currentScene))
        {
            _currentScene = SceneManager.GetActiveScene().name;
        }

        GameMainObject.Instance.StartCoroutine(RequestChangeSceneAsync(sceneName));
    }
    private IEnumerator RequestChangeSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _currentScene = sceneName;
    }
    public static void GotoScene(string sceneName)
    {
        GetInstance().RequestGotoScene(sceneName);
    }
}
