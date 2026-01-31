using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(EventLoop());
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
        else
        {
            // No event while blindfolded
        }
    }

    IEnumerator HeartbeatEvent()
    {
        eventRunning = true;
        heartbeatSource.Play();

        GameManager.Instance.InteractionTextUIChange(
            "You hear fast heartbeats... don't look."
        );


        float timer = 0f;
        while (timer < heartbeatDuration)
        {
            if (GameManager.Instance.IsBlindfolded)
            {
                // YOU DIED
                SceneManager.LoadScene("GameOver");
            }

            timer += Time.deltaTime;
            yield return null;
        }

        heartbeatSource.Stop();
        GameManager.Instance.InteractionTextUIChange("");
        eventRunning = false;
    }

}

