using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Rigidbody Cleanup")]
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyCleanup : MonoBehaviour, IRecycled {
		private new Rigidbody rigidbody;

		private void Start() {
			rigidbody = GetComponent<Rigidbody>();
		}
		public void OnRecycle() {
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}
