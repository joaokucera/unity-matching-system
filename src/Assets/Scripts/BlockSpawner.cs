using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Block Spawner")]
	[RequireComponent(typeof(BlockPooling))]
	[RequireComponent(typeof(ExplosionPooling))]
	public class BlockSpawner : MonoBehaviour 
	{
		private BlockPooling m_blockPooling;
		private ExplosionPooling m_explosionPooling;
		private float m_waitTimeToShow = 1f;

		[SerializeField] private Color[] m_colors;

		void Awake()
		{
			m_blockPooling = GetComponent<BlockPooling> ();

			m_explosionPooling = GetComponent<ExplosionPooling>();
		}

		public void SpawnNewRow()
		{
			if (!GlobalVariables.CanClick) return;

			GlobalVariables.CanClick = false;

			m_waitTimeToShow = 0;
		}

		public void StartSpawning()
		{
			StartCoroutine("SpawnBlocks");
		}

		public void StopSpawning()
		{
			StopCoroutine("SpawnBlocks");
		}

		public bool TryReturnToPool (Block block)
		{
			if (block.TryReset ())
			{
				m_explosionPooling.ExecuteParticleFromPool (block.transform.position);

				m_blockPooling.ReturnBlockToPool (block);

				return true;
			}

			return false;
		}

		public Block CreateBlock (int index, Vector2 position, Transform parent, bool notSpawned)
		{
			int type = Random.Range (0, m_colors.Length);

			return m_blockPooling.GetBlockFromPool(index, type, m_colors [type], position, parent, notSpawned);
		}

		private IEnumerator SpawnBlocks()
		{
			 var blocks = GetSpawnedBlocks ();

			for (int i = 0; i < blocks.Length; i++)
			{
				if (blocks[i].ClickedBehaviour.BehaviourType == ClickedBehaviourType.Block || blocks[i].ClickedBehaviour.BehaviourType == ClickedBehaviourType.Ice) 
				{
					blocks[i].SetVisibility(true);
				}

				yield return new WaitForSeconds (m_waitTimeToShow);
			}

			m_waitTimeToShow = 1f;

			GridManager.AddBlocks (blocks);
		}

		private Block[] GetSpawnedBlocks()
		{
			var blocks = new Block[GridManager.Columns];
			
			for (int i = 0; i < blocks.Length; i++)
			{
				var position = new Vector2(GridManager.BlockFirstPosition.x + i * GridManager.BlockSize.x, GridManager.BlockFirstPosition.y - GridManager.BlockSize.y - .1f);
				
				blocks[i] = CreateBlock(i, position, GridManager.WallTransform, false);
				blocks[i].gameObject.layer = GlobalVariables.SpawnedLayer;

				blocks[i].SetVisibility(false);
				blocks[i].SetAlpha(0.25f);
			}

			return blocks;
		}
	}
}