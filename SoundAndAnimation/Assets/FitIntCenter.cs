using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitIntCenter : MonoBehaviour
{
    public Transform parent;
    private Vector3 _updatePosition;
    [SerializeField] SkinnedMeshRenderer Mesh;

    // Start is called before the first frame update
    void Start()
    {
        _updatePosition = new Vector3(Mesh.bounds.center.x, Mesh.bounds.center.y - Mesh.bounds.extents.y, Mesh.bounds.center.z);
    }

    // Update is called once per frame
    void Update()
    {
        _updatePosition = new Vector3(Mesh.bounds.center.x, Mesh.bounds.center.y - Mesh.bounds.extents.y, Mesh.bounds.center.z);
      
        //transform.position = _updatePosition;
        //var newPos = transform.TransformPoint(_updatePosition);
        parent.localPosition = - _updatePosition;
    }
}
