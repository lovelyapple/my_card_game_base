using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransit : SingletonBase<SceneTransit>
{
    private string _currentScene = null;
    public void RequestGotoScene(string sceneName)
    {
        if(string.IsNullOrEmpty(_currentScene))
        {
            _currentScene = SceneManager.GetActiveScene().name;
        }

        
    }
}
