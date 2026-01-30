using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blood : MonoBehaviour
{
    private Volume _volume;
    private Vignette _vignette;

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float maxIntensity = 1f;

    void Awake()
    {
        _volume = GetComponent<Volume>();
        if (_volume?.profile.TryGet(out _vignette) == true)
        {
            _vignette.intensity.overrideState = true;
        }
    }

    void OnEnable()
    {
        if (_vignette != null) _vignette.intensity.value = 0f;
    }

    void Update()
    {
        if (_vignette != null && _vignette.intensity.value < maxIntensity)
        {
            _vignette.intensity.value += speed * Time.deltaTime;
        }
    }

    void OnDisable()
    {
        if (_vignette != null)
        {
            _vignette.intensity.value = 0f;
        }
    }
}