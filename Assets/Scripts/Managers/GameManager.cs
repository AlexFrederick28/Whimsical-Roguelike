using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // have access to save files
    // dont have excess bloat loaded in the main menu
    // load the extra managers and whatnot when entering the game
    // load save files
    public GameObject currentOpenedOverlay;

    public static Action<OverlayPanelBase> SetOverlayPanel;
    
    [SerializeField] private GameObject playerPrefab; // should load the player in 
    [SerializeField] private GameObject playerObject;
    [SerializeField] private CharacterController playerController;
    [SerializeField] public PlayerSettings playerSettings;

    public static GameManager instance;

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

        SaveSystem.Init();
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
            SaveSystem.Save(json);
            Debug.Log(json);
        }
    }

    public void Load(InputAction.CallbackContext context)
    {
        // currently loading by pressing 2, need to change to button on menu eventually

        if (context.performed)
        {
            string saveString = SaveSystem.Load();

            if (saveString != null)
            {
                Debug.Log("Loaded save");
                SaveData saveData = JsonUtility.FromJson<SaveData>(saveString);

                // player
                playerController.Move(saveData.playerPosition - playerController.transform.position); // controller has full control over player position (cannot just use player.transform.position)
                Debug.Log(saveData.playerPosition);

                // camera (still need to setup a way to access the player camera)

            }
            else
            {
                Debug.LogError("No save file");
            }
        }
    }

    public void OnPressClosePanel(InputAction.CallbackContext context)
    {
        if (currentOpenedOverlay == null) { return; }
        else if (currentOpenedOverlay != null)
        {
            if (currentOpenedOverlay.GetComponentInParent<OverlayPanelBase>().isEscMenu) { return; } // ensures the esc menu doesnt close as soon as it opens
        }

        if (context.performed)
        {
            Debug.Log("Closing panels");
            SetOverlayPanel?.Invoke(null);
        }
    }

    public void SetCamerSwivelSpeed(float value)
    {
        playerSettings.cameraRotationSpeed = value;
    }

    public void SetCameraZoomSpeed(float value)
    {
        playerSettings.cameraZoomSpeed = value;
    }
}
