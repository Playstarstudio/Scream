using UnityEngine;
using UnityEngine.UI;

public class GestureOpener : MonoBehaviour, IInteractable
{
    public int InteractionPriority => 3;
    public bool CanInteract => true;

    public Image GesturePanel;
    public bool GestureActivate;
    public bool matches;
    public GameObject matchObject;
    public bool candle;
    public GameObject candleObject;

    public void Interact()
    {
        if (GesturePanel != null)
        {
            if (GesturePanel.gameObject.activeInHierarchy == false)
            {
                GesturePanel.gameObject.SetActive(true);
            }
            else
                GesturePanel.enabled = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        PanelCheck();

    }
    private void CandleCheck()
    {
        if (candleObject != null)
        {
            if (candle && !candleObject.activeInHierarchy)
            {
                candleObject.SetActive(true);
            }
            if (!candle && candleObject.activeInHierarchy)
            {
                candleObject.SetActive(false);
            }
        }
    }
    private void MatchCheck()
    {
        if (matchObject != null)
        {
            if (matches && !matchObject.activeInHierarchy)
            {
                matchObject.SetActive(true);
            }
            if (!matches && matchObject.activeInHierarchy)
            {
                matchObject.SetActive(false);
            }
        }
    }
    private void PanelCheck()
    {
        if (GestureActivate && GesturePanel != null)
        {
            if (GesturePanel.gameObject.activeInHierarchy == false)
            {
                GesturePanel.gameObject.SetActive(true);
            }
        }
        if (!GestureActivate && GesturePanel != null)
        {
            if (GesturePanel.gameObject.activeInHierarchy == true)
            {
                GesturePanel.gameObject.SetActive(false);
            }
        }
    }

}
