using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHealth : MonoBehaviour, IDamage
{
    [Header("----Resource Health----")]
    [SerializeField] public float maxHealth;
    [SerializeField] private ResourceObject woodResource;
    [SerializeField] private ResourceObject stoneResource;

    private ResourceObject _resourceObject;
    public float currHealth;
    private float origHealth;
    private string resourceName;

    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
        resourceName = gameObject.name;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I TOUCHED SOMETHING");
    }
    public void TakeDamage(float dmg)
    {
        int _resourceToAdd = 0;
       
        //switch (resourceName)
        //{
        //    case "Stone":
        //        if (GameManager.instance.currentTool == 2)
        //        {
        //            GameManager.instance.stoneCount += 4;
        //            _resourceToAdd = 4;
        //        }
        //        else if (GameManager.instance.currentTool == 3)
        //        {
        //            GameManager.instance.stoneCount += 2;
        //            _resourceToAdd = 2;
        //        }
        //        else if (GameManager.instance.currentTool == 1)
        //        {
        //            GameManager.instance.stoneCount += 0;
        //            _resourceToAdd = 0;
        //        }
        //        else if (GameManager.instance.currentTool == 0)
        //        {
        //            GameManager.instance.stoneCount += 1;
        //            _resourceToAdd = 1;
        //        }
        //        _resourceObject = stoneResource;
        //        break;
        //    case "Tree":
        //        if (GameManager.instance.currentTool == 2)
        //        {
        //            GameManager.instance.woodCount += 2;
        //            _resourceToAdd = 2;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 3)
        //        {
        //            GameManager.instance.woodCount += 3;
        //            _resourceToAdd = 3;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 1)
        //        {
        //            GameManager.instance.woodCount += 0;
        //            _resourceToAdd = 0;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 0)
        //        {
        //            GameManager.instance.woodCount += 1;
        //            _resourceToAdd = 1;
        //            Debug.Log("Got Wood");
        //        }
        //        _resourceObject = woodResource;
        //        break;
        //    case "Tree (1)":
        //        if (GameManager.instance.currentTool == 2)
        //        {
        //            GameManager.instance.woodCount += 2;
        //            _resourceToAdd = 2;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 3)
        //        {
        //            GameManager.instance.woodCount += 3;
        //            _resourceToAdd = 3;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 1)
        //        {
        //            GameManager.instance.woodCount += 0;
        //            _resourceToAdd = 0;
        //            Debug.Log("Got Wood");
        //        }
        //        else if (GameManager.instance.currentTool == 0)
        //        {
        //            GameManager.instance.woodCount += 1;
        //            _resourceToAdd = 1;
        //            Debug.Log("Got Wood");
        //        }
        //        _resourceObject = woodResource;
        //        break;
        //}
        if (gameObject.CompareTag("Tree"))
        {
            if (GameManager.instance.currentTool == 2)
            {
                GameManager.instance.woodCount += 2;
                _resourceToAdd = 2;
                Debug.Log("Got Wood");
            }
            else if (GameManager.instance.currentTool == 3)
            {
                GameManager.instance.woodCount += 3;
                _resourceToAdd = 3;
                Debug.Log("Got Wood");
            }
            else if (GameManager.instance.currentTool == 1)
            {
                GameManager.instance.woodCount += 0;
                _resourceToAdd = 0;
                Debug.Log("Got Wood");
            }
            else if (GameManager.instance.currentTool == 0)
            {
                GameManager.instance.woodCount += 1;
                _resourceToAdd = 1;
                Debug.Log("Got Wood");
            }
            _resourceObject = woodResource;
        }
        GameManager.instance.hotBarObject.AddItem(_resourceObject, _resourceToAdd);
        Debug.Log(_resourceToAdd);
        GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
        Debug.Log("Created Display");

        currHealth -= dmg;
        //int ranZ = Random.Range(-1, 1);
        //int ranX = Random.Range(-1, 1);
        //int forceRanX = Random.Range(-100, 100);
        //int forceRanZ = Random.Range(-100, 100);

        //var instance = Instantiate(rescource, gameObject.transform.position + new Vector3(ranX, 2, ranZ), Quaternion.identity);
        //// Adds force to the instantiated Objected
        //instance.GetComponent<Rigidbody>().AddForce(new Vector3(forceRanX, 2, forceRanZ));
        if (currHealth <= 0)
        {
            Debug.Log("Broke Tree!");
            BreakTree();
        }
       
    }

    private void BreakTree()
    {
        Destroy(gameObject, 1);
    }
}
