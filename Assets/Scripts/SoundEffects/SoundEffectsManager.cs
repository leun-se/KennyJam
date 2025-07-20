using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the audioClip
        audioSource.clip = audioclip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

    }
}
