using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Block")]
	public class Block : MonoBehaviour
	{
		private SpriteRenderer m_spriteRenderer;
		private Rigidbody2D m_rigidbody;
		private SpriteRenderer m_bombRenderer;
		private SpriteRenderer m_fireworksRenderer;
		private SpriteRenderer m_iceRenderer;
		private List<Block> m_neighbors = new List<Block> ();

		[HideInInspector] public int Index;
		[HideInInspector] public int ColorType;
		[HideInInspector] public ClickedBehaviour ClickedBehaviour;

		public int Column { get { return Mathf.RoundToInt(transform.localPosition.x); } }
		public int Row { get { return Mathf.RoundToInt(transform.localPosition.y); } }
		public Color CurrentColor { get { return m_spriteRenderer.color; } }
		public List<Block> Neighbors 
		{
			get
			{
				m_neighbors = m_neighbors.Where(n => n != null && n.gameObject.activeInHierarchy).Distinct().ToList();

				return m_neighbors;
			}
		}
		
		void Awake()
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
			m_rigidbody = GetComponent<Rigidbody2D> ();

			m_bombRenderer = transform.FindChild ("Renderer Bomb").GetComponent<SpriteRenderer>();
			m_fireworksRenderer = transform.FindChild ("Renderer Fireworks").GetComponent<SpriteRenderer>();
			m_iceRenderer = transform.FindChild ("Renderer Ice").GetComponent<SpriteRenderer>();
		}
		
		void OnMouseDown()
		{
			if (!GlobalVariables.CanClick || gameObject.layer != GlobalVariables.DefaultLayer) return;
			
			OnClicked ();
		}

		public void Setup (int index, int type, Color color, Vector2 position, Transform parent)
		{
			Index = index;
			ColorType = type;

			m_spriteRenderer.color = color;

			transform.SetParent (parent);
			transform.localPosition = position;
		}

		public void SetClickedBehaviour (ClickedBehaviourType type)
		{
			ClickedBehaviour = ClickedBehaviourFactory.GetClickedBehaviour (type);

			ClickedBehaviour.Setup (this, m_bombRenderer, m_fireworksRenderer, m_iceRenderer);
		}
		
		public void AddNeighbors(Block neighbor)
		{
			if (!m_neighbors.Contains(neighbor)) m_neighbors.Add (neighbor);
		}
		
		public void RemoveNeighbors(Block neighbor)
		{
			m_neighbors.Remove(neighbor);
		}
		
		public void SetVisibility(bool enable)
		{
			m_spriteRenderer.enabled = enable;
		}

		public void SetAlpha(float alpha)
		{
			m_spriteRenderer.SetAlpha (alpha);

			m_bombRenderer.SetAlpha (alpha);
			m_fireworksRenderer.SetAlpha (alpha);
			m_iceRenderer.SetAlpha (alpha);
		}
		
		public void ActivateGravity()
		{
			m_rigidbody.isKinematic = false;
		}

		public bool TryReset()
		{
			bool isDestroyable = ClickedBehaviour.BehaviourType != ClickedBehaviourType.Ice;

			if (isDestroyable)
			{
				m_rigidbody.isKinematic = true;
			}
			else
			{
				SetClickedBehaviour(ClickedBehaviourType.Block);
			}

			return isDestroyable;
		}

		private void OnClicked()
		{
			GlobalVariables.CanClick = false;

			var sequence = DOTween.Sequence ();
			sequence.Append (m_spriteRenderer.DOFade (0, .1f))
					.Append (m_spriteRenderer.DOFade (1, .1f));

			AfterClick ();
		}

		private void AfterClick()
		{
			ClickedBehaviour.ExecuteBehaviour (this);
			
			bool isGameOver;
			UIManager.TryUpdateMoves (out isGameOver);
			
			if (isGameOver)
			{
				GlobalVariables.SoundManager.PlaySoundEffect ("Defeat");
				
				GridManager.Stop ();
				
				UIManager.ShowGameOver ();
			} 
			else 
			{
				GlobalVariables.CanClick = true;
			}
		}
	}
}