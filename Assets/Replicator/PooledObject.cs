using UnityEngine;

namespace Replicator {
	[AddComponentMenu("")] // Prevents this Component from appearing in the Unity editor.
	internal class PooledObject : MonoBehaviour
#if UNITY_EDITOR
	, IPooled // Interface used to manage HideFlags, no use outside editor
#endif
	{
		private ObjectPool owner;
		private bool recycleFlag;

#region HideInHierarchy
#if UNITY_EDITOR
		private void Awake() {
			hideFlags = HideFlags.HideInInspector;
			if(owner.HideUnspawned) gameObject.hideFlags |= HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}

		public void OnSpawn() {
			if(owner.HideUnspawned) gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}

		public void OnRecycle() {
			if(owner.HideUnspawned) gameObject.hideFlags |= HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}
#endif
#endregion

		private void LateUpdate() {
			if(recycleFlag) {
				recycleFlag = false;
				owner.Recycle(this);
			}
		}

		public void SetOwner(ObjectPool newOwner) {
			if(owner == null) {
				owner = newOwner;
				PoolRegistry.Pools.Add(gameObject, owner);
				owner.OnDisablePool += deregisterInstance;
			}
			else {
				Debug.Log(Strings.SetOwnerOnOwned);
			}
		}

		private void deregisterInstance() {
			PoolRegistry.Pools.Remove(gameObject);
			owner.OnDisablePool -= deregisterInstance;
		}

		public bool BelongsTo(ObjectPool pool) => owner == pool;

		public void Recycle() => recycleFlag = true;

		public GameObject Spawn() => owner.Spawn(transform.position, Quaternion.identity);
	}
}
