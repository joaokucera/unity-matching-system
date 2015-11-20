using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Grid Manager")]
	[RequireComponent(typeof(BlockSpawner))]
	public class GridManager : Singleton<GridManager> 
	{
		private BlockSpawner m_spawner;
		private List<Block>[] m_blocks;
		private int m_colums = 12;
		private int m_amountToMatch = 3;
		private Vector2 m_blockSize;
		private Vector2 m_blockFirstPosition;

		[SerializeField] private Toy m_toyPrefab;

		[Header("Grid Settings")]
		[SerializeField] private Transform m_gridTransform;
		[SerializeField] private Collider2D m_gridBottomCollider;

		[Header("Block Settings")]
		[SerializeField] private Vector2 m_blockScale;
		[SerializeField] private Sprite m_blockSprite;

		public static int Columns { get { return Instance.m_colums; } }
		public static Transform WallTransform { get { return Instance.m_gridTransform; } }
		public static Vector2 BlockSize { get { return Instance.m_blockSize; } }
		public static Vector2 BlockFirstPosition { get { return Instance.m_blockFirstPosition; } }

		void Start()
		{
			m_spawner = GetComponent<BlockSpawner> ();

			InitializeBlocks ();
		}

		public static void Stop()
		{
			Instance.m_spawner.StopSpawning ();
		}

		public static void AddBlocks (Block[] newBlocks)
		{
			Instance.m_gridBottomCollider.transform.position = new Vector2 (0, Instance.m_gridBottomCollider.transform.position.y - Instance.m_blockSize.y - .1f);

			Instance.m_gridBottomCollider.transform
				.DOMoveY (Instance.m_blockFirstPosition.y - .1f, .1f)
				.OnStart (() => 
				{
					for (int i = 0; i < newBlocks.Length; i++)
					{
						newBlocks[i].SetAlpha(1f);
						newBlocks[i].ActivateGravity();
						newBlocks[i].gameObject.layer = GlobalVariables.DefaultLayer;
						
						int index = newBlocks[i].Index;
						
						Instance.m_blocks[index].Add(newBlocks[i]);
					}
				})
				.OnComplete (() =>
				{
					Instance.m_spawner.StartSpawning ();

					GlobalVariables.CanClick = true;
				});
		}

		public static void AddNewRow ()
		{
			Instance.m_spawner.SpawnNewRow ();
		}

		public static void RemoveByNeighborhood (Block block)
		{
			var queue = new Queue<Block> ();
			queue.Enqueue (block);

			var neighbors = block.Neighbors.Where (n => n.gameObject.layer == GlobalVariables.DefaultLayer && 
			                                       n.ClickedBehaviour.BehaviourType == ClickedBehaviourType.Block || n.ClickedBehaviour.BehaviourType == ClickedBehaviourType.Ice);

			foreach (Block b in neighbors)
			{
				Instance.CheckNeighbors(b, block.ColorType, block.Column, block.Row, ref queue);
			}

			Instance.CheckToRemoveBlocks (queue);
		}

		public static void RemoveByColor (Block block)
		{
			var queue = new Queue<Block> ();
			queue.Enqueue (block);

			for (int x = 0; x < Instance.m_blocks.Length; x++)
			{
				for (int y = 0; y < Instance.m_blocks[x].Count; y++)
				{
					if (Instance.m_blocks[x][y].ColorType == block.ColorType && 
					    Instance.m_blocks[x][y].ClickedBehaviour.BehaviourType != ClickedBehaviourType.Firework)
					{
						queue.Enqueue (Instance.m_blocks[x][y]);
					}
				}
			}

			Instance.CheckToRemoveBlocks (queue, true);
		}

		public static void RemoveByColumnAboveTheRow (Block block)
		{
			var queue = new Queue<Block> ();
			queue.Enqueue (block);

			for (int y = 0; y < Instance.m_blocks [block.Index].Count; y++)
			{
				if (Instance.m_blocks [block.Index][y].Row > block.Row)
				{
					queue.Enqueue(Instance.m_blocks [block.Index][y]);
				}
			}

			Instance.CheckToRemoveBlocks (queue, true);
		}

		private void CheckNeighbors(Block block, int type, int column, int row, ref Queue<Block> queue)
		{
			if (block == null || block.ColorType != type || queue.Contains(block)) return;

			if (block.Column == column || block.Row == row)
			{
				queue.Enqueue (block);

				type = block.ColorType;
				column = block.Column;
				row = block.Row;

				var neighbors = block.Neighbors.Where (n => n.gameObject.layer == GlobalVariables.DefaultLayer && 
				                                       n.ClickedBehaviour.BehaviourType == ClickedBehaviourType.Block || n.ClickedBehaviour.BehaviourType == ClickedBehaviourType.Ice);

				foreach (Block b in neighbors)
				{
					CheckNeighbors(b, type, column, row, ref queue);
				}
			}
		}
		
		private void CheckToRemoveBlocks (Queue<Block> queue, bool forceRemove = false)
		{
			if (queue.Count >= Instance.m_amountToMatch || forceRemove)
			{
				UIManager.UpdateScore (queue.Count);
				
				while (queue.Count > 0) 
				{
					var item = queue.Dequeue ();
					var index = item.Index;

					if (Instance.m_spawner.TryReturnToPool (item))
					{
						Instance.m_blocks [index].Remove (item);
					}
				}
				
				ScreenShake.Shake ();
			}
		}

		private void InitializeBlocks()
		{
			m_blockSize = new Vector2(m_blockSprite.bounds.size.x * m_blockScale.x, m_blockSprite.bounds.size.y * m_blockScale.y);
			m_blockFirstPosition = new Vector2 (-m_blockSize.x * m_colums / 2 + m_blockSize.x / 2, -m_blockSize.y * GlobalVariables.GameDirector.Data.CurrentMaxRows / 2 + m_blockSize.y / 2);

			m_blocks = new List<Block>[m_colums];

			for (int x = 0; x < m_colums; x++)
			{
				m_blocks[x] = new List<Block>();

				int rows = Random.Range(GlobalVariables.GameDirector.Data.CurrentMinRows, GlobalVariables.GameDirector.Data.CurrentMaxRows);

				for (int y = 0; y < rows; y++)
				{
					var position = new Vector2(m_blockFirstPosition.x + x * m_blockSize.x, m_blockFirstPosition.y + y * m_blockSize.y);

					var block = m_spawner.CreateBlock(x, position, m_gridTransform, true);

					m_blocks[x].Add(block);
				}
			}

			m_gridTransform.position = new Vector2 (0, -GlobalVariables.MainCamera.orthographicSize * 2);
			m_gridTransform
				.DOMoveY (m_blockSize.y, 1f)
				.OnComplete(() => UIManager.ShowTextGo(ReadyBlocks));
		}

		private void ReadyBlocks()
		{
			InstantiateToy ();

			m_gridBottomCollider.transform.position = new Vector2 (0, m_blockFirstPosition.y - .1f);
			m_gridBottomCollider.isTrigger = false;

			for (int x = 0; x < m_blocks.Length; x++) 
			{
				for (int y = 0; y < m_blocks[x].Count; y++) 
				{
					m_blocks[x][y].ActivateGravity();
				}
			}

			m_spawner.StartSpawning ();

			GlobalVariables.CanClick = true;
		}

		private void InstantiateToy()
		{
			int index = Random.Range (0, m_colums);

			var position = new Vector2(m_blocks[index][0].transform.position.x, GlobalVariables.MainCamera.orthographicSize);

			Instantiate (m_toyPrefab, position, Quaternion.identity);
		}
	}
}