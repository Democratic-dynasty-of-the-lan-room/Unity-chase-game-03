using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using Code.Scripts.SampleScene.MenuScripts;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("FileStorageConfig")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;

    // Check Point
    [SerializeField] private string RestartfileName;
    [SerializeField] private bool RestartuseEncryption;

    private CheckPointData checkPointData;

    private List<IDataPersistence> RestartdataPersistenceObjects;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, Application.persistentDataPath, RestartfileName, useEncryption, RestartuseEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    //Remove Later as well I think
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        this.RestartdataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();

        this.checkPointData = new CheckPointData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before the data can be loaded.");
            return;
        }
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // if we don't have any data, Log a worning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private List<IDataPersistence> RestartFindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> RestartdataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(RestartdataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public bool RestartHasGameData()
    {
        return checkPointData != null;
    }

    //Restart Load Game
    public void RestartLoadGame()
    {
        // Load any saved Restartdata from a file using the data handler
        this.checkPointData = dataHandler.RestartLoad();

        // if no data can be loaded, don't continue
        if (this.checkPointData == null)
        {
            Debug.Log("You havent Gotten to a check point");
            return;
        }
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence RestartdataPersistenceObj in RestartdataPersistenceObjects)
        {
            RestartdataPersistenceObj.RestartLoadData(checkPointData);
        }
    }

    public void RestartSaveGame()
    {
        // if we don't have any data, Log a worning here
        if (this.checkPointData == null)
        {
            Debug.LogWarning("No data was found. When you went into a spawn point");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence RestartdataPersistenceObj in RestartdataPersistenceObjects)
        {
            RestartdataPersistenceObj.RestartSaveData(ref checkPointData);
        }

        // save that data to a file using the data handler
        dataHandler.RestartSave(checkPointData);
    }
}
