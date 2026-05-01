using UnityEngine;
using UnityEngine.EventSystems;


public class BackpackInteractScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator animator;
    
    private AudioManager _audio;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HIT");
        animator.SetBool("MouseHoverTrigger", true);
        _audio?.PlayOneShot(AudioID.SFX.Interface.Inventory.hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("MouseHoverTrigger", false);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("MouseHoverTrigger", true);
        _audio = AudioManager.Instance;

    }

}
