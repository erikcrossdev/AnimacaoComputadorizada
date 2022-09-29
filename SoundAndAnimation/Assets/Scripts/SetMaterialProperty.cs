using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialProperty : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;
    private const string EMISSION_COLOR = "_EmissionColor";

    public float randR;
    public Color Color;
    public float randG;
    public float randB;
    public float range;
    void Awake()
    {

        propertyBlock = new MaterialPropertyBlock();

    }

    // Start is called before the first frame update
    public void SetColor(float r)
    {
        range = Mathf.Clamp(r,0f,10f);
        randR = (Random.Range(0.1f, range) / range)/10;
        randG = (Random.Range(0.1f, range) / range)/10;
        randB = (Random.Range(0.1f, range) / range)/10;
        Color = new Color(randR, randG, randB, 1);
        rend.GetPropertyBlock(propertyBlock, 0);
        propertyBlock.SetColor(EMISSION_COLOR, Color);
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
