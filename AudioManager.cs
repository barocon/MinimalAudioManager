using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    /* Made by Justus Pousi | https://twitter.com/barocon
     * 
     * Use the singleton to access the AudioManager. 
     *
     * The commands are: 
     * AudioManager.singleton.Play3D(...);
     * AudioManager.singleton.PlayMusic(...);
     * AudioManager.singleton.StopMusic();
     * AudioManager.singleton.SetAllAudioSpeed(...);
     * AudioManager.singleton.ResetAudioSpeed();
     */
    public static AudioManager singleton;

    [Tooltip("Default Audio Mixer Group")]
    public AudioMixerGroup defaultGroup;

    private List<AudioUnit> _audioUnits = new List<AudioUnit>();

    private AudioSource _musicAudioSource;

    void Awake ()
    {
        singleton = this;
        _musicAudioSource = GetComponent<AudioSource>();
    }

    // A class that holds the necessary properties of the audiosource gameobject
    public class AudioUnit
    {
        public Transform myTransform;
        public AudioSource audioSource;
        public float pitch = 1f;

        // Constructor for the audio unit. Makes a new gameobject and sets it up.
        public AudioUnit()
        {
            myTransform = new GameObject("audioUnit").transform;
            myTransform.parent = singleton.transform;
            audioSource = myTransform.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 15f;
        }
    }

    /// <summary>
    /// Play an audioclip   	
    /// </summary>
    /// <param name="clip">The audioclip you want to play. </param>
    /// <param name="position">Where you want to hear the sound. </param>
    /// <param name="pitchVariation">How much random pitch variation you want. Use 0 if you want none. </param>
    /// <param name="group">What AudioMixerGroup you want the clip to use </param>
    public void Play3D(AudioClip clip, Vector3 position, float pitchVariation, AudioMixerGroup group = null)
    {
        AudioUnit _unit = null;
        if (group == null)
        {
            group = defaultGroup;
        }

        // Try to find an unused audioUnit
        for (int i = 0; i < _audioUnits.Count; i++)
        {
            if (!_audioUnits[i].audioSource.isPlaying)
            {
                _unit = _audioUnits[i];
                break;
            }
        }

        // Else we create a new audioUnit
        if (_unit == null)
        {
            _unit = new AudioUnit();
            _audioUnits.Add(_unit);
        }

        if (pitchVariation != 0)
        {
            float variation = Random.Range(-pitchVariation, pitchVariation);
            _unit.audioSource.pitch = (1f + variation) * Time.timeScale;
            _unit.pitch = 1f + variation; // storing pitch for when we need to reset
        }
        else
        {
            _unit.audioSource.pitch = 1f;
        }
        _unit.audioSource.outputAudioMixerGroup = group;
        _unit.myTransform.position = position;
        _unit.audioSource.clip = clip;
        _unit.audioSource.Play();
    }

    /// <summary>
    /// Play this music clip.  	
    /// </summary>
    /// <param name="clip">The music clip you want to play. </param>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Missing audio clip", gameObject);
            return;
        }
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    // Change the pitch of every audioSource in the manager. Useful for slow-mo.
    public void SetAllAudioSpeed(float pitch)
    {
        for (int i = 0; i < _audioUnits.Count; i++)
        {
            _audioUnits[i].audioSource.pitch *= pitch;
        }
    }

    public void ResetAllAudioSpeed()
    {
        for (int i = 0; i < _audioUnits.Count; i++)
        {
            _audioUnits[i].audioSource.pitch = _audioUnits[i].pitch;
        }
    }
}
