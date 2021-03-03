using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory instance { get; private set; }
    private GameObject[] ItemSlots = new GameObject[5];
    private GameObject[] items = new GameObject[5];

    public Sprite defaultItemSlotSprite;
    GameObject previousSlot;

    private bool previousSelectionExists = false;

    void Awake()
    {
        instance = this;
    }

    private void Start() 
    {
        // Get all Slots GameObjects
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            ItemSlots[i] = gameObject.transform.GetChild(i).gameObject; // get item slot with number i
            items[i] = ItemSlots[i].transform.GetChild(0).gameObject;   // get item 
        }

        previousSlot = new GameObject();
    }

    public void ChangeSlot(Sprite spriteToChange, int index)
    {
        Image itemImage = items[index].GetComponent<Image>();
        if (spriteToChange != null) // if we want add item to the inventory
        {
            itemImage.sprite = spriteToChange;
            itemImage.gameObject.SetActive(true);
        }
        else                        // if we want to delete item from the inventory
        {
            itemImage.sprite = null;
            itemImage.gameObject.SetActive(false);
        }
    }

    public void defineSlot(int index)
    {
        Image itemImage = items[index].GetComponent<Image>();

        // itemImage.IsActive()
        if ( itemImage.gameObject.activeSelf == true )  // if item slot contains item)
        {
            if ( !previousSelectionExists )     // if item is equipped for the first time
            {
                previousSlot = ItemSlots[index];
                previousSelectionExists = true;

                ScaleInTime(ItemSlots[index]);
                Debug.Log("Slot Upscaled!");
            }                                   // if item is equiped any time after first
            else if ( previousSelectionExists && previousSlot != ItemSlots[index] )
            {
                DownScaleInTime(previousSlot);
                ScaleInTime(ItemSlots[index]);
                previousSlot = ItemSlots[index];
                Debug.Log("Change Slot!");
            }
        }
        else
        {
            // another condition for childs
            previousSelectionExists = false;
            DownScaleInTime( ItemSlots[index] );
            Debug.Log("Item is deleted and downscaled");
        }
    }

    private void ScaleInTime(GameObject itemSlot)
    {
        Vector2 slotScale = itemSlot.GetComponent<RectTransform>().localScale;
        slotScale = new Vector2(1.1f, 1.1f);
        itemSlot.GetComponent<RectTransform>().localScale = slotScale;
        
        /*  for (float scaleX = slotScale.x, scaleY = slotScale.y; 
            scaleX < MaximumScale; 
            scaleX += 0.01f, scaleY += 0.01f)
        {

        } */
    }

    private void DownScaleInTime(GameObject itemSlot)
    {
        Vector2 slotScale = itemSlot.GetComponent<RectTransform>().localScale;
        slotScale = new Vector2(1.0f, 1.0f);
        itemSlot.GetComponent<RectTransform>().localScale = slotScale;

        /*  for (float scaleX = slotScale.x, scaleY = slotScale.y;
            scaleX > MinimumScale;
            scaleX -= 0.01f, scaleY = 0.01f)
        {

        } */
    }

/*  private void Update() {
        
    } */

}