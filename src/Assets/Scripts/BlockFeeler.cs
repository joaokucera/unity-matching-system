using System.Collections;
using UnityEngine;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Block Feeler")]
	public class BlockFeeler : MonoBehaviour 
	{
		private Block m_parentBlock;

		void Awake()
		{
			m_parentBlock = GetComponentInParent<Block> ();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.IsBlock ()) 
			{
				var block = collider.GetComponent<Block>();

				m_parentBlock.AddNeighbors(block);
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.IsBlock ())
			{
				var block = collider.GetComponent<Block>();
				
				m_parentBlock.RemoveNeighbors(block);
			}
		}
	}
}