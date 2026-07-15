using System;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // have access to save files
    // dont have excess bloat loaded in the main menu
    // load the extra managers and whatnot when entering the game
    // load save files

    // the registered on top UI overlay
    public GameObject currentOpenedOverlay;

    // opens/closes overlay panels
    public static Action<OverlayPanelBase> SetOverlayPanel;
    public static Action OnInitialisePlayer;

    // player references
    [SerializeField] private Transform playerDefaultSpawnPosition;
    [SerializeField] private GameObject playerPrefab; // should load the player in 
    [SerializeField] private GameObject playerObject;
    [SerializeField] private SwivelCamera playerCamera;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CharacterController playerController;

    // all input actions
    public static InputSystem_Actions globalInputActions { get; private set; }

    // settings regarding the player 
    public PlayerSettings playerSettings;

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

        globalInputActions = new InputSystem_Actions();
        InitialisePlayerCharacter(); // this is here temporarily

        SaveSystem.Init();
    }

    private void OnEnable()
    {
        // save and load
        globalInputActions.Player.Save.performed += Save;
        globalInputActions.Player.Load.performed += Load;

        globalInputActions.Enable();
    }

    private void OnDisable()
    {
        // save and load
        globalInputActions.Player.Save.performed -= Save;
        globalInputActions.Player.Load.performed -= Load;

        globalInputActions.Disable();
    }

    private void InitialisePlayerCharacter()
    {
        // should initialise after entering the game scene from the main menu (or once a cinematic is over)
        if (playerObject == null)
        {
            // will need a spawn position in future
            playerObject = Instantiate(playerPrefab, playerDefaultSpawnPosition.position, Quaternion.identity);
            playerCamera = playerObject.GetComponentInChildren<SwivelCamera>();
            playerController = playerObject.GetComponent<CharacterController>();
            playerMovement = playerObject.GetComponent<PlayerMovement>();
        }
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

            };

            string json = JsonUtility.ToJson(saveData);
            SaveSystem.Save(json);
            Debug.Log(json);
        }
    }

    /// <summary>
    /// Loads player game save data
    /// </summary>
    /// <param name="context"></param>
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
            if (currentOpenedOverlay.GetComponentInParent<OverlayPanelBase>().isEscMenu && currentOpenedOverlay.activeInHierarchy) { return; } // ensures the esc menu doesnt close as soon as it opens
        }

        if (context.performed)
        {
            Debug.Log("Closing panels");
            SetOverlayPanel?.Invoke(null);
        }
    }

    /// <summary>
    /// Applies player settings when in the settings menu
    /// </summary>
    public void ApplyPlayerSettings()
    {
        // need another save file for player settings (so that they dont save alongside a game save)
        // these settings will only be saved once pressing the save button, needs to happen when hitting apply
        // player camera
        string json = JsonUtility.ToJson(playerSettings);
        SaveSystem.SaveSettings(json);
        Debug.Log("Saved applied settings");
        playerCamera.rotationSpeed = playerSettings.cameraRotationSpeed;
        playerCamera.zoomSpeed = playerSettings.cameraZoomSpeed;
    }

    /// <summary>
    /// Used on game start/open (applies player settings from last played)
    /// </summary>
    public void LoadPLayerSettings()
    {
        string saveString = SaveSystem.LoadSettings();

        if (saveString != null)
        {
            Debug.Log("Loaded save");
            PlayerSettings saveData = JsonUtility.FromJson<PlayerSettings>(saveString);

            // camera 
            playerCamera.rotationSpeed = saveData.cameraRotationSpeed;
            playerCamera.zoomSpeed = saveData.cameraZoomSpeed;
        }
        else
        {
            Debug.LogError("No save file");
        }
    }

    public void SetCamerSwivelSpeed(float value)
    {
        playerSettings.cameraRotationSpeed = (value * 100.0f) * 2;
    }

    public void SetCameraZoomSpeed(float value)
    {
        playerSettings.cameraZoomSpeed = (value * 100.0f) * 2;
    }
}
