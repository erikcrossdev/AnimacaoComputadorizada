using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderProperty : MonoBehaviour
{
    public string property;
    public string ColorProperty;
    [SerializeField]
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;
    public Vector3 AxisColor;
    public Color Color;
    public bool setColor =false;

    public float MaxScale = -2;
    public float DeformationScale = 1;
  
    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        SetProperty();
    }

    void SetProperty() {
        rend.GetPropertyBlock(propertyBlock, 0);
        propertyBlock.SetFloat(property, AudioPeer.AmplitudeBuffer * (MaxScale));
        if (setColor) {
            SetColor(); 
        }
        rend.SetPropertyBlock(propertyBlock, 0);
    }

    void SetColor() {

        Color = new Color(AudioPeer._audioBandBuffer[(int)AxisColor.x], AudioPeer._audioBandBuffer[(int)AxisColor.y], AudioPeer._audioBandBuffer[(int)AxisColor.z], 1);
        propertyBlock.SetColor(ColorProperty, Color);
    }
    private void Reset()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
    }
}
