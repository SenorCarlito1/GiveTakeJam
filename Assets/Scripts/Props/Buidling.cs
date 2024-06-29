using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buidling : MonoBehaviour
{

    private Renderer[] _renderer;
    private MeshCollider[] _meshCollider;
    private MeshCollider meshThingy;
    private Material _defaultMaterial;
    [SerializeField] Material _originalMaterial;
    // Start is called before the first frame update

    private void Awake()
    {
        _renderer = GetComponentsInChildren<Renderer>();
        _meshCollider = GetComponentsInChildren<MeshCollider>();
       
        for (int i = 0; i < _renderer.Length; i++)
        {
            if (_renderer[0]) _defaultMaterial = _renderer[0].material;

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateMaterial(Material newMaterial)
    {
        for (int i = 0; i < _meshCollider.Length; i++)
        {
          _meshCollider[i].enabled = false;
        }
       
        for (int i = 0; i < _renderer.Length; i++)
        {
            if (_renderer[i].material != newMaterial)
            {
                _renderer[i].material = newMaterial;
            }

        }
       
    }

    public void PlaceBuilding()
    {
            //_renderer = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < _renderer.Length; i++)
        {
            //if (_renderer[i]) _defaultMaterial = _renderer[i].material;
            _renderer[i].material = _originalMaterial;
        }
        for (int i = 0; i < _meshCollider.Length; i++)
        {
            _meshCollider[i].enabled = true;
        }
    }
}
