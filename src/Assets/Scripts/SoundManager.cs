using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Sound Manager")]
	public class SoundManager : MonoBehaviour
	{
		private static bool m_loaded;

		[Header("Sources")]
		[SerializeField] private AudioSource m_musicSource;
		[SerializeField] private AudioSource m_sfxSource;

		[Header("Sound Effects Collection")]
		[SerializeField] private AudioClip[] sfxClips;
		
		private Dictionary<string, AudioClip> m_soundDictionary = new Dictionary<string, AudioClip>();

		void Awake()
		{
			if (!m_loaded)
			{
				m_loaded = true;
				
				DontDestroyOnLoad(gameObject);

				CreateSoundDictionary();
				
				StartCoroutine(PlayMusic());
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void PlaySoundEffect(string clipName)
		{
			AudioClip originalClip;
			
			if (m_soundDictionary.TryGetValue(clipName, out originalClip))
			{
				MakeSoundEffect(originalClip);
			}
		}
		
		private void CreateSoundDictionary()
		{
			for (int i = 0; i < sfxClips.Length; i++)
			{
				m_soundDictionary.Add(sfxClips[i].name, sfxClips[i]);
			}
		}
		
		private IEnumerator PlayMusic()
		{
			m_musicSource.volume = 0f;
			m_musicSource.loop = true;
			m_musicSource.Play();
			
			while (m_musicSource.volume < 0.5f)
			{
				m_musicSource.volume += Time.deltaTime;
				
				yield return null;
			}
		}
		
		private void MakeSoundEffect(AudioClip originalClip)
		{
			m_sfxSource.PlayOneShot(originalClip);
		}
	}
}