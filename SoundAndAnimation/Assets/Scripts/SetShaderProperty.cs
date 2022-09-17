using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderProperty : MonoBehaviour
{
    public string property;
    [SerializeField]
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;

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
