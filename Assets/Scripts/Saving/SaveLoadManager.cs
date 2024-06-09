/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 6/2/24
*    Description: This manager allows saving and loading, assigning 
*    savedata, deleting savedata,
*******************************************************************/
using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region Instance
    //regions are cool, i guess. Just hiding boring stuff
    public static SaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
    public event Action OnSaveGame;
    public event Action<SaveData> OnLoadData;

    private const string directory = "/SavedGame/";
    private const string fileName = "SaveGame.sav";

    private SaveData newData = new SaveData();

    private void CollectInventoryData()
    {
        InventoryHolder[] inventoryHolders = FindObjectsOfType<InventoryHolder>();
        foreach (var holder in inventoryHolders)
        {
            string objectName = holder.gameObject.name;
            InventorySystem inventorySystem = holder.InventorySystem;
            if (!newData.inventoryDictionary.ContainsKey(objectName))
            {
                newData.inventoryDictionary.Add(objectName, inventorySystem);
            }
        }
    }

    public bool SaveGameToSaveFile()
    {
        OnSaveGame?.Invoke();
        string dir = Application.persistentDataPath + directory;
        //creating a file at this location if it doesnt exist already. If it
        //does, we will overwrite it    
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);      
        }

        CollectInventoryData();
        string jsonString = JsonUtility.ToJson(newData, true);
        //prettyPrint is nice; organizes the file
        File.WriteAllText(dir + fileName, jsonString);
        GUIUtility.systemCopyBuffer = dir;
        return true;
    }
    /// <summary>
    /// Using json utility to reconstruct our savegame from the file 
    /// </summary>
    /// <returns></returns>
    public SaveData LoadGameFromSaveFile()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData temp = new SaveData();

        if (File.Exists(fullPath))//if a file exists at this path
        {
            string jsonString = File.ReadAllText(fullPath);
            temp = JsonUtility.FromJson<SaveData>(jsonString);
            AssignLoadedData(temp);
            OnLoadData?.Invoke(temp);
        }
        else
        {
            print("Dun goofed, save file doesnt exist at given location");
        }
        return temp;
    }

    public void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))//if a file exists at this path
        {
            File.Delete(fullPath);
        }
    }

    private void AssignLoadedData(SaveData data)
    {
        foreach (var entry in data.inventoryDictionary)
        {
            string objectName = entry.Key;
            InventorySystem savedInventory = entry.Value;

            GameObject obj = GameObject.Find(objectName);
            if (obj != null)
            {
                InventoryHolder holder = obj.GetComponent<InventoryHolder>();
                if (holder != null)
                {
                    holder.SetInventorySystem(savedInventory);
                }
                else
                {
                    print("No InventoryHolder component found on " + objectName);
                }
            }
            else
            {
                print("No GameObject named " + objectName + " found in the scene");
            }
        }
    }
}
