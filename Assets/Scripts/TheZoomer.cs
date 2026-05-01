using UnityEngine;

public class TheZoomer : MonoBehaviour, IInteractable
{

    [SerializeField] public Sprite hiResSprite;
    public int InteractionPriority => 9;

    public bool CanInteract => true;

    public void Interact()
    {
        NarrativeZoomScript zoomScript = FindFirstObjectByType<NarrativeZoomScript>();
        zoomScript.OpenZoomCanvas(hiResSprite);
    }

}
