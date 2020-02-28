using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Particle System Cleanup")]
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSystemCleanup : MonoBehaviour, IPooled {
		[SerializeField, Tooltip(Strings.ParticlesResetTooltip)]
		private ParticleSystemStopBehavior resetBehaviour = ParticleSystemStopBehavior.StopEmittingAndClear;
		private new ParticleSystem particleSystem;

		private void Start() {
			particleSystem = GetComponent<ParticleSystem>();
		}

		public void OnRecycle() {
			particleSystem.Stop(true, resetBehaviour);
			particleSystem.time = 0f;
		}

		public void OnSpawn() {
			if (particleSystem.main.playOnAwake) particleSystem.Play();
		}
	}
}
