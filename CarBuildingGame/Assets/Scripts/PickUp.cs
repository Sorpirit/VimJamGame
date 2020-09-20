using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("inventory").GetComponent<Inventory>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventory.PickedUp(gameObject);
            Destroy(gameObject);
        }
    }
}
