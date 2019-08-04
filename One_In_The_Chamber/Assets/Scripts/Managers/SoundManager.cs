using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class SoundManager : Singleton<SoundManager> {

	#region Local_Function

	[System.Serializable]
	private struct AudioFile {
		[SerializeField, Tooltip("The sound type for this audio file"), SearchableEnum]
		private SoundType soundType;

		[SerializeField, Tooltip("The audio file for this sound type"), MustBeAssigned]
		private AudioClip audioClip;

		public SoundType SoundType { get => soundType; }
		public AudioClip AudioClip { get => audioClip; }
	}

	#endregion

	[SerializeField, Tooltip("The respective audio files and the sound types with it"), MustBeAssigned]
	private AudioFile[] audioFiles;

	[SerializeField, Tooltip("The audio source to play the audio files"), MustBeAssigned]
	private AudioSource audioSource;

	[SerializeField, Tooltip("The looping audio source to play the audio files")]
	private AudioSource audioSourceLooping;


	public void PlayAudioFileBySoundType(SoundType soundType) {
		foreach(var audioFile in audioFiles) {
			if(audioFile.SoundType == soundType) {
				audioSource.PlayOneShot(audioFile.AudioClip);
				break;
			}
		}
	}

	public void PlayOrChangeBGMBySoundType(SoundType soundType) {
		foreach(var audioFile in audioFiles) {
			if(audioFile.SoundType == soundType) {
				if(audioSourceLooping.isPlaying) {
					audioSourceLooping.Stop();
				}
				audioSourceLooping.PlayOneShot(audioFile.AudioClip);
				break;
			}
		}
	}
}
