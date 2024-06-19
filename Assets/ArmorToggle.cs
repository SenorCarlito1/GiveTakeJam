using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] armors;

    private void Awake()
    {
        for (int i = 0; i < armors.Length; i++)
        {
            armors[i].gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        for (int i = 0; i < armors.Length; i++)
        {
            armors[i].gameObject.SetActive(false);
        }
    }
}
