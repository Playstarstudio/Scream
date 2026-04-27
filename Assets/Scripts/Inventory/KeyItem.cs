using UnityEngine;
using UnityEngine.InputSystem;
using Inventory;
using System;

public class KeyItem : MonoBehaviour
{
    public int itemId;

    private bool inTrigger;

    private IInventory _inventory;

    void Start()
    {
        inTrigger = false;
        _inventory = FindFirstObjectByType<Inventory.Inventory>();
        CharacterMovement chmov = FindFirstObjectByType<CharacterMovement>();
        chmov.InteractPressed += Interacted;
    }


    private void Interacted(object sender, EventArgs e)
    {
        Debug.Log("interacted");
        if (inTrigger)
        {
            _inventory.AddToInventory(itemId);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inTrigger = true; Debug.Log("in trigger");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inTrigger = false; Debug.Log("out trigger");
    }

}
