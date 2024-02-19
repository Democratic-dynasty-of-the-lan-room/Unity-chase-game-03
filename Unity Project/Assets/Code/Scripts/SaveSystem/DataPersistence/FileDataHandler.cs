using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    // Save Data Restartss
    private string restartDataDirPath = "";
    private string restartDataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "TheHorror";

    private bool RestartuseEncryption = false;

    // are these two needed again?
    //private bool RestartuseEncryption = false;
    //private readonly string RestartencryptionCodeWord = "TheHorror";

    public FileDataHandler(string dataDirPath, string dataFileName, string restartDataDirPath, string restartDataFileName,  bool useEncryption, bool RestartuseEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        
        this.restartDataDirPath = restartDataDirPath;
        this.restartDataFileName = restartDataFileName;

        this.useEncryption = useEncryption;
        this.RestartuseEncryption = RestartuseEncryption;
    }



    public GameData Load()
    {
        // use path.Combine to account for OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // optionally encrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file:" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        // use path.Combine to account for OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file:" + fullPath + "\n" + e);
        }
    }

    // Restart Data
    public CheckPointData RestartLoad()
    {
        // use path.Combine to account for OS's having different path separators
        string RestartfullPath = Path.Combine(restartDataDirPath, restartDataFileName);
        CheckPointData restartLoadedData = null;
        if (File.Exists(RestartfullPath))
        {
            try
            {
                // Load the serialized data from the file
                string restartDataToLoad = "";
                using (FileStream restartStream = new FileStream(RestartfullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(restartStream))
                    {
                        restartDataToLoad = reader.ReadToEnd();
                    }
                }

                // optionally encrypt the data
                if (useEncryption)
                {
                    restartDataToLoad = EncryptDecrypt(restartDataToLoad);
                }

                // deserialize the data from Json back into the C# object
                restartLoadedData = JsonUtility.FromJson<CheckPointData>(restartDataToLoad);
            }
            catch (Exception b)
            {
                Debug.LogError("Restart Error occured when trying to load data from file:" + RestartfullPath + "\n" + b);
            }
        }
        return restartLoadedData;
    }

    public void RestartSave(CheckPointData RestartData)
    {
        // use path.Combine to account for OS's having different path separators
        string RestartfullPath = Path.Combine(restartDataDirPath, restartDataFileName);
        try
        {
            // create directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(RestartfullPath));
            Debug.Log($"Saving data to {RestartfullPath}");

            // serialize the C# game data object into Json
            string RestartdataToStore = JsonUtility.ToJson(RestartData, true);

            // optionally encrypt the data
            if (RestartuseEncryption)
            {
                RestartdataToStore = EncryptDecrypt(RestartdataToStore);
            }

            // write the serialized data to the file
            using (FileStream restartStream = new FileStream(RestartfullPath, FileMode.Create))
            {
                using (StreamWriter restartWriter = new StreamWriter(restartStream))
                {
                    restartWriter.Write(RestartdataToStore);
                }
            }
        }
        catch (Exception b)
        {
            Debug.LogError("Error occured when trying to save data to file:" + RestartfullPath + "\n" + b);
        }
    }

    // the below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
