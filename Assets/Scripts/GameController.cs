using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string Name;
    public GameObject GamePrefab;
    public Sprite MenuImage;
    public Vector2 TimeBonusCellPosition;
    public AudioClip Music;
}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private SceneLoader _sceneLoader;
    [SerializeField]
    private SoundController _soundController;
    [Header("Audio")]
    [SerializeField]
    private AudioListener _audioListener;
    [SerializeField]
    private AudioClip _menuMusic;

    [SerializeField]
    private List<LevelData> _levelData;

    public static GameController Instance { get; private set; }
    public SceneLoader SceneLoader { get { return _sceneLoader; } }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (gameObject != objs[i])
                {
                    Destroy(objs[i]);
                    break;
                }
            }
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _sceneLoader.Initialize();
    }

    private List<string> GetAllLevelNames()
    {
        var list = new List<string>();
        foreach (var entry in _levelData)
        {
            list.Add(entry.Name);
        }
        return list;
    }

    public List<Sprite> GetMenuLevelSprites()
    {
        var list = new List<Sprite>();
        foreach (var entry in _levelData)
        {
            list.Add(entry.MenuImage);
        }
        return list;
    }

    public string GetLevelNameByIdx(int idx)
    {
        if (idx >= 0 && idx < _levelData.Count)
        {
            return _levelData[idx].Name;
        }
        Debug.LogError("Wrong index for level name!");
        return "???";
    }

    public void LoadGameplayScenario()
    {
        _soundController.Stop();
        _sceneLoader.LoadGameplayScene();
    }

    public void LoadMainMenu()
    {
        _soundController.Play(_menuMusic);
        _sceneLoader.LoadMainMenu();
    }

    public void DeleteData()
    {
        _sceneLoader.ReloadScene();
    }

    public void ToggleMusicEnabled()
    {
        _soundController.IsEnabled = !_soundController.IsEnabled;
    }
}
