using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialProperty : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;

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
        //loat rand = 0.0f, 1f * (range/6;
        randR = (Random.Range(0.1f, range) / range)/10;
        //Debug.Log($"rand r = {rand}, rand / (range / 10) = {rand / (range / 10)}, range = {range}, range/10 = {range/10}");
        randG = (Random.Range(0.1f, range) / range)/10;
        randB = (Random.Range(0.1f, range) / range)/10;
        Color = new Color(randR, randG, randB, 1);
        Debug.Log($"R,G,B: {randR}, range: {range}");
        rend.GetPropertyBlock(propertyBlock, 0);
       // propertyBlock.SetColor("_EmissionColor", new Color(1, 0.6f, 0.5f, 1));
        propertyBlock.SetColor("_EmissionColor", Color);
        //propertyBlock.SetColor("_Color", new Color(1, 0.6f, 0.5f, 1));
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
