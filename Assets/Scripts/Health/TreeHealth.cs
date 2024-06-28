using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHealth : MonoBehaviour, IDamage
{
    [Header("----Tree Health----")]
    [SerializeField] public float maxHealth;
    [SerializeField] GameObject wood;
    public float currHealth;
    private float origHealth;

    


    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
      
    }

    public void TakeDamage(float dmg)
    {
        
        
        currHealth -= dmg;
       int ranZ = Random.Range(-1, 1);
       int ranX= Random.Range(-1, 1);
       int forceRanX = Random.Range(-100, 100);
       int forceRanZ = Random.Range(-100, 100);

        var instance = Instantiate(wood, gameObject.transform.position + new Vector3(ranX, 2, ranZ), Quaternion.identity);
        // Adds force to the instantiated Objected
        instance.GetComponent<Rigidbody>().AddForce(new Vector3(forceRanX, 2, forceRanZ));
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
