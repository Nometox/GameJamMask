using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance;

    [Header("General")]
    public float minTimeBetweenEvents = 10f;
    public float maxTimeBetweenEvents = 25f;

    private bool eventRunning = false;

    [Header("Heartbeat Event")]
    public AudioSource heartbeatSource;
    public float heartbeatDuration = 6f;

    [Header("Footstep Event")]
    public AudioSource footstepAudioSource;      // AudioSource for footsteps
    public AudioClip[] footstepSounds;           // Array of footstep sounds
    public float idleTimeBeforeFootsteps = 5f;   // How long player must stand still before footsteps
    public float timeAfterFootstepsBeforeDeath = 2f; // Extra seconds after footsteps to die if still

    private float idleTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(EventLoop());
    }

    void Update()
    {
        HandleFootstepIdleCheck();
    }

    IEnumerator EventLoop()
    {
        while (true)
        {
            float wait = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            yield return new WaitForSeconds(wait);

            if (!eventRunning)
            {
                TryStartRandomEvent();
            }
        }
    }

    void TryStartRandomEvent()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.IsBlindfolded)
        {
            StartCoroutine(HeartbeatEvent());
        }
    }

    IEnumerator HeartbeatEvent()
    {
        eventRunning = true;
        heartbeatSource.Play();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.InteractionTextUIChange(
            "You hear fast heartbeats... don't look."
        );

        float timer = 0f;
        while (timer < heartbeatDuration)
        {
            if (GameManager.Instance.IsBlindfolded)
            {
                HorrorManager.Instance.PlayRandomScare();
            }

            timer += Time.deltaTime;
            yield return null;
        }

        heartbeatSource.Stop();
        GameManager.Instance.InteractionTextUIChange("");
        eventRunning = false;
    }

    // ------------------ Footstep & Idle Death ------------------

    private void HandleFootstepIdleCheck()
    {
        if (eventRunning) return;
        if (GameManager.Instance == null || footstepAudioSource == null) return;

        // Check if player is moving
        if (!GameManager.Instance.PlayerMoving)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeBeforeFootsteps)
            {
                StartCoroutine(FootstepThenDeathCheck());
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer = 0f;
        }
    }

    IEnumerator FootstepThenDeathCheck()
    {
        eventRunning = true;

        // ------------------ Footsteps ------------------
        if (footstepSounds.Length > 0)
        {
            int steps = Random.Range(2, 5);
            for (int i = 0; i < steps; i++)
            {
                int index = Random.Range(0, footstepSounds.Length);
                footstepAudioSource.PlayOneShot(footstepSounds[index]);
                yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));

                // If player moved during footsteps, cancel
                if (GameManager.Instance.PlayerMoving)
                {
                    eventRunning = false;
                    footstepAudioSource.Stop();
                    yield break;
                }
            }
        }

        // ------------------ Check for death ------------------
        float deathTimer = 0f;
        while (deathTimer < timeAfterFootstepsBeforeDeath)
        {
            if (GameManager.Instance.PlayerMoving)
            {
                eventRunning = false;
                yield break; // player moved, cancel death
            }
            deathTimer += Time.deltaTime;
            yield return null;
        }

        // Player stayed still → trigger death / jump scare
        GameManager.Instance.InteractionTextUIChange("You stayed still too long...");
        yield return new WaitForSeconds(1f);
        HorrorManager.Instance.PlayRandomScare();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");

        eventRunning = false;
    }
}
