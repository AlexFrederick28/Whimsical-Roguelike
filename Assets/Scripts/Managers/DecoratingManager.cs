using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecoratingManager : MonoBehaviour
{
    public static Action<GameObject> onSelectedDecoration;
    public static Action<GameObject> onDeSelectedDecoration;
    public static Action<GameObject> onPlacedDecoration;
    public static Action<GameObject> onRemovedDecoration;

    public List<GameObject> placedDecorationsList = new List<GameObject>();

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
    }

    private void DeSelectDecoration(GameObject decoration)
    {
        //selectedDecoration = null;
    }

    private void Update()
    {
        if (GameManager.instance.stateMachine.currentState != StateMachine.GameStates.Decorating)
        {
            //ensures that the selected object goes away if the player is no longer decorating
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
        onPlacedDecoration?.Invoke(selectedDecoration);
        selectedDecoration = null;
    }
}
