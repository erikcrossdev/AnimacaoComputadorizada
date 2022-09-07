using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialProperty : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;
    private float _twirlStrength = 1;
    private float _speed = 3;

    private MaterialPropertyBlock propertyBlock;

    void Awake()
    {

        propertyBlock = new MaterialPropertyBlock();

    }

    // Start is called before the first frame update
    void Update()
    {

        

    }

    public void ChangeTwirl(bool reset) {
        rend.GetPropertyBlock(propertyBlock, 0);
        _twirlStrength = reset ? Mathf.PingPong(Time.time * _speed, 8) + 1 : Mathf.MoveTowards(_twirlStrength, 1, Time.time);
        Debug.Log("_twirl Strength " + _twirlStrength);
        propertyBlock.SetFloat("_TwirlStrength", _twirlStrength);
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
