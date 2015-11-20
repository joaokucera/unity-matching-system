using UnityEngine;
using System.Collections;
using System;

namespace MatchingSystem
{
	public static class GlobalVariables
	{
		public static bool CanClick;

		private static Camera m_mainCamera;
		public static Camera MainCamera 
		{
			get 
			{
				if (m_mainCamera == null) m_mainCamera = Camera.main;

				return m_mainCamera;
			}
		}

		public static GameDirector m_gameDirector;
		public static GameDirector GameDirector
		{
			get
			{
				if (m_gameDirector == null)
				{
					m_gameDirector = GameObject.FindObjectOfType<GameDirector>();
				}

				return m_gameDirector;
			}
		}

		public static SoundManager m_soundManager;
		public static SoundManager SoundManager
		{
			get
			{
				if (m_soundManager == null)
				{
					m_soundManager = GameObject.FindObjectOfType<SoundManager>();
				}
				
				return m_soundManager;
			}
		}

		private static int m_defaultLayer;
		public static int DefaultLayer
		{
			get
			{
				if (m_defaultLayer == 0) m_defaultLayer = LayerMask.NameToLayer ("Default");
			
				return m_defaultLayer;
			}
		}

		private static int m_spawnedLayer;
		public static int SpawnedLayer
		{
			get
			{

				if (m_spawnedLayer == 0) m_spawnedLayer = LayerMask.NameToLayer ("Spawned");

				return m_spawnedLayer;
			}
		}

		public static Array m_clickedBehaviourTypeValues;
		public static Array ClickedBehaviourTypeValues
		{
			get
			{
				if (m_clickedBehaviourTypeValues == null) m_clickedBehaviourTypeValues = Enum.GetValues (typeof(ClickedBehaviourType));
				
				return m_clickedBehaviourTypeValues;
			}
		}
	}
}