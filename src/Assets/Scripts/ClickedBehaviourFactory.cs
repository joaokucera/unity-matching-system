using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatchingSystem
{
	public static class ClickedBehaviourFactory 
	{
		public static ClickedBehaviour GetClickedBehaviour(ClickedBehaviourType type)
		{
			ClickedBehaviour item = null;

			switch (type)
			{
				case ClickedBehaviourType.Block:
					item = new BlockClickedBehaviour();
					break;
				case ClickedBehaviourType.Bomb:
					item = new BombClickedBehaviour();
					break;
				case ClickedBehaviourType.Firework:
					item = new FireworkClickedBehaviour();
					break;
				case ClickedBehaviourType.Ice:
					item = new IceClickedBehaviour();
					break;
			}

			item.BehaviourType = type;

			return item;
		}
	}
}