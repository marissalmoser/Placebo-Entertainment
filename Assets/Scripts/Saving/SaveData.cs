/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 6/3/24
*    Description: I can probably make this a struct? All its doing 
*    is making a SerializeableDictionary
*******************************************************************/
[System.Serializable]
public class SaveData
{
    public SerializeableDictionary<string, InventorySystem> inventoryDictionary;
    public SaveData()
    {
        inventoryDictionary = new SerializeableDictionary<string, InventorySystem>();
    }
}
