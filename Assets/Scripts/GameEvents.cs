using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }



    //Item Pickup Event
    public event Action onItemInteract;
    public void ItemInteract()
    {
        onItemInteract();
    }


}
