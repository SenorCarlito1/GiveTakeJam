using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Food,
    Tool,
    Resource,
    Part,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public GameObject itemPrefab;
    public Sprite itemImage;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
