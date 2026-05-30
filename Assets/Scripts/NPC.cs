using DG.Tweening;
using UnityEngine;

public class NPC : Interactuable
{
    //[SerializeField] private DialogueSO npcDialogue;
    [SerializeField] private Color interactionColor;
    
    protected override void Awake()
    {
        base.Awake(); //Heredo el awake del padre, en este caso, Interactuable
        outline.OutlineColor = interactionColor;
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log("Empieza el diálogo");
    }
    
}