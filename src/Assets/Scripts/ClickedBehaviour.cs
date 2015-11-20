using UnityEngine;
using DG.Tweening;
using System;

namespace MatchingSystem
{
	public enum ClickedBehaviourType
	{
		Block = 0,
		Bomb = 1,
		Firework = 2,
		Ice = 3
	}

	public abstract class ClickedBehaviour
	{
		public ClickedBehaviourType BehaviourType;
		public SpriteRenderer BehaviourRenderer;

		public abstract void Setup(Block block, params SpriteRenderer[] renderers);
		public abstract void ExecuteBehaviour(Block block);
	}

	public class BlockClickedBehaviour : ClickedBehaviour
	{
		public override void Setup (Block block, params SpriteRenderer[] renderers)
		{
			foreach (var item in renderers) item.enabled = false;
		}

		public override void ExecuteBehaviour (Block block)
		{
			GridManager.RemoveByNeighborhood(block);
		}
	}

	public class BombClickedBehaviour : ClickedBehaviour
	{
		public override void Setup (Block block, params SpriteRenderer[] renderers)
		{
			block.SetVisibility(false);
			
			foreach (var item in renderers) item.enabled = false;

			renderers [0].enabled = true;
			renderers [0].color = block.CurrentColor;
		}

		public override void ExecuteBehaviour (Block block)
		{
			GridManager.RemoveByColor (block);
		}
	}

	public class FireworkClickedBehaviour : ClickedBehaviour
	{
		public override void Setup (Block block, params SpriteRenderer[] renderers)
		{
			block.SetVisibility(false);
			
			foreach (var item in renderers) item.enabled = false;

			renderers [1].enabled = true;
			renderers [1].color = Color.white;
		}

		public override void ExecuteBehaviour (Block block)
		{
			GridManager.RemoveByColumnAboveTheRow (block);
		}
	}

	public class IceClickedBehaviour : ClickedBehaviour
	{
		public override void Setup (Block block, params SpriteRenderer[] renderers)
		{
			foreach (var item in renderers) item.enabled = false;

			renderers [2].enabled = true;
			renderers [2].color = Color.white;
		}

		public override void ExecuteBehaviour (Block block)
		{
		}
	}
}