using UnityEngine;

namespace Replicator {
	[AddComponentMenu("Pooling/Rigidbody2D Cleanup")]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Rigidbody2DCleanup : MonoBehaviour, IRecycled {
		private Rigidbody2D rigidbody2d;

		private void Start() {
			rigidbody2d = GetComponent<Rigidbody2D>();
		}
		public void OnRecycle() {
			rigidbody2d.velocity = Vector2.zero;
			rigidbody2d.angularVelocity = 0f;
		}
	}
}
