using UnityEngine;
using UnityEngine.InputSystem;

public class OverlayPanelBase : MonoBehaviour, IOverlayPanel
{
    public bool isEscMenu = false;
    [SerializeField] private GameObject panelObject;

    private void OnEnable()
    {
        GameManager.SetOverlayPanel += OpenOrClosePanelEvent;
        if (isEscMenu)
        {
            GameManager.globalInputActions.Menus.PauseMenu.performed += OnPressOpenOrClosePanel;
        }
        GameManager.globalInputActions.UI.Escape.performed += GameManager.instance.OnPressClosePanel;
    }

    private void OnDisable()
    {
        GameManager.SetOverlayPanel -= OpenOrClosePanelEvent;

        if (isEscMenu)
        {
            GameManager.globalInputActions.Menus.PauseMenu.performed -= OnPressOpenOrClosePanel;
        }
        GameManager.globalInputActions.UI.Escape.performed -= GameManager.instance.OnPressClosePanel;
    }


    // open panel (settings) pressing esc
    // close panel (settings) pressing esc
    // eg. open panel (inventory) pressing I
    // close panel (inventory) pressing I or Esc
    // atm we can only open and close the inventory pressing I

    public void OnPressOpenOrClosePanel(InputAction.CallbackContext context)
    {
        // open and close using the same button
        // TODO: need a way to use Esc to close all panels
        if (context.performed)
        {
            Debug.Log("Panel invoked: " + panelObject.name);
            GameManager.SetOverlayPanel?.Invoke(this);
        }
    }

    public void OpenOrClosePanelEvent(OverlayPanelBase panel)
    {
        if (panel == null)
        {
            // if recieved null, close panels
            Debug.Log("Panel closed: " + panelObject.name);
            panelObject.SetActive(false);
        }
        else if (panel == this && GameManager.instance.currentOpenedOverlay != panelObject)
        {
            // if panel is set to this object, open it
            Debug.Log("Panel open");
            GameManager.instance.currentOpenedOverlay = panelObject;
            panelObject.SetActive(true);
        }
        else
        {
            // if panel is not this object, close it
            Debug.Log("Panel closed: " + panelObject.name);
            panelObject.SetActive(false);
            GameManager.instance.currentOpenedOverlay = null;
        }
    }

    public void ClosePanelByButton()
    {
        GameManager.instance.currentOpenedOverlay = null;
    }
}
