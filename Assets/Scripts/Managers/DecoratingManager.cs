using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecoratingManager : MonoBehaviour
{
    public static Action<GameObject> onSelectedDecoration;
    public static Action onDeSelectedDecoration;
    public static Action<GameObject> onPlacedDecoration;
    public static Action<GameObject> onRemovedDecoration; // cannot remove a decoration yet

    public List<GameObject> placedDecorationsList = new List<GameObject>(); // unused

    public GameObject selectedDecoration;

    [SerializeField] private LayerMask groundCheckLayer;

    private void OnEnable()
    {
        GameManager.globalInputActions.Decoration.Place.performed += PlaceDecoration;

        onSelectedDecoration += SelectedDecoration;
        onDeSelectedDecoration += DeSelectDecoration;
    }

    private void OnDisable()
    {
        GameManager.globalInputActions.Decoration.Place.performed -= PlaceDecoration;

        onSelectedDecoration -= SelectedDecoration;
        onDeSelectedDecoration -= DeSelectDecoration;
    }

    private void SelectedDecoration(GameObject decoration)
    {
        selectedDecoration = decoration;
        

        ShopObject obj = decoration.GetComponent<ShopObject>();
        obj.DisplayTransparent();
    }

    private void DeSelectDecoration()
    {
        if (selectedDecoration != null)
        {
            ShopObject obj = selectedDecoration.GetComponent<ShopObject>();
            obj.DisplayLit();

            selectedDecoration = null;
        }
    }

    private void Update()
    {
        if (GameManager.instance.stateMachine.currentState != StateMachine.GameStates.Decorating)
        {
            //ensures that the selected object goes away if the player is no longer decorating - not in use due to decorating state not currently working from the state machine
            //if (spawnedObject != null)
            //{
            //    Destroy(spawnedObject);
            //    spawnedObject = null;
            //}
            //return;
        }

        if (selectedDecoration != null)
        {
            selectedDecoration.transform.position = PlayerMouseCursor.GetMousePosition3D(groundCheckLayer);
        }
    }

    private void PlaceDecoration(InputAction.CallbackContext context)
    {
        onPlacedDecoration?.Invoke(selectedDecoration); // currently unused
        DeSelectDecoration();
    }
}
