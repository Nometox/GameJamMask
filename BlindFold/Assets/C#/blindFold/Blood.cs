using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blood : MonoBehaviour
{
    private Volume _volume;
    private Vignette _vignette;
    public AudioSource _audioSource;

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float maxIntensity = 1f;

    void Awake()
    {
        _volume = GetComponent<Volume>();
        _audioSource = GetComponent<AudioSource>();

        if (_volume?.profile.TryGet(out _vignette) == true)
        {
            _vignette.intensity.overrideState = true;
        }

        if (_audioSource != null)
        {
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
        }
    }

    void OnEnable()
    {
        _vignette.intensity.value = 0f;
        _audioSource.Play();
    }

    void Update()
    {
        if (_vignette != null)
        {
            if (_vignette.intensity.value < maxIntensity)
            {
                _vignette.intensity.value += speed * Time.deltaTime;
            }

            if (_vignette.intensity.value <= 0f && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

    void OnDisable()
    {
        if (_vignette != null)
        {
            _vignette.intensity.value = 0f;
        }

        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
