using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Particle System Cleanup (Mutli)")]
	public class ParticleSystemsCleanup : MonoBehaviour, IPooled {
		[SerializeField, Tooltip(Strings.ParticlesResetTooltip)]
		private ParticleSystemStopBehavior resetBehaviour = ParticleSystemStopBehavior.StopEmittingAndClear;
		private ParticleSystem[] particleSystems;

		private void Start() {
			particleSystems = GetComponentsInChildren<ParticleSystem>();
		}

		public void OnRecycle() {
			foreach(ParticleSystem particles in particleSystems) {
				particles.Stop(true, resetBehaviour);
				particles.time = 0;
			}
		}

		public void OnSpawn() {
			foreach(ParticleSystem particles in particleSystems) {
				if(particles.main.playOnAwake) particles.Play();
			}
		}
	}
}
