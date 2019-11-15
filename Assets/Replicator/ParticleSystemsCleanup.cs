using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Particle System Cleanup (Mutli)")]
	public class ParticleSystemsCleanup : MonoBehaviour, IPooled {
		private ParticleSystem[] particleSystems;

		private void Start() {
			particleSystems = GetComponentsInChildren<ParticleSystem>();
		}

		public void OnRecycle() {
			foreach(ParticleSystem particles in particleSystems) {
				particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}

		public void OnSpawn() {
			foreach(ParticleSystem particles in particleSystems) {
				if(particles.main.playOnAwake) particles.Play();
			}
		}
	}
}
