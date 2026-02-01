using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource;
    private float minIntensity = 0f;
    private float maxIntensity = 1f;
    private float minTime = 1f;
    private float maxTime = 3f;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            float wait = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(wait);
        }
    }
}
