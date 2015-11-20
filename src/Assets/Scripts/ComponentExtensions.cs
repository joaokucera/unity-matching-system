using UnityEngine;

namespace MatchingSystem
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            T result = component.GetComponent<T>();

            if (result == null)
            {
                result = component.gameObject.AddComponent<T>();
            }

            return result;
        }

		public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
		{
			Color newColor = spriteRenderer.color;

			newColor.a = alpha;

			spriteRenderer.color = newColor;
		}

		public static bool IsBlock(this Collider2D collider)
		{
			return collider.CompareTag ("Block");
		}

		public static bool IsGridBottom(this Collider2D collider)
		{
			return collider.CompareTag ("GridBottom");
		}
    }
}