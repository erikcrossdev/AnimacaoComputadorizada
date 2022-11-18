using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBone : MonoBehaviour
{
    public Transform target;
    public Transform origin;
    public GameObject visuals;
    public Vector3 originalPos;
    public Quaternion rotation;
    bool _follow = false;
    bool _followOrigin = false;

    public float speedPosition = 1f, speedRotation = 1f;

    // Use this for initialization
    void Start()
    {
        originalPos = transform.position;
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_follow)
        {
            var newPos = new Vector3(originalPos.x, originalPos.y, target.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, speedPosition * Time.deltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speedRotation * Time.deltaTime);
        }
        else if(_followOrigin){
            var newPos = new Vector3(originalPos.x, originalPos.y, origin.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, speedPosition * Time.deltaTime);
        }
    }


    public void SetFollow(bool follow) {
        _follow = follow;
        _followOrigin = !follow;
    }

    public void SetFollowOrigin(bool follow)
    {
        _followOrigin = follow;
    }


    public void SetActive(bool active) {
        visuals.SetActive(active);
    }
   

}
