using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    [Header("----Attack Parameters----")]
    public List<ToolStats> toolList = new List<ToolStats>();
    [SerializeField] private float damage;
    [SerializeField] private float durability;
    [SerializeField] private float attackInterval;
    [SerializeField] private MeshFilter toolModel;
    [SerializeField] private MeshRenderer toolMat;
    [SerializeField] private MeshCollider toolCollider;

    //[Header("----Tool Locker----")]
    //[SerializeField] GameObject swordPrefab;
    //[SerializeField] private BoxCollider weaponReach;
    //public ToolStats toolStats;
    //public Transform weaponLocation;

    private Animator anim;
    private Rotate rotation;

    private void Start()
    {
        anim = GameManager.instance.playerAnim;
        //weaponReach = GetComponent<BoxCollider>();
        //weaponReach.gameObject.SetActive(false);
    }

    private void Update()
    {
        //weaponLocation = GameObject.Find("Wrist_R").transform;

        gameObject.transform.rotation = gameObject.transform.parent.rotation;

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
            gameObject.GetComponent<MeshCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    public void AddTool(ToolStats toolStat)
    {
        //Instantiate(swordPrefab, transform.position, Quaternion.identity);

        toolList.Add(toolStat);

        damage = toolStat.damage;
        attackInterval = toolStat.attackInterval;
        durability = toolStat.durability;

        toolModel.mesh = toolStat.model.GetComponent<MeshFilter>().sharedMesh;
        toolMat.material = toolStat.model.GetComponent<MeshRenderer>().sharedMaterial;
        toolCollider.sharedMesh = toolModel.mesh;
        gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
