using UnityEngine;
using System.IO;
using Droidzone.Core;


[System.Serializable]
public class PlayerData
{

    //PC
    public string playerName;
    public string race;
    public string classe;

    public float Health;
    public float Energy;
    public float Resistance;
    public int Level;
    public int XP;
    //Skills
    public float MeleeDamage = 1;
    public float MeleeBonus = 0;
    public float RangedBonus = 0;
    public float RangedDamage = 1;
    public float Hacking = 2;
    public float Healing = 1;
    public float Leadership = 1;
    public float GenEngineering = 1;
    public float Piloting = 1;
    public float Tracking = 1;
    public float Taming = 1;
    public float HackLock = 1;

    public string CurrentLocation;
    public string PreviousLocation;

    public int[] attributes;

    public int itemsCount;
    public string[] itemNames;
    public int[] itemsAmount;

    public string[] feats;

    
}

public class SaveLoadManager : MonoBehaviour
{
    public PlayerData playerData;

    private string saveFilePath;

    GameManager gameManager;

    public bool isCreation;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        // Set the save file path based on the application's persistent data path
        saveFilePath = Path.Combine(Application.persistentDataPath, "C:/droidzone/playerdata.json");
    }

    private void Start()
    {
      
        

        if(isCreation == false)
        {
            LoadPlayerData();
            print("Loaded race: " + playerData.race);
            GameManager.playerData = playerData;
            gameManager.Initialize();
        }
       

    }

    public bool SavePlayerData()
    {
        // Serialize the player data to JSON format
        string jsonData = JsonUtility.ToJson(playerData);

        // Save the JSON data to the file
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("Player data saved to: " + saveFilePath);

        return true;
    }

    public void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            // Load the JSON data from the file
            string jsonData = File.ReadAllText(saveFilePath);

            // Deserialize the JSON data back to player data object
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            Debug.Log("Player data loaded from: " + saveFilePath);
        }
        else
        {
            // If the save file doesn't exist, create a new player data object
            playerData = new PlayerData();

            Debug.Log("No save file found. New player data created.");
        }
    }
    public void DeletePlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            // Delete the save file
            File.Delete(saveFilePath);

            Debug.Log("Player data deleted: " + saveFilePath);
        }
        else
        {
            Debug.Log("No save file found. Nothing to delete.");
        }
    }
    public bool IsSaveFileExists()
    {
        return File.Exists(saveFilePath);
    }

}
