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

    public float maxScale = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {

        propertyBlock = new MaterialPropertyBlock();

    }

    // Update is called once per frame
    void Update()
    {
        SetProperty();
    }

    void SetProperty() {
        rend.GetPropertyBlock(propertyBlock, 0);
        // propertyBlock.SetColor("_EmissionColor", new Color(1, 0.6f, 0.5f, 1));
        propertyBlock.SetFloat(property, AudioPeer.AmplitudeBuffer * maxScale);       
        if (setColor) SetColor();
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
