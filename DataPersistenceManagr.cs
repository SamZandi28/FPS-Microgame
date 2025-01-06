using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DataPersistence : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializedDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;

    private FileDataHandler dataHandler;
   public static DataPersistence Instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    private void Awake()
    {
        if (Instance != null)
        {
          Debug.Log ("found more than one Data Persistance Manager in the scene, Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded Called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        //Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null && initializedDataIfNull)
        {
            NewGame();
        }

        if ( this.gameData == null)
        {
            Debug.Log("no data was found, A New game needs to be started before data can be loaded");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        //Debug.Log("Loaded Skill 1" + gameData.IsDefendingShieldUnlocked);
    }

    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found, A New game needs to be started before data can be saved.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //Debug.Log("Loaded Skill 1" + gameData.IsDefendingShieldUnlocked);

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        
        MonoBehaviour[] allBehaviours = FindObjectsOfType<MonoBehaviour>();

        
        List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
        foreach (var behaviour in allBehaviours)
        {
            if (behaviour is IDataPersistence dataPersistence)
            {
                dataPersistenceObjects.Add(dataPersistence);
            }
        }

        return dataPersistenceObjects;
    }

    public bool HasGameData() 
    {
        return gameData != null;
    }

}

