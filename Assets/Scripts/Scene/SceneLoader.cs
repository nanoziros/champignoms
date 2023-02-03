using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int _gameplaySceneIdx;
    [SerializeField]
    private int _mainMenuSceneIdx = 0;
    [SerializeField]
    private GameObject _loadingOverlayObj;

    private int _currentSceneIdx = -1;
    private string _currentSceneName = "";

    public void Initialize()
    {
        Scene s = SceneManager.GetActiveScene();
        _currentSceneIdx = s.buildIndex;
        _currentSceneName = s.name;

        _loadingOverlayObj.SetActive(false);
    }

    private void LoadScene(int idx)
    {
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(idx);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
            return;
        }

        Debug.LogError("Cannot find scene build index " + idx.ToString());
    }


    private IEnumerator LoadSceneRoutine(string name)
    {
        _loadingOverlayObj.SetActive(true);
        yield return StartCoroutine(AssetsCleanRoutine());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        _currentSceneName = name;
        Scene s = SceneManager.GetActiveScene();
        _currentSceneIdx = s.buildIndex;
        _loadingOverlayObj.SetActive(false);
    }

    private IEnumerator AssetsCleanRoutine()
    {
        var unloader = Resources.UnloadUnusedAssets();
        while (!unloader.isDone)
        {
            yield return null;
        }
        //Debug.Log("Cleaning assets routine finished!");
    }

    public void ReloadScene()
    {
        LoadScene(_currentSceneIdx);
    }

    public void LoadMainMenu()
    {
        LoadScene(_mainMenuSceneIdx);
    }

    public void LoadGameplayScene()
    {
        LoadScene(_gameplaySceneIdx);
    }

    public bool CurrentSceneIsGameplay()
    {
        return _mainMenuSceneIdx != _currentSceneIdx;
    }

    public int CurrentSceneIndex()
    {
        return _currentSceneIdx;
    }
}