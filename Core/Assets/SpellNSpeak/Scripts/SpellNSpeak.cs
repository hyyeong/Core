//
// Credit to:
// https://bitbucket.org/Nerull22/unity-reusable-scripts/src/14dd64f454fda8625ba7375295080ea68fb6c1ae/Unity%20Reusables%20Project/Assets/Reusable%20Scripts/Helper%20Scripts/GetInputKeyCode.cs?at=develop
//

using System;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SpellNSpeak : MonoBehaviour {

    // --------------------------------------------------------------------------------------------- DATA MEMBERS

    public float minVol = 0.5f;
    public float maxVol = 1.0f;
    AudioSource srcAudio;


    // --------------------------------------------------------------------------------------------- UNITY METHODS

    void Awake() {
        srcAudio = gameObject.GetComponent<AudioSource>();
        
        return;
    }


    void Update() {
        bool loud = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        foreach (KeyCode curCode in ((KeyCode[])Enum.GetValues(typeof(KeyCode))).Where(Input.GetKeyDown)) {
            AudioClip clip = Resources.Load("SpellNSpeak/Audio/" + curCode.ToString()) as AudioClip;

            srcAudio.PlayOneShot(clip, loud ? maxVol : minVol);
        }

        return;
    }
}
