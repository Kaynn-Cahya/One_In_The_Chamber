using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadableSpriteRendererObj : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private Coroutine fadeCoroutine;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeCoroutine = null;
    }

    public void FadeOutObject(float fadeSpeedRelativeToDeltatime, Action onFadeOutCallback = null) {
        StopFadeCoroutineIfExists();
        GetSpriteRendererObjIfNull();

        fadeCoroutine = StartCoroutine(FadeOutSpriteRenderer(fadeSpeedRelativeToDeltatime, onFadeOutCallback));
    }

    public void FadeInObject(float fadeSpeedRelativeToDeltatime, Action onFadeInCallback = null) {
        StopFadeCoroutineIfExists();
        GetSpriteRendererObjIfNull();

        fadeCoroutine = StartCoroutine(FadeInSpriteRenderer(fadeSpeedRelativeToDeltatime, onFadeInCallback));
    }

    private void StopFadeCoroutineIfExists() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }
    }

    private void GetSpriteRendererObjIfNull() {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }


    private IEnumerator FadeOutSpriteRenderer(float relativeFadeSpeed, Action onFadeOutCallback) {

        float progress = 1f;

        Color temp;

        while (progress > 0f) {
            temp = spriteRenderer.color;
            temp.a = progress;
            spriteRenderer.color = temp;

            progress -= Time.deltaTime * relativeFadeSpeed;

            yield return new WaitForEndOfFrame();
        }

        temp = spriteRenderer.color;
        temp.a = 0f;
        spriteRenderer.color = temp;

        onFadeOutCallback?.Invoke();

        yield return null;
    }

    private IEnumerator FadeInSpriteRenderer(float relativeFadeSpeed, Action onFadeInCallback) {

        float progress = 0f;

        Color temp;

        while (progress < 1f) {
            temp = spriteRenderer.color;
            temp.a = progress;
            spriteRenderer.color = temp;

            progress += Time.deltaTime * relativeFadeSpeed;

            yield return new WaitForEndOfFrame();
        }

        temp = spriteRenderer.color;
        temp.a = 1f;
        spriteRenderer.color = temp;

        onFadeInCallback?.Invoke();

        yield return null;
    }
}
