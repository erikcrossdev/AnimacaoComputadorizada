using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSpeed : MonoBehaviour
{
    public Animator _anim;
    public AudioSource _source;
    public Transform transformToLook;
    public Transform lookAt;

    public List<AnimationClip> LookFrontClips = new List<AnimationClip>();
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var speed = _source.pitch;
        if (speed < -0.1f) speed = -1f;
        _anim.speed = Mathf.Abs(speed);


        var clipInfo = _anim.GetCurrentAnimatorClipInfo(0);
        if (LookFrontClips.Contains(clipInfo[0].clip))
        {
            transformToLook.transform.LookAt(lookAt);
        }
    }



}
