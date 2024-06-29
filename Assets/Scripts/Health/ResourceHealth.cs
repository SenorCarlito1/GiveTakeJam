using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHealth : MonoBehaviour, IDamage
{
    [Header("----Resource Health----")]
    [SerializeField] public float maxHealth;
    [SerializeField] GameObject rescource;
    public float currHealth;
    private float origHealth;
    private string resourceName;

    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
        resourceName = gameObject.name;
    }

    public void TakeDamage(float dmg)
    {
        GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
        switch (resourceName)
        {
            case "Stone":
                if (GameManager.instance.currentTool == 2)
                {
                    GameManager.instance.stoneCount += 4;
                }
                else if (GameManager.instance.currentTool == 3)
                {
                    GameManager.instance.stoneCount += 2;
                }
                else if (GameManager.instance.currentTool == 1)
                {
                    GameManager.instance.stoneCount += 0;
                }
                break;
            case "Tree":
                if (GameManager.instance.currentTool == 2)
                {
                    GameManager.instance.woodCount += 2;
                }
                else if (GameManager.instance.currentTool == 3)
                {
                    GameManager.instance.woodCount += 3;
                }
                else if (GameManager.instance.currentTool == 1)
                {
                    GameManager.instance.woodCount += 0;
                }
               
               
                break;
        }

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
        for (int i = 0; i < GameManager.instance.hotBarMenu.transform.childCount; i++)
        {
            Destroy(GameManager.instance.hotBarMenu.transform.transform.GetChild(i).gameObject);
        }
        GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
    }

    private void BreakTree()
    {
        Destroy(gameObject, 1);
    }
}
