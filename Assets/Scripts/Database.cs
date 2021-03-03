using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemObjectCollection
{
    public Item item;
    public GameObject itemGameObject;
}

public class Database : MonoBehaviour
{
    public static Database instance { get; private set; }
    public ItemObjectCollection[] ItemDataBase;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < ItemDataBase.Length; i++)
        {
            ItemDataBase[i].item = new Item(
                ItemDataBase[i].itemGameObject.name,
                ItemDataBase[i].itemGameObject.GetComponent<SpriteRenderer>().sprite
            );
        }
    }

    public Item SearchItemBy(GameObject gameObject)
    {
        for (int i = 0; i < ItemDataBase.Length; i++)
        {            
            if (ItemDataBase[i].itemGameObject.GetComponent<SpriteRenderer>().sprite == gameObject.GetComponent<SpriteRenderer>().sprite)
            {
                return ItemDataBase[i].item;
            } 
        }
        return null;
    }

    public GameObject SearchGameObjBy(Item item)
    {
        for (int i = 0; i < ItemDataBase.Length; i++)
        {
            if ( item.Sprite == ItemDataBase[i].item.Sprite )
            {
                return ItemDataBase[i].itemGameObject;
            }
        }

        throw new System.Exception("item doesn't match any gameObject");
    }
}