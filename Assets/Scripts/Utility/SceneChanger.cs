using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : SingleMonoBehaviour<SceneChanger>
{
    protected override void OnAwake()
    {
        DontDestroyObject();
    }
    public void LoadScene(string sceneName)
    {
        if (sceneName.Length <= 0) { Debug.LogWarning("シーンを指定してください"); return; }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (sceneName.Length <= 0) { Debug.LogWarning("シーンを指定してください"); return; }
        StartCoroutine(LoadAsyncImpl(sceneName));
    }
    private IEnumerator LoadAsyncImpl(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (true)
        {
            yield return null;
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);
                break;
            }
        }
    }
    public void FadeScene(string sceneName)
    {
        FadeSystem.FadeOut(() => LoadScene(sceneName));
    }

    public void FadeScene(string sceneName, float fadeTime = 1.0f)
    {
        FadeSystem.FadeOut(fadeTime, () => LoadScene(sceneName));
    }
}
