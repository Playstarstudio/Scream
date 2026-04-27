using UnityEngine;
using UnityEngine.EventSystems;


public class BackpackInteractScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HIT");
        animator.SetBool("MouseHoverTrigger", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("MouseHoverTrigger", false);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("MouseHoverTrigger", true);

    }

}
