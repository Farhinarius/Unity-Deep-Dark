using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    float horizontal, vertical;
    Vector2 lookDirection;
    int PressedKey;
    public Vector2 lengthRay = new Vector2(1.5f, 1.5f);

    private bool previousEquipedExists = false;
    private GameObject previousEquiped;

    private GameObject inventoryChild;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        // initialize item
        previousEquiped = new GameObject();

        inventoryChild = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per 
    void Update()
    {
        GetInput();
        SetLookDirection();
        // Interact();
        TryToPickUpItem();

        Debug.DrawRay(rigidbody2d.position, lookDirection * lengthRay, Color.blue);
    }
    
    void FixedUpdate()
    {
        MovementControl();
    }   

    private void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void MovementControl()
    {        
        Vector2 nextPos = rigidbody2d.position;
        nextPos.x += speed * horizontal * Time.deltaTime;
        nextPos.y += speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(nextPos);
    }

    private void SetLookDirection()
    {
        if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
        {
            lookDirection.Set(horizontal, vertical);
            lookDirection.Normalize();
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, lookDirection, lengthRay.x, LayerMask.GetMask("Default"));
            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }
        }
    }

    private void TryToPickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, lookDirection, lengthRay.x, LayerMask.GetMask("Default"));
            if (hit.collider.gameObject != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                PickUp(hit.collider.gameObject);
            }
        }

    }

    private void PickUp(GameObject itemObjHit)
    {
        var itemToAdd = Database.instance.SearchItemBy(itemObjHit);
        if (itemToAdd != null)
        {
            InventoryManager.instance.TryToAdd(itemToAdd);
            EquipItem(itemObjHit, this.gameObject);
        }
        else
        {
            throw new System.Exception("GameObject doesn't match any item");
        }
    }

    public void EquipItem(GameObject item, GameObject player)
    {
        GameObject rightHand = player.transform.GetChild(0).gameObject;
        if (previousEquipedExists)
        {
            previousEquiped.SetActive(false);
            
            item.transform.SetParent(rightHand.transform, false);
            item.transform.position = rightHand.transform.position;
            item.layer = 9;

            previousEquiped = item;
        }
        else 
        {
            item.transform.SetParent(rightHand.transform, false);
            item.transform.position = rightHand.transform.position;
            item.layer = 9;

            previousEquipedExists = true;
            previousEquiped = item;     // initiating previous equiped
        }
    }

    public void UnequipItem(int index)
    {
        var hand = gameObject.transform.GetChild(0).gameObject;
        var objToDrop = hand.transform.GetChild(index).gameObject;
        
        // dropitem
        objToDrop.transform.SetParent(null, false);
        objToDrop.transform.position = hand.transform.position;
        objToDrop.layer = LayerMask.NameToLayer("Default");

        // previous selection doesn't 
        previousEquipedExists = false;
        previousEquiped = null;

    }

    // condition, when you drop item and equipt another one
    public void EquipNew(int newEquipedIndex)
    {
        var hand = gameObject.transform.GetChild(0).gameObject;
        var newEquiped = hand.transform.GetChild(newEquipedIndex).gameObject;

        newEquiped.SetActive(true);

        previousEquipedExists = true;
        previousEquiped = newEquiped;
    }

    public void ChangeItem(int index)
    {
        GameObject rightHand = gameObject.transform.GetChild(0).gameObject;
        
        Debug.Log($"Previous item exists: {previousEquipedExists}");
        var EquipedItem = rightHand.transform.GetChild(index).gameObject;

        if (previousEquipedExists && previousEquiped != EquipedItem)
        {
            previousEquiped.SetActive(false);
            EquipedItem.SetActive(true);
            previousEquiped = EquipedItem;
        }
        else
        {
            EquipedItem.SetActive(true);
            Debug.Log("Item Changing");
        }
    }
}



// realisation with direct child
/*  item.transform.SetParent(player.transform, false);
Vector2 pos = (Vector2)player.transform.position + new Vector2(0.6f, 0);
item.transform.position = pos; */

/*         for (int i = hand.transform.childCount - 1; i >= 0; i--)
        {
            if ( hand.transform.GetChild(i).gameObject.activeSelf == false )
            {
                hand.transform.GetChild(i).gameObject.SetActive(true);
                return;
            }
        } */