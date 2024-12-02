using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DayNightPostProcessing : MonoBehaviour
{
    public Volume postProcessVolume;  // Reference to the global post-processing volume
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Ensure we have a valid reference to Color Adjustments
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f; // Initial post-exposure value for daytime
        }
    }

    public void SetNight(bool isNight)
    {
        StopAllCoroutines(); // Stop any ongoing transition
        StartCoroutine(TransitionPostExposure(isNight ? -1f : 0f, 5f));
    }

    private IEnumerator TransitionPostExposure(float targetValue, float duration)
    {
        if (colorAdjustments == null) yield break;

        float startValue = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            colorAdjustments.postExposure.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        colorAdjustments.postExposure.value = targetValue; // Ensure final value is set
    }
}
