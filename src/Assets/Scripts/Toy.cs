using UnityEngine;
using System.Collections;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Toy")]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Toy : MonoBehaviour
	{
		private SpriteRenderer m_spriteRenderer;

		[SerializeField] private Sprite[] m_sprites;

		void Awake()
		{
			m_spriteRenderer = GetComponent<SpriteRenderer> ();

			int index = Random.Range (0, m_sprites.Length);

			m_spriteRenderer.sprite = m_sprites[index];
		}
	}
}