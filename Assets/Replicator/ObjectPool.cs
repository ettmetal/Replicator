using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Replicator {
	/// <summary>Asset representing and providing a pool of GameObjects.</summary>
	[CreateAssetMenu(menuName = Strings.PoolMenuName, fileName = Strings.PoolFileName, order = 203)]
	public class ObjectPool : ScriptableObject {
		[SerializeField, Tooltip(Strings.PrefabTooltip)]
		private GameObject prefab = null;
		[SerializeField, Tooltip(Strings.CapacityTooltip)]
		private ushort capacity = 0;
		[SerializeField, Tooltip(Strings.PreLoadTooltip)]
		private ushort preLoad = ushort.MaxValue; // Using this is a bit of a hack to signify 'unedited' but negates the need for another serialized field
		[SerializeField, Tooltip(Strings.GrowTooltip)]
		private GrowthStrategy growth = GrowthStrategy.None;
		[SerializeField, Tooltip(Strings.HideUnspawedTooltip)]
		internal bool HideUnspawned = true;
		private ushort activeObjectCount;
		private Stack<PooledObject> pool;
		internal event Action OnDisablePool;

		protected virtual bool CanSpawn => activeObjectCount + pool.Count < capacity;
		protected virtual bool CanGrow => growth != GrowthStrategy.None;

		/// <summary>
		/// Create a new <see cref="ObjectPool" /> with the given parameters.
		/// </summary>
		public static ObjectPool Create(GameObject prefab, ushort capacity = 0, ushort preLoad = 0, GrowthStrategy growth = 0) {
			ObjectPool newPool = CreateInstance<ObjectPool>();
			newPool.Initialise(prefab, capacity, preLoad, growth);
			newPool.OnEnable();
			return newPool;
		}

		internal virtual void Initialise(GameObject prefab, ushort capacity, ushort preLoad, GrowthStrategy growth) {
			this.prefab = prefab;
			this.capacity = capacity;
			this.preLoad = preLoad;
			this.growth = growth;
		}

		protected virtual void OnEnable() { // Check if can be initialised, work around
			if(prefab == null) return;
			preLoad = preLoad == ushort.MaxValue ? capacity : preLoad;
			InitialisePool();
			RegisterSelf();
			SceneManager.sceneLoaded += onSceneLoaded;
		}

		protected virtual void OnDisable() {
			DeregisterSelf();
			OnDisablePool?.Invoke();
		}

		protected virtual void OnValidate() => preLoad = capacity > preLoad ? preLoad : capacity;

		private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
			if(preLoad > 0) AddNewObjects(preLoad);
		}

		/// <summary>Spawn a GameObject from the pool, if one is available.</summary>
		/// <param name="position">Position of the spawned GameObject</param>
		/// <param name="rotation">Rotation of the spawned GameObject</param>
		/// <param name="parent">(optional) Parent of the spawned GameObject</param>
		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null) {
			if(CanSpawn && !HasAvailableSpawnees()) Expand(GrowthStrategy.Single);
			else if(CanGrow && !CanSpawn) Expand(growth);
			GameObject spawned = GetObjectToSpawn();
			if(spawned == null) {
				Debug.Log(Strings.UnableToSpawn);
				return null;
			}

			spawned.transform.SetParent(parent);
			spawned.transform.position = position;
			spawned.transform.rotation = rotation;

			spawned.gameObject.SetActive(true);
			TriggerSpawnHandlers(spawned);
			activeObjectCount++;
			return spawned.gameObject;
		}

		/// <summary>Recycle the <paramref name="gameObject"/> if it belongs to this pool.</summary>
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
				resetPooledObject(pooledObject);
				TriggerRecycleHandlers(pooledObject.gameObject);
				reclaimRecycledObject(pooledObject);
				activeObjectCount--;
			}
		}

		private protected virtual void reclaimRecycledObject(PooledObject recycled) => pool.Push(recycled);

		protected virtual void InitialisePool() => pool = new Stack<PooledObject>();

		protected virtual void RegisterSelf() {
			if(prefab != null) PoolRegistry.Pools.Add(prefab, this);
		}

		protected virtual void DeregisterSelf() {
			if(prefab != null) PoolRegistry.Pools.Remove(prefab);
		}

		protected virtual bool HasAvailableSpawnees() => pool.Count > 0;

		protected virtual void Expand(GrowthStrategy strategy) {
			int growAmount = 0;
			if(strategy == GrowthStrategy.Single) growAmount = 1;
			else if(strategy == GrowthStrategy.Tenth) growAmount = capacity / 10;
			else if(strategy == GrowthStrategy.Quarter) growAmount = capacity / 4;
			else if(strategy == GrowthStrategy.Half) growAmount = capacity / 2;
			else if(strategy == GrowthStrategy.Double) growAmount = capacity * 2;
			AddNewObjects(growAmount);
		}

		protected virtual GameObject GetObjectToSpawn() => pool.Pop().gameObject;

		protected virtual void AddNewObjects(int amountToAdd) {
			for(int i = 0; i < Mathf.Min(amountToAdd, capacity); i++) {
				pool.Push(newPooledObjectInstance());
			}
		}

		private protected PooledObject newPooledObjectInstance() {
			GameObject instance = InstantiateInactive(prefab);
			return instance.GetComponent<PooledObject>();
		}

		protected static void TriggerSpawnHandlers(GameObject target) {
			ISpawned[] spawnHandlers = target.GetComponentsInChildren<ISpawned>();
			foreach(ISpawned spawnHandler in spawnHandlers) {
				spawnHandler.OnSpawn();
			}
		}

		protected static void TriggerRecycleHandlers(GameObject target) {
			IRecycled[] recycleHandlers = target.GetComponentsInChildren<IRecycled>();
			foreach(IRecycled recycleHandler in recycleHandlers) {
				recycleHandler.OnRecycle();
			}
		}

		protected GameObject InstantiateInactive(GameObject source) {
			GameObject instance = Instantiate(source);
			instance.SetActive(false);
#if UNITY_EDITOR
			if(HideUnspawned) instance.hideFlags |= HideFlags.HideInHierarchy;
#endif
			PooledObject pooledObject = instance.GetComponent<PooledObject>() ?? instance.AddComponent<PooledObject>();
			pooledObject.SetOwner(this);
			return instance;
		}

		private static void resetPooledObject(PooledObject pooledObject) {
			pooledObject.gameObject.SetActive(false);
			pooledObject.transform.SetParent(null);
		}

		private static void logUnableToRecycle(string reason) => Debug.LogFormat(Strings.CantRecycleFormat, reason);
	}
}
