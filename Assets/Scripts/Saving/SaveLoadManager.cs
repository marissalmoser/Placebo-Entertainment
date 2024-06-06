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
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion
    public event Action OnSaveGame;
    public event Action<SaveData> OnLoadData;

    private const string directory = "/SavedGame/";
    private const string fileName = "SaveGame.sav";

    private SaveData newData = new SaveData();
    public InventoryItemData itemData;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            Save(newData);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Load();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHolder>().InventorySystem.AddToInventory(itemData, 3);
        }
    }

    public bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            //creating a file at this location if it doesnt exist already. If it
            //does, we will overwrite it
            data.CollectInventoryData();
            string jsonString = JsonUtility.ToJson(data, true);
            //prettyPrint is nice; organizes the file
            File.WriteAllText(dir + fileName, jsonString);
        }
        return true;
    }
    /// <summary>
    /// Using json utility to reconstruct our savegame from the file 
    /// </summary>
    /// <returns></returns>
    public SaveData Load()
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
