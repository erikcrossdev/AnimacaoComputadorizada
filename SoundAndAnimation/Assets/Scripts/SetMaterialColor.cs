using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialColor : MonoBehaviour
{
    public string property;
    [SerializeField]
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;

    public Vector3 Axis;

    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }
    void Update()
    {
        SetProperty();
    }

    void SetProperty()
    {
        rend.GetPropertyBlock(propertyBlock, 0);
        propertyBlock.SetColor(property, new Color(AudioPeer._audioBand[(int)Axis.x], AudioPeer._audioBand[(int)Axis.y], AudioPeer._audioBand[(int)Axis.z]));
        rend.SetPropertyBlock(propertyBlock, 0);
    }
    private void Reset()
    {

        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

    }
}
