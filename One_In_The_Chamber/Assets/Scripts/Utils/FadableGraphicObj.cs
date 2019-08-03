using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class FadableGraphicObj : MonoBehaviour {
    private Graphic graphicObj;

    private Coroutine fadeCoroutine;

    void Start() {
        graphicObj = GetComponent<Graphic>();
        fadeCoroutine = null;
    }

    public void FadeOutObject(float fadeSpeedRelativeToDeltatime, Action onFadeOutCallback = null) {
        StopFadeCoroutineIfExists();
        GetGraphicObjIfNull();

        fadeCoroutine = StartCoroutine(FadeOutGraphic(fadeSpeedRelativeToDeltatime, onFadeOutCallback));
    }

    public void FadeInObject(float fadeSpeedRelativeToDeltatime, Action onFadeInCallback = null) {
        StopFadeCoroutineIfExists();
        GetGraphicObjIfNull();

        fadeCoroutine = StartCoroutine(FadeInGraphic(fadeSpeedRelativeToDeltatime, onFadeInCallback));
    }

    private void StopFadeCoroutineIfExists() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }
    }

    private void GetGraphicObjIfNull() {
        if (graphicObj == null) {
            graphicObj = GetComponent<Graphic>();
        }
    }


    private IEnumerator FadeOutGraphic(float relativeFadeSpeed, Action onFadeOutCallback) {

        float progress = 1f;

        Color temp;

        while (progress > 0f) {
            temp = graphicObj.color;
            temp.a = progress;
            graphicObj.color = temp;

            progress -= Time.deltaTime * relativeFadeSpeed;

            yield return new WaitForEndOfFrame();
        }

        temp = graphicObj.color;
        temp.a = 0f;
        graphicObj.color = temp;

        onFadeOutCallback?.Invoke();

        yield return null;
    }

    private IEnumerator FadeInGraphic(float relativeFadeSpeed, Action onFadeInCallback) {

        float progress = 0f;

        Color temp;

        while (progress < 1f) {
            temp = graphicObj.color;
            temp.a = progress;
            graphicObj.color = temp;

            progress += Time.deltaTime * relativeFadeSpeed;

            yield return new WaitForEndOfFrame();
        }

        temp = graphicObj.color;
        temp.a = 1f;
        graphicObj.color = temp;

        onFadeInCallback?.Invoke();

        yield return null;
    }
}
