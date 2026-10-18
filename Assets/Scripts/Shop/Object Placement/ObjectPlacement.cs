using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    // this class as well as others should turn off when not needed? 

    private Vector3 mousePosition;
    public GameObject objectToPlace; // this would be selected from UI

    // need to show a ghost of the object the player wants to place, so they can see how it fits in the location and rotate it

    private void Update()
    {
        //if (GameManager.instance.stateMachine.currentState != StateMachine.GameStates.Decorating) { return; }

        Vector3 mousePos = PlayerMouseCursor.GetMousePosition3D();
    }

    // should start to show when the object has been selected
    public void ShowObjectGhost()
    {
        if (objectToPlace == null) { return; }
        // show the ghost of the object the player is about to place
        objectToPlace.GetComponent<ShopObject>().DisplayTransparent();
    }

    // should stop showing when another object has been selected, or when nothing is selected
    public void DisableObjectGhost()
    {
        if (objectToPlace == null) { return; }
        objectToPlace.GetComponent<ShopObject>().DisplayLit();
    }

    // using Q and E
    public void RotateObject()
    {
        if (objectToPlace == null) { return; }
    }

    // Left click
    public void PlaceObject()
    {
        if (GameManager.instance.stateMachine.currentState != StateMachine.GameStates.Decorating) { return; } 
        if (objectToPlace == null) { return; }

        //Instantiate(objectToPlace, mousePos, );
    }
}
