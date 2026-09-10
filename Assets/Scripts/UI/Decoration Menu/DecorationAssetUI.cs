using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecorationAssetUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject spawnedObject;
    [SerializeField] private LayerMask groundCheckLayer;
    [SerializeField] private bool highlited = false;

    // need a way to know if the player has chosen a different decoration, has exited decorate mode, or has unchosen this decoration

    private void OnEnable()
    {
        DecoratingManager.onPlacedDecoration += PlacedDecoration;
    }

    private void OnDisable()
    {
        DecoratingManager.onPlacedDecoration -= PlacedDecoration;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            DecoratingManager.onDeSelectedDecoration?.Invoke(spawnedObject);
            Destroy(spawnedObject);
            spawnedObject = null;
        }
        else if (spawnedObject == null)
        {
            Vector3 mousePos = PlayerMouseCursor.GetMousePosition3D(groundCheckLayer);
            spawnedObject = Instantiate(objectToSpawn, mousePos, Quaternion.identity);
            DecoratingManager.onSelectedDecoration?.Invoke(spawnedObject);
            Debug.Log("Spawned " + spawnedObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // highlight
        highlited = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // un-highlight
        highlited = false;
    }

    public void PlacedDecoration(GameObject decoration)
    {
        if (spawnedObject != null && highlited == false)
        {
            spawnedObject = null;
        }
    }
}
