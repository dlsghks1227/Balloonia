using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public UnityEngine.UI.Image fade;

    public void FadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeIn(fadeOutTime, nextEvent));
    }

    public void FadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeOut(fadeOutTime, nextEvent));
    }

    IEnumerator CoFadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        Color tempColor = fade.color;
        while(tempColor.a < 1.0f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            fade.color = tempColor;

            if (tempColor.a >= 1.0f)
                tempColor.a = 1.0f;
            yield return null;
        }
        fade.color = tempColor;
        if (nextEvent != null)
            nextEvent();
    }

    IEnumerator CoFadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
		Color tempColor = fade.color;
		while(tempColor.a > 0.0f){
			tempColor.a -= Time.deltaTime / fadeOutTime;
            fade.color = tempColor;

			if(tempColor.a <= 0.0f)
                tempColor.a = 0.0f;
            yield return null;
		}
        fade.color = tempColor;
        if (nextEvent != null)
            nextEvent();
    }
}
