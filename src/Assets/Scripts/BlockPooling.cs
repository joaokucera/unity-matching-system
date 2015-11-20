using UnityEngine;
using System.Collections;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Block Pooling")]
	public class BlockPooling : GenericPooling<Block>
	{
		private bool[] m_probabilityArray = new bool[100];

		void Awake()
		{
			CreateProbabilityArray ();
		}

		public Block GetBlockFromPool(int index, int type, Color color, Vector2 position, Transform parent, bool notSpawned)
		{
			var block = GetObjectFromPool();

			block.Setup (index, type, color, position, parent);

			block.SetClickedBehaviour ((ClickedBehaviourType)GetValue(notSpawned));

			return block;
		}

		public void ReturnBlockToPool(Block block)
		{
			ResetObject (block);
		}

		/// <summary>
		/// Randomize a value between 1 and 5 (values from ClickedBehaviourType enum) if probability is TRUE, otherwise pick 0.
		/// </summary>
		private int GetValue(bool notSpawned)
		{
			bool probability = m_probabilityArray [Random.Range (0, m_probabilityArray.Length)];

			int value = notSpawned && probability ? Random.Range(1, GlobalVariables.ClickedBehaviourTypeValues.Length) : 0;

			return value;
		}
		
		private void CreateProbabilityArray ()
		{
			int i = 0;
			for (; i < GlobalVariables.GameDirector.Data.CurrentNoBlockBehaviourProbability; i++)
			{
				m_probabilityArray [i] = true;
			}
			for (; i < m_probabilityArray.Length; i++)
			{
				m_probabilityArray [i] = false;
			}
		}
	}
}