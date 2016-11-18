# MinimalAudioManager
A minimal AudioManager with pooling for Unity.
Useful for prototypes and small games.

# Instructions:
1. Add this script to a manager gameobject in Unity
2. Use the singleton to access the AudioManager. 

# The commands are: 
AudioManager.singleton.Play3D(...)

AudioManager.singleton.PlayMusic(...)

AudioManager.singleton.StopMusic()

AudioManager.singleton.SetAllAudioSpeed(...);

AudioManager.singleton.ResetAudioSpeed();
