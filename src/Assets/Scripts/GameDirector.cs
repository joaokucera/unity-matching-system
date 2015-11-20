using System;
using System.Collections;
using UnityEngine;

namespace MatchingSystem
{
	[Serializable]
	public class GameData
	{
		private const int NoBlockBehaviourProbability = 10;
		private const int StartMoves = 60;
		private const int StartMinRows = 4;
		private const int StartMaxRows = 8;

		public int CurrentNoBlockBehaviourProbability = NoBlockBehaviourProbability;
		public int CurrentMoves = StartMoves;
		public int CurrentMinRows = StartMinRows;
		public int CurrentMaxRows = StartMaxRows;

		public int CurrentLevel = 1;
		public int BestScore = 0;
		public int CurrentScore = 0;

		public void Reset()
		{
			CurrentNoBlockBehaviourProbability = NoBlockBehaviourProbability;
			CurrentMoves = StartMoves;
			CurrentMinRows = StartMinRows;
			CurrentMaxRows = StartMaxRows;
			
			CurrentLevel = 1;
			CurrentScore = 0;
		}

		public void Apply()
		{
			CurrentNoBlockBehaviourProbability++;
			CurrentMoves--;
			CurrentMinRows++;
			CurrentMaxRows++;
			
			CurrentLevel++;
		}

		public void CheckScore()
		{
			if (CurrentLevel > BestScore)
			{
				BestScore = CurrentScore;
			}
		}
	}

	[AddComponentMenu("CUSTOM / Game Director")]
	public class GameDirector : Singleton<GameDirector>
	{
		private static bool m_loaded;

		[HideInInspector] public GameData Data;

		void Awake()
		{
			if (!m_loaded)
			{
				m_loaded = true;
				
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void NextLevel()
		{
			Data.Apply ();

			LoadGame ();
		}

		public void Retry()
		{
			Data.Reset ();

			LoadGame ();
		}
		
		private void LoadGame()
		{
			Application.LoadLevel ("Game");
		}
	}
}