#pragma warning disable 649 // Prevent field not initialized warnings

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Replicator {
	[CreateAssetMenu(menuName = Strings.PoolMenuName, fileName = Strings.PoolFileName, order = 203)]
	public class ObjectPool : ScriptableObject {
		[SerializeField, Tooltip(Strings.PrefabTooltip)]
		private GameObject prefab;
		[SerializeField, Tooltip(Strings.PreLoadTooltip)]
		private bool preLoad;
		[SerializeField, Tooltip(Strings.CapacityTooltip)]
		private int capacity;
		[SerializeField, Tooltip(Strings.GrowTooltip)]
		private bool grow;
		private int activeObjectCount;

		private Stack<PooledObject> pool;

		internal event Action OnDisablePool;

		private void OnEnable() {
			pool = new Stack<PooledObject>();
			GameObjectExtensions.poolRegistry.Add(prefab, this);
			SceneManager.sceneLoaded += onSceneLoaded;
		}

		private void OnDisable() {
			GameObjectExtensions.poolRegistry.Remove(prefab);
			OnDisablePool?.Invoke();
		}

		private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
			if(preLoad) {
				preloadObjects();
			}
		}

		/// <summary>
		/// Spawn a GameObject from the pool, if one is available.
		/// </summary>
		/// <param name="position">Position of the spawned GameObject</param>
		/// <param name="rotation">Rotation of the spawned GameObject</param>
		/// <param name="parent">(optional) Parent of the spawned GameObject</param>
		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null) {
			if(pool.Count < 1 && grow) {
				pool.Push(newPooledObjectInstance());
			}
			PooledObject spawned = pool.Pop();
			if(spawned == null) {
				Debug.Log(Strings.UnableToSpawn);
				return null;
			}

			spawned.transform.SetParent(parent);
			spawned.transform.position = position;
			spawned.transform.rotation = rotation;

			spawned.gameObject.SetActive(true);
			ISpawned[] spawnHandlers = spawned.GetComponentsInChildren<ISpawned>();
			foreach(ISpawned spawnHandler in spawnHandlers) {
				spawnHandler.OnSpawn();
			}
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
				IRecycled[] recycleHandlers = pooledObject.gameObject.GetComponentsInChildren<IRecycled>();
				foreach(IRecycled recycleHandler in recycleHandlers) {
					recycleHandler.OnRecycle();
				}
				pool.Push(pooledObject);
				activeObjectCount--;
			}
		}

		private void preloadObjects() {
			for(int i = 0; i < capacity; i++) {
				pool.Push(newPooledObjectInstance());
			}
		}

		private PooledObject newPooledObjectInstance() {
			GameObject instance = instantiateInactive(prefab);
			PooledObject pooledObject = instance.GetOrAddComponent<PooledObject>();
			pooledObject.SetOwner(this);
			return pooledObject;
		}

		private static GameObject instantiateInactive(GameObject source) {
			GameObject instance = Instantiate(source);
			instance.SetActive(false);
			instance.hideFlags = HideFlags.HideInHierarchy;
			return instance;
		}

		private static void reclaimPooledObject(PooledObject pooledObject) {
			pooledObject.gameObject.SetActive(false);
			pooledObject.gameObject.hideFlags = HideFlags.HideInHierarchy;
			pooledObject.transform.SetParent(null);
		}

		private static void logUnableToRecycle(string reason) {
			Debug.LogFormat(Strings.CantRecycleFormat, reason);
		}
	}
}
