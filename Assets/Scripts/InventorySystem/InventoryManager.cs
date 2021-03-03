using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public enum EquipedItem
{
    first,
    second,
    third,
    fourth,
    fifth,
    none,
}

public class InventoryManager : MonoBehaviour 
{
    public static InventoryManager instance { get; set; }
    private Item[] inventory = new Item[5];
    private EquipedItem equipedItemNumber = EquipedItem.none;
    private PlayerController playerController;

    public Item equipedItem
    {
        get
        {
            return inventory[ (int) equipedItemNumber ];
        }
        set
        {

        }
    }

    void Awake()
    {
        instance = this;
    }

    private void Start() 
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    private void ListenSwitchSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if ( inventory[ (int) EquipedItem.first ] != null )
            {
                equipedItemNumber = EquipedItem.first;
                playerController.ChangeItem((int)equipedItemNumber);
                UI_Inventory.instance.defineSlot((int)equipedItemNumber);
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if ( inventory[ (int) EquipedItem.second ] != null )
            {
                equipedItemNumber = EquipedItem.second;
                playerController.ChangeItem((int)equipedItemNumber);
                UI_Inventory.instance.defineSlot((int)equipedItemNumber);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if ( inventory[ (int) EquipedItem.third ] != null )
            {
                equipedItemNumber = EquipedItem.third;
                playerController.ChangeItem((int)equipedItemNumber);
                UI_Inventory.instance.defineSlot((int)equipedItemNumber);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if ( inventory[ (int) EquipedItem.fourth ] != null )
            {
                equipedItemNumber = EquipedItem.fourth;
                playerController.ChangeItem((int)equipedItemNumber);
                UI_Inventory.instance.defineSlot((int)equipedItemNumber);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if ( inventory[ (int) EquipedItem.fifth ] != null )
            {
                equipedItemNumber = EquipedItem.fifth;
                playerController.ChangeItem((int)equipedItemNumber);
                UI_Inventory.instance.defineSlot((int)equipedItemNumber);
            }
        }
    }

    public void EquipNew()
    {
        for (int i = 0; i < inventory.Length; i--)
        {
            if (inventory[i - 1] != null)
            {
                Debug.Log("Try to equip: success");
                equipedItemNumber = (EquipedItem) i;
                UI_Inventory.instance.defineSlot(i);
                return;
            }
        }
        Debug.Log( equipedItemNumber );
        throw new System.Exception("inventory doesn't contain any item");
    }

    public void TryToAdd(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                Debug.Log("TryToAdd: success");
                AddToInventory(item, i);
                return;
            }
        }
        throw new System.Exception("Max items in inventory"); 
    }

    public void AddToInventory(Item itemToAdd, int slotIndex)
    {
        // inventory[slotIndex] = item;
        inventory.SetValue(itemToAdd, slotIndex);
        UI_Inventory.instance.ChangeSlot(itemToAdd.Sprite, slotIndex);

        equipedItemNumber = (EquipedItem) slotIndex;
        Debug.Log(equipedItemNumber);
        Debug.Log( (int) equipedItemNumber);

        // equipedItem = itemToAdd;
        UI_Inventory.instance.defineSlot(slotIndex);
    }

    public void TryToRemove(Item equipedItem)
    {
        // constantly update equiped item when trying to remove
        if (equipedItem == null || equipedItemNumber == EquipedItem.none)
        {
            Debug.Log("Can't delete item, because item doesn't exist!");
            return;
        }
        else
        {
            RemoveAndEquip();
        }
    }

    public void Remove()
    {
        inventory[(int)equipedItemNumber] = null;    // clear inventory inner slot
        UI_Inventory.instance.ChangeSlot(null, (int)equipedItemNumber);   // delete icon
        UI_Inventory.instance.defineSlot((int)equipedItemNumber);        // change selection
        playerController.UnequipItem((int)equipedItemNumber);  // drop

        equipedItemNumber = EquipedItem.none;
    }

    public void RemoveAndEquip()
    {
        Remove();
        
        if ( !InvenotryIsEmpty() )
        {
            this.EquipNew(); // change item it inventory, get new equipedItemNumber
            playerController.EquipNew( (int)equipedItemNumber  ); // change item in hand
        }
        else
        {
            equipedItemNumber = EquipedItem.none;
        }

        Debug.Log( (int) equipedItemNumber);
    }

    public bool InvenotryIsEmpty()
    {
        if ( inventory.All( x => x == null )  )
        { 
            return true;
        }
        else
        {
            Debug.Log("Inventory is not Empty");
            return false;
        }
    }

    void Update()
    {
        // listen alpha key code pressed
        ListenSwitchSlot();

        // listen removing
        ListenRemove();
    }


    private void CheckAdding()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryToAdd( Database.instance.ItemDataBase[1].item );
        }
    }

    private void ListenRemove()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryToRemove(equipedItem);
        }
    }

}