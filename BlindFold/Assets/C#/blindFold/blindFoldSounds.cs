using UnityEngine;

public class blindFoldSounds : MonoBehaviour
{
    public AudioSource maskOnSound;
    public AudioSource maskOffSound;

    public void PlaySoundMaskon()
    {
        maskOnSound.Play();
    }

    public void StopSoundMaskon()
    {
        maskOnSound.Stop();
    }

    public void PlaySoundMaskoff()
    {
        maskOffSound.Play();
    }

    public void StopSoundMaskoff()
    {
        maskOffSound.Stop();
    }
}
