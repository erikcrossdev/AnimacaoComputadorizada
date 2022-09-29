using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ControlParticleBySound : MonoBehaviour
{
    public ParticleSystem _particle;
    public float GravityMultiplier;
    public float strength;
    public float speed;
    public Vector3 Axis;
    public Color Color;
    public float randA =1;
    public float range;
    public float Amplitude;
    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        Amplitude = AudioPeer.Amplitude;
      
        Color = new Color(AudioPeer._audioBandBuffer[(int)Axis.x], AudioPeer._audioBandBuffer[(int)Axis.y], AudioPeer._audioBandBuffer[(int)Axis.z], randA);
        var main = _particle.main;
        main.simulationSpeed = Amplitude * speed;
        main.startColor = Color;
        main.gravityModifier = Amplitude *strength * GravityMultiplier;
        var noise = _particle.noise;

        noise.strength = Amplitude * strength;
        noise.scrollSpeed = Amplitude * strength;
    }
}
