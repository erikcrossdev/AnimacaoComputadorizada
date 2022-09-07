using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    private AudioSource _source;
    // Start is called before the first frame update
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
  
}
