using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{

    [Header("----Equipment Stats----")]
    [SerializeField] public int serialNumber;
    [SerializeField] public float damage;
    [SerializeField] public float durability;
    //[SerializeField] public Collider hitCollider;
    [SerializeField] public float attackInterval;
    //public MeshCollider collider;
    public GameObject model;
    public AudioClip swingSound;
    [Range(0, 1)] public float swingSoundVol;
    public AudioClip hitSound;
    [Range(0, 1)] public float hitSoundVol;

    public void Awake()
    {
        type = ItemType.Equipment;
    }

}
