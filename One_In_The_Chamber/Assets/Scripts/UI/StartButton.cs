using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerDownHandler {
	[SerializeField, Tooltip("Name of scene to change to")]
	private string sceneToLoad;

	[SerializeField, Tooltip("Transition fader to fade the scene out.")]
	private FadableGraphicObj fader;

	public void OnPointerDown(PointerEventData data) {
		StartSceneTransition();
	}

	private void StartSceneTransition() {
		SoundManager.Instance.PlayAudioFileBySoundType(SoundType.SHOOT_SHOTGUN);
		fader.FadeInObject(1f, LoadNextScene);
	}

	private void LoadNextScene() {
		SceneTransitionManager.Instance.TransitionToSceneBySceneName(sceneToLoad);
	}
}
