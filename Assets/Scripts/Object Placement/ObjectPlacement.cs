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

    public void PlaceObject()
    {
        if (objectToPlace == null) { return; }

        //Instantiate(objectToPlace, mousePos, );
    }
}
