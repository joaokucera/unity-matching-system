using UnityEngine;
using System.Collections;

namespace MatchingSystem
{
	[AddComponentMenu("CUSTOM / Explosion Pooling")]
	public class ExplosionPooling : GenericPooling<ParticleSystem>
	{
		public void ExecuteParticleFromPool(Vector2 position)
		{
			var particle = GetObjectFromPool();

			particle.transform.localPosition = position;
			particle.Play();

			StartCoroutine (ReturnParticleToPool(particle, particle.startLifetime));
		}

		private IEnumerator ReturnParticleToPool(ParticleSystem particle, float waitTime)
		{
			yield return new WaitForSeconds (waitTime);

			ResetObject (particle);
		}
	}
}