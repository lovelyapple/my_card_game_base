using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.SceneManagement;

public class SceneTransit : SingletonBase<SceneTransit>
{
    private string _currentScene = null;
    private const string InternalSceneName = "Internal";
    public static void GotoScene(string sceneName)
    {
        GetInstance().RequestGotoScene(sceneName);
    }
    private void RequestGotoScene(string sceneName)
    {
        if (string.IsNullOrEmpty(_currentScene))
        {
            _currentScene = SceneManager.GetActiveScene().name;
        }

        RequestChangeSceneAsync(sceneName).Forget();
    }
    private async UniTask<Unit> RequestChangeSceneAsync(string sceneName)
    {
        using (await LoadScope.CreateAsync())
        {
            await SceneManager.LoadSceneAsync(InternalSceneName, LoadSceneMode.Single);

            _currentScene = InternalSceneName;

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            _currentScene = sceneName;
        }

        return Unit.Default;
    }
}
