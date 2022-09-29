using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitIntCenter : MonoBehaviour
{
    public Transform parent;
    private Vector3 _updatePosition;
    [SerializeField] SkinnedMeshRenderer Mesh;

    void Start()
    {
        _updatePosition = new Vector3(Mesh.bounds.center.x, Mesh.bounds.center.y - Mesh.bounds.extents.y, Mesh.bounds.center.z);
    }

    // Update is called once per frame
    void Update()
    {
        _updatePosition = new Vector3(Mesh.bounds.center.x, Mesh.bounds.center.y - Mesh.bounds.extents.y, Mesh.bounds.center.z);
      
        parent.localPosition = - _updatePosition;
    }
}
