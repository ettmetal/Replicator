using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Particle System Cleanup")]
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSystemCleanup : MonoBehaviour, IPooled {
		private new ParticleSystem particleSystem;

		private void Start() {
			particleSystem = GetComponent<ParticleSystem>();
		}

		public void OnRecycle() {
			particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}

		public void OnSpawn() {
			if (particleSystem.main.playOnAwake) particleSystem.Play();
		}
	}
}
