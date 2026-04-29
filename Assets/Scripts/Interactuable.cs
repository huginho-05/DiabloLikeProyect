using System;
using UnityEngine;
using UnityEngine.EventSystems;

//Todo interactuable va a llevar consigo un outline
[RequireComponent(typeof(Outline))]
public abstract class Interactuable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public float InteractionDistance { get; private set; }
    [SerializeField] private Texture2D interactionIcon;
    
    protected Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public abstract void Interact(GameObject interactor);

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Detecto ratón");
        outline.enabled = true;
        Cursor.SetCursor(interactionIcon, new Vector2(0.5f, 0.5f), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("No detecto ratón");
        outline.enabled = false;
        Cursor.SetCursor(null, new Vector2(0.5f, 0.5f), CursorMode.Auto);
    }
}
