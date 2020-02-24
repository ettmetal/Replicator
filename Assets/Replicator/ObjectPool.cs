#pragma warning disable 649 // Prevent field not initialized warnings

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Replicator {
	/// <summary>
	/// Asset representing and providing a pool of GameObjects.
	/// </summary>
	[CreateAssetMenu(menuName = Strings.PoolMenuName, fileName = Strings.PoolFileName, order = 203)]
	public class ObjectPool : ScriptableObject {
		[SerializeField, Tooltip(Strings.PrefabTooltip)]
		private GameObject prefab;
		[SerializeField, Tooltip(Strings.CapacityTooltip)]
		private ushort capacity;
		[SerializeField, Tooltip(Strings.PreLoadTooltip)]
		private ushort preLoad;
		[SerializeField, Tooltip(Strings.GrowTooltip)]
		private bool grow;
		[SerializeField, Tooltip(Strings.HideUnspawedTooltip)]
		internal bool hideUnspawned = true;
		private ushort activeObjectCount;
		private Stack<PooledObject> pool;
		internal event Action OnDisablePool;

		private void OnEnable() {
			initialisePool();
			registerSelf();
			SceneManager.sceneLoaded += onSceneLoaded;
		}

		private void OnDisable() {
			deregisterSelf();
			OnDisablePool?.Invoke();
		}

		private void OnValidate() => preLoad = (ushort)Mathf.Min(preLoad, capacity);

		private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
			if(preLoad > 0) preloadObjects(preLoad);
		}

		/// <summary>
		/// Spawn a GameObject from the pool, if one is available.
		/// </summary>
		/// <param name="position">Position of the spawned GameObject</param>
		/// <param name="rotation">Rotation of the spawned GameObject</param>
		/// <param name="parent">(optional) Parent of the spawned GameObject</param>
		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null) {
			if(grow && !hasAvailableSpawnees()) expand();
			GameObject spawned = getObjectToSpawn();
			if(spawned == null) {
				Debug.Log(Strings.UnableToSpawn);
				return null;
			}

			spawned.transform.SetParent(parent);
			spawned.transform.position = position;
			spawned.transform.rotation = rotation;

			spawned.gameObject.SetActive(true);
			triggerSpawnHandlers(spawned.gameObject);
			activeObjectCount++;
			return spawned.gameObject;
		}

		/// <summary>
		/// Recycle the <paramref name="gameObject"/> if it belongs to this pool.
		/// </summary>
		public void Recycle(GameObject gameObject) {
			PooledObject pooledObject = gameObject.GetComponent<PooledObject>();
			if(pooledObject == null) {
				logUnableToRecycle(Strings.NotPooled);
			}
			else {
				Recycle(pooledObject);
			}
		}

		internal void Recycle(PooledObject pooledObject) {
			if(!pooledObject.BelongsTo(this)) {
				throw new InvalidOperationException(Strings.NotInPool);
			}
			else {
				reclaimPooledObject(pooledObject);
				triggerRecycleHandlers(pooledObject.gameObject);
				pool.Push(pooledObject);
				activeObjectCount--;
			}
		}

		protected virtual void initialisePool() => pool = new Stack<PooledObject>();

		protected virtual void registerSelf() {
			if(prefab != null) PoolRegistry.pools.Add(prefab, this);
		}

		protected virtual void deregisterSelf() {
			if(prefab != null) PoolRegistry.pools.Remove(prefab);
		}

		protected virtual bool hasAvailableSpawnees() => pool.Count > 0;

		protected virtual void expand() => pool.Push(newPooledObjectInstance());

		protected virtual GameObject getObjectToSpawn() => pool.Pop().gameObject;

		protected virtual void preloadObjects(int amountToPreload) {
			for(int i = 0; i < Mathf.Min(amountToPreload, capacity); i++) {
				pool.Push(newPooledObjectInstance());
			}
		}

		private PooledObject newPooledObjectInstance() {
			GameObject instance = instantiateInactive(prefab);
			return instance.GetComponent<PooledObject>();
		}

		protected static void triggerSpawnHandlers(GameObject target) {
			ISpawned[] spawnHandlers = target.GetComponentsInChildren<ISpawned>();
			foreach(ISpawned spawnHandler in spawnHandlers) {
				spawnHandler.OnSpawn();
			}
		}

		protected static void triggerRecycleHandlers(GameObject target) {
			IRecycled[] recycleHandlers = target.GetComponentsInChildren<IRecycled>();
			foreach(IRecycled recycleHandler in recycleHandlers) {
				recycleHandler.OnRecycle();
			}
		}

		protected GameObject instantiateInactive(GameObject source) {
			GameObject instance = Instantiate(source);
			instance.SetActive(false);
#if UNITY_EDITOR
			if(hideUnspawned) instance.hideFlags |= HideFlags.HideInHierarchy;
#endif
			PooledObject pooledObject = instance.GetComponent<PooledObject>() ?? instance.AddComponent<PooledObject>();
			pooledObject.SetOwner(this);
			return instance;
		}

		private static void reclaimPooledObject(PooledObject pooledObject) {
			pooledObject.gameObject.SetActive(false);
			pooledObject.transform.SetParent(null);
		}

		private static void logUnableToRecycle(string reason) => Debug.LogFormat(Strings.CantRecycleFormat, reason);
	}
}
