using UnityEngine;
using System.Collections;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Menu Screen")]
	public class MenuScreen : MonoBehaviour 
	{
		public void Play()
		{
			GlobalVariables.SoundManager.PlaySoundEffect ("Click");

			Application.LoadLevel ("Game");
		}
	}
}