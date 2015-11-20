using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / UI Manager")]
	public class UIManager : Singleton<UIManager> 
	{
		private int m_moves;

		[SerializeField] private RectTransform m_panelMask;

		[Header("Level UI Components")]
		[SerializeField] private RectTransform m_panelLevel;
		[SerializeField] private Text m_textGo;
		[SerializeField] private Text m_textLevel;
		[SerializeField] private Text m_textScore;
		[SerializeField] private Text m_textMoves;
		
		[Header("Result UI Components")]
		[SerializeField] private RectTransform m_panelResult;
		[SerializeField] private Text m_textLevelTitle;
		[SerializeField] private Text m_textCurrentScore;

		[Header("Game Over UI Components")]
		[SerializeField] private RectTransform m_panelGameOver;
		[SerializeField] private Text m_textBestScore;
		[SerializeField] private Text m_textFinalScore;

		void Awake()
		{
			m_moves = GlobalVariables.GameDirector.Data.CurrentMoves;

			UpdateMovesText ();
			UpdateLevelText ();
			UpdateScoreText (GlobalVariables.GameDirector.Data.CurrentScore);
		}

		public static void ShowTextGo(Action callback)
		{
			Instance.m_textGo.enabled = true;

			Instance.m_textGo.DOFade (0, 1f).SetDelay(1f).OnComplete (() => callback());
		}

		public static void UpdateScore(int score)
		{
			GlobalVariables.SoundManager.PlaySoundEffect ("Explosion");

			int previous = GlobalVariables.GameDirector.Data.CurrentScore;
			GlobalVariables.GameDirector.Data.CurrentScore += score;

			Instance.StartCoroutine (Instance.UpdateScore (previous, GlobalVariables.GameDirector.Data.CurrentScore));
		}

		public static void TryUpdateMoves(out bool isGameOver)
		{
			Instance.m_moves--;

			isGameOver = Instance.m_moves < 0;

			if (!isGameOver)
			{
				UpdateMovesText();
			}
		}

		public static void ShowResult()
		{
			Instance.m_textLevelTitle.text = string.Format ("LEVEL {0}", GlobalVariables.GameDirector.Data.CurrentLevel);
			Instance.m_textCurrentScore.text = string.Format ("Current Score <color=#FFA000FF>{0}</color>", GlobalVariables.GameDirector.Data.CurrentScore);
			
			Sequence sequence = DOTween.Sequence ();
			sequence.Append (Instance.m_panelLevel.DOScale (0, .25f))
					.Append (Instance.m_panelMask.DOScale (1, .25f))
					.Append (Instance.m_panelResult.DOScale (1, .25f));
		}
		
		public static void ShowGameOver()
		{
			GlobalVariables.GameDirector.Data.CheckScore ();

			Instance.m_textBestScore.text = string.Format ("Best Score <color=#FFC000FF>{0}</color>", GlobalVariables.GameDirector.Data.BestScore);
			Instance.m_textFinalScore.text = string.Format ("Final Score <color=#FFC000FF>{0}</color>", GlobalVariables.GameDirector.Data.CurrentScore);
			
			Sequence sequence = DOTween.Sequence ();
			sequence.Append (Instance.m_panelLevel.DOScale (0, .25f))
					.Append (Instance.m_panelMask.DOScale (1, .25f))
					.Append (Instance.m_panelGameOver.DOScale (1, .25f));
		}

		public void NewRow()
		{
			GridManager.AddNewRow ();
		}

		public void Retry()
		{
			GlobalVariables.GameDirector.Retry();
		}
		
		public void NextLevel()
		{
			GlobalVariables.GameDirector.NextLevel();
		}

		private IEnumerator UpdateScore(int previous, int total)
		{
			for (int i = previous; i < total; i += 10)
			{
				UpdateScoreText(i);

				yield return new WaitForSeconds(.1f);
			}

			UpdateScoreText(total);
		}

		private static void UpdateScoreText(int score)
		{
			Instance.m_textScore.text = string.Format("Score\n<color=#FFC000FF>{0}</color>", score);
		}

		private static void UpdateLevelText()
		{
			Instance.m_textLevel.text = string.Format("Level\n<color=#FFC000FF>{0}</color>", GlobalVariables.GameDirector.Data.CurrentLevel);
		}

		private static void UpdateMovesText()
		{
			Instance.m_textMoves.text = string.Format ("Moves\n<color=#FFC000FF>{0}</color>", Instance.m_moves);
		}
	}
}