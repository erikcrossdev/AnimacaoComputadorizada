using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class DanceSpeed : MonoBehaviour
{
    public Animator _anim;
    public AudioSource _source;
    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        var speed = _source.pitch;
        if (speed < -0.1f) speed = -1f;
        _anim.speed = Mathf.Abs(speed);
    }
}
