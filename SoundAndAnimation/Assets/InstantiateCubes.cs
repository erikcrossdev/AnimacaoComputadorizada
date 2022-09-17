using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    public SetMaterialProperty _sampleCubePrefab;
    public float maxScale = 1.5f;

    SetMaterialProperty[] _sampleCube = new SetMaterialProperty[512];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _sampleCube.Length; i++)
        {
            SetMaterialProperty _instanceCube = Instantiate(_sampleCubePrefab);
            _instanceCube.transform.position = this.transform.position;
            _instanceCube.transform.parent = this.transform;
            _instanceCube.name = "Cube " + i;

            this.transform.eulerAngles = new Vector3(0, (AudioPeer.Samples/360) * i, 0);
            _instanceCube.transform.position = Vector3.forward * 100;
            _sampleCube[i] = _instanceCube;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _sampleCube.Length; i++)
        {
            if (_sampleCube != null) {
                _sampleCube[i].transform.localScale = new Vector3(1, AudioPeer._samples[i]* maxScale, 1);
               
                _sampleCube[i].SetColor(AudioPeer._samples[i]);
            }
        }
    }
}
