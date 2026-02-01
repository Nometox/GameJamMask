using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HorrorManager : MonoBehaviour
{
    public static HorrorManager Instance;

    [Header("Jumpscare Visuals")]
    public GameObject[] jumpscares; // disabled prefabs/images

    [Header("Jumpscare Sounds")]
    public AudioClip[] scareSounds;

    public AudioSource audioSource;
    public float scareDuration = 1.2f;

    void Awake()
    {
        Instance = this;
    }

    // CALL THIS
    public void PlayRandomScare()
    {
        int scareIndex = Random.Range(0, jumpscares.Length);
        int soundIndex = Random.Range(0, scareSounds.Length);

        GameObject chosenScare = jumpscares[scareIndex];
        AudioClip chosenSound = scareSounds[soundIndex];

        StartCoroutine(ScareRoutine(chosenScare, chosenSound));
    }

    IEnumerator ScareRoutine(GameObject scare, AudioClip sound)
    {
        scare.SetActive(true);
        audioSource.PlayOneShot(sound);
        yield return new WaitForSeconds(scareDuration);
        SceneManager.LoadScene("GameOver");
    }
}
