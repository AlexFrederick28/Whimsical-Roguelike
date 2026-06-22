using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // have access to save files
    // dont have excess bloat loaded in the main menu
    // load the extra managers and whatnot when entering the game
    // load save files

    [SerializeField] private string saveDirectory = "/Save/save.txt";
    [SerializeField] private GameObject playerPrefab; // should load the player in 
    [SerializeField] private GameObject playerObject;
    [SerializeField] private PlayerSettings playerSettings;

    static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);

        SaveData saveData = new SaveData
        {
            cameraRotationSpeed = 50.0f,
            cameraZoomSpeed = 50.0f
        };

        string json = JsonUtility.ToJson(saveData);
        Debug.Log(json);

        SaveData loadedGameSave = JsonUtility.FromJson<SaveData>(json);
        Debug.Log(loadedGameSave.cameraRotationSpeed);
    }

    public void Save(InputAction.CallbackContext context)
    {
        // currently saving by pressing 1, need to change to button on menu eventually

        if (context.performed)
        {
            SaveData saveData = new SaveData()
            {
                // player
                playerPosition = playerObject.transform.position,

                // camera
                cameraRotationSpeed = playerSettings.cameraRotationSpeed,
                cameraZoomSpeed = playerSettings.cameraZoomSpeed
            };

            string json = JsonUtility.ToJson(saveData);
            Debug.Log(json);

            File.WriteAllText(Application.dataPath + saveDirectory, json);
        }
    }

    public void Load(InputAction.CallbackContext context)
    {
        // currently loading by pressing 2, need to change to button on menu eventually

        if (context.performed)
        {
            if (File.Exists(Application.dataPath + saveDirectory))
            {
                string saveString = File.ReadAllText(Application.dataPath + saveDirectory);
                SaveData saveData = JsonUtility.FromJson<SaveData>(saveString);

                // player
                playerObject.transform.position = saveData.playerPosition;

                // camera (still need to setup a way to access the player camera)
                
            }
            else
            {
                Debug.LogError("No save file");
            }
        }
    }
}
