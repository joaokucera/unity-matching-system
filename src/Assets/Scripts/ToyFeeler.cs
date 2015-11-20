using System.Collections;
using UnityEngine;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Toy Feeler")]
	public class ToyFeeler : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.IsGridBottom ())
			{
				GlobalVariables.CanClick = false;
				
				GlobalVariables.SoundManager.PlaySoundEffect ("Victory");
				
				GridManager.Stop();
				
				UIManager.ShowResult();
			}
		}
	}
}