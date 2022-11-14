using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationLayerController : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] Toggle _idle;
    [SerializeField] Toggle _jump;
    [SerializeField] Toggle _crouch;
    [SerializeField] Toggle _sit;

    [SerializeField] Toggle _rifle;
    [SerializeField] Toggle _waveBoth;
    [SerializeField] Toggle _waveSigle;
    [SerializeField] Toggle _shoot;

    public const string IDLE = "Idle";
    public const string JUMP = "Jump";
    public const string CROUCH = "Crouch";
    public const string SIT = "Sit";

    public const string RIFLE = "Rifle";
    public const string WAVE_BOTH_HANDS = "WaveBothHands";
    public const string WAVE_SINGLE_HAND = "Wave";
    public const string SHOOT = "Shoot";

    public Animation IdleClip;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var param in _animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                _animator.ResetTrigger(param.name);
            }
        }
        Rifle();
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    bool AnimatorIsPlaying()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length >
               _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void Idle()
    {
        if (_idle.isOn)
        {
            _jump.isOn = false;
            _crouch.isOn = false;
            _sit.isOn = false;
            if (!AnimatorIsPlaying(IDLE))
            {
                Debug.Log("Playing Idle!!!!");
                _animator.SetTrigger(IDLE);
            }
        }
    }

    public void Jump()
    {
        if (_jump.isOn)
        {
            _idle.isOn = false;
            _crouch.isOn = false;
            _sit.isOn = false;
            _animator.SetTrigger(JUMP);
        }
    }

    public void Crouch()
    {
        if (_crouch.isOn)
        {
            _jump.isOn = false;
            _idle.isOn = false;
            _sit.isOn = false;
            _animator.SetTrigger(CROUCH);
        }
    }

    public void Sit()
    {
        if (_sit.isOn)
        {
            _jump.isOn = false;
            _idle.isOn = false;
            _crouch.isOn = false;
            _animator.SetTrigger(SIT);
        }
    }

    public void Rifle()
    {
        _animator.SetBool(RIFLE, true);
        _animator.SetBool(WAVE_BOTH_HANDS, false);
        _animator.SetBool(WAVE_SINGLE_HAND, false);
        _animator.SetBool(SHOOT, false);
        _waveBoth.isOn = false;
        _waveSigle.isOn = false;
        _shoot.isOn = false;

    }

    public void WaveBothHands()
    {
        _animator.SetBool(WAVE_BOTH_HANDS, true);
        _animator.SetBool(RIFLE, false);
        _animator.SetBool(WAVE_SINGLE_HAND, false);
        _animator.SetBool(SHOOT, false);
        _rifle.isOn = false;
        _waveSigle.isOn = false;
        _shoot.isOn = false;

    }

    public void WaveOneHand()
    {
        _animator.SetBool(WAVE_SINGLE_HAND, true);
        _animator.SetBool(WAVE_BOTH_HANDS, false);
        _animator.SetBool(RIFLE, false);
        _animator.SetBool(SHOOT, false);
        _waveBoth.isOn = false;
        _rifle.isOn = false;
        _shoot.isOn = false;

    }

    public void Shoot()
    {
        _animator.SetBool(SHOOT, _shoot.isOn);
        _animator.SetBool(RIFLE, true);
        _animator.SetBool(WAVE_BOTH_HANDS, false);
        _animator.SetBool(WAVE_SINGLE_HAND, false);
        _waveBoth.isOn = false;
        _waveSigle.isOn = false;
        //_shoot.isOn = false;

    }


    // Update is called once per frame
    void Update()
    {

    }
}
