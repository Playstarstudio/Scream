using Inventory;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Collider2D))]
public class InspectText : MonoBehaviour, IInteractable
{
    private CharacterMovement character;
    private Canvas charCanvas;
    public string textToRead;
    public int InteractionPriority => 9;

    public bool CanInteract => true;

    public void Interact()
    {
        TypewriterScript[] typewriterArray = charCanvas.GetComponentsInChildren<TypewriterScript>();

        foreach (TypewriterScript typewriterScript in typewriterArray)
        {
            typewriterScript.SetText(textToRead);
        }
    }

    private void Start()
    {
        character = FindFirstObjectByType<CharacterMovement>();
        charCanvas = character.GetComponentInChildren<Canvas>();
    }

    void Update()
    {

    }
}
