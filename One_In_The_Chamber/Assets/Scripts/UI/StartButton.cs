using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField, Tooltip("Name of scene to change to")]
    private string sceneToLoad;

    public void OnPointerDown(PointerEventData data) {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene() {
        SoundManager.Instance.PlayAudioFileBySoundType(SoundType.SHOOT_SHOTGUN);

        yield return new WaitForSeconds(0.6f);

        SceneTransitionManager.Instance.TransitionToSceneBySceneName(sceneToLoad);
    }
}
