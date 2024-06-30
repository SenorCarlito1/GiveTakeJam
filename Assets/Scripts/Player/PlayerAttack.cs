using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    [Header("----Attack Parameters----")]
    public List<ToolStats> toolList = new List<ToolStats>();
    public int selectedTool;
    [SerializeField] private float damage;
    [SerializeField] private float durability;
    [SerializeField] private float attackInterval;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private MeshFilter toolModel;
    [SerializeField] private MeshRenderer toolMat;
    [SerializeField] public MeshCollider toolCollider;
    [SerializeField] public BoxCollider fistCollider;
    [SerializeField] public ToolStats fist;
    //[SerializeField] public BoxCollider SwordCollider;
    //[SerializeField] public BoxCollider PickCollider;
    //[SerializeField] public BoxCollider AxeCollider;

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
        //AddTool();
        AddTool(fist);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && cooldownTimer > attackInterval && toolList.Count > 0)
        {
            Attack();
        }

        ChangeTool();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (durability > 0)
        {
            anim.SetTrigger("Attack");
            durability--;
            cooldownTimer = 0;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            return;
        }

        IDamage dam = other.GetComponent<IDamage>();

        if (dam != null)
        {
            dam.TakeDamage(damage);
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
        toolMat.materials = toolStat.model.GetComponent<MeshRenderer>().sharedMaterials;

        toolCollider.sharedMesh = toolModel.mesh;
        gameObject.GetComponent<MeshCollider>().enabled = false;

        switch (toolStat.serialNumber)
        {
            case 1:
                gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                gameObject.transform.localPosition = new Vector3(0.03f, 0.217f, 0.005f);
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                break;
            case 2:
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.transform.localPosition = new Vector3(0.052f, 0.365f, 0.011f);
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                break;
            case 3:
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.transform.localPosition = new Vector3(0.012f, -0.0199f, -0.0031f);
                gameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
                break;
        }

        selectedTool = toolList.Count - 1;
        //toolCollider.enabled = false;
    }

    private void ChangeTool()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedTool < toolList.Count - 1)
        {
            selectedTool++;
            ChangeToolStats();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedTool > 0)
        {
            selectedTool--;
            ChangeToolStats();
        }
    }

    private void ChangeToolStats()
    {
        damage = toolList[selectedTool].damage;
        durability = toolList[selectedTool].durability;
        attackInterval = toolList[selectedTool].attackInterval;
        toolModel.mesh = toolList[selectedTool].model.GetComponent<MeshFilter>().sharedMesh;
        toolMat.materials = toolList[selectedTool].model.GetComponent<MeshRenderer>().sharedMaterials;

        switch (toolList[selectedTool].serialNumber)
        {
            case 1:
                gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                gameObject.transform.localPosition = new Vector3(0.03f, 0.217f, 0.005f);
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                break;
            case 2:
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.transform.localPosition = new Vector3(0.052f, 0.365f, 0.011f);
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                break;
            case 3:
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.transform.localPosition = new Vector3(0.012f, -0.0199f, -0.0031f);
                gameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
                break;
        }
    }
}
