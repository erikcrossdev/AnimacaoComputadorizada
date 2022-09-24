using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Songs {
    public string SongName;
    public AudioClip Clip;
}

[System.Serializable]
public class ToggleObject {
    public Toggle Toggle;
    public GameObject GameObject;
}
public class CanvasController : MonoBehaviour
{
    [SerializeField] Button Arrow;
    [SerializeField] Animator Anim;
    [SerializeField] AudioSource SongSource;
    [SerializeField] TMP_Dropdown SongsDropdown;
    [SerializeField] GameObject dollynho;
    [SerializeField] GameObject shield;

    [SerializeField] Slider Pitch;
    [SerializeField] Slider Volume;

    [SerializeField] List<ToggleObject> ToggleObjects = new List<ToggleObject>();

    [SerializeField] List<Songs> Songs = new List<Songs>();
    private List<string> _songNames = new List<string>();

    private bool isPanelOpen = false;
    public const string OPEN_BOOL = "Open";
    void Start()
    {

        for (int i = 0; i < Songs.Count; i++)
        {
            _songNames.Add(Songs[i].SongName);
        }

        SongsDropdown.AddOptions(_songNames);

        for (int i = 0; i < ToggleObjects.Count; i++)
        {
            ToggleObjects[i].GameObject.SetActive(ToggleObjects[i].Toggle.isOn);
        }

        SongSource.clip = Songs[0].Clip;
        SongSource.Play();
        OnChangePitch();
        OnChangeVolume();
    }

    public void OnChangeSong()
    {
        SongSource.clip = Songs[SongsDropdown.value].Clip;

        SongSource.Play();
    }

    public void OnChangePitch() {
        SongSource.pitch = Pitch.value;
    }

    public void ResetPitch()
    {
        Pitch.value = 1.0f;
    }

    public void ResetDollyPosition() {
        dollynho.transform.position = shield.transform.position;
        dollynho.transform.rotation = shield.transform.rotation;
    }

    public void OnChangeVolume() {
        SongSource.volume = Volume.value;
    }

    public void OnToggle(GameObject obj) {

        for (int i = 0; i < ToggleObjects.Count; i++)
        {
            if (ToggleObjects[i].GameObject == obj) {
                obj.SetActive(ToggleObjects[i].Toggle.isOn);
            }
        }
    
    }

    public void OnButtonClick() {
        isPanelOpen = !isPanelOpen;
        Anim.SetBool(OPEN_BOOL, isPanelOpen);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
