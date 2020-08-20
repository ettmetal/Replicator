﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Replicator {
	[CreateAssetMenu(menuName = Strings.BurstPoolMenuName, fileName = Strings.PoolFileName, order = 205)] // TODO: update order
	/// <summary>
	/// An <see cref="ObjectPool"/> which can expand on-demand to meet burst needs, and cull instances after use.
	/// </summary>
	public class BurstPool : ObjectPool {
		[SerializeField, Tooltip(Strings.BurstPoolCullMaxTooltip)]
		private int maxCulledInstances = 10;
		[SerializeField, Tooltip(Strings.BurstPoolCullIntervalTooltip)]
		private float cullInterval = 5f; // Maybe interval in frames, not time?

		private List<PooledObject> extras = new List<PooledObject>();
		private Stack<PooledObject> cullQueue = new Stack<PooledObject>();

		private CountdownTimer cullTimer;

		/// <summary>
		/// Create a new <see cref="BurstPool" />, with the given parameters.
		/// </summary>
		public static BurstPool Create(GameObject prefab, ushort capacity = 0, ushort preLoad = 0, GrowthStrategy growth = 0, int maxCulledInstances = 10, float cullInterval = 5f) {
			BurstPool newPool = CreateInstance<BurstPool>();
			newPool.Initialise(prefab, capacity, preLoad, growth);
			newPool.maxCulledInstances = maxCulledInstances;
			newPool.cullInterval = cullInterval;
			return newPool;
		}

		protected override void OnEnable() {
			base.OnEnable();
			SceneManager.sceneLoaded += onSceneLoaded;
		}

		protected override void OnDisable() {
			base.OnDisable();
			SceneManager.sceneLoaded -= onSceneLoaded;
		}

		private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
			foreach(PooledObject extra in extras) cullQueue.Push(extra);
			extras.Clear();
			cullInstances(cullQueue.Count);
			cullTimer.StopTimer();
		}

		protected override void InitialisePool() {
			base.InitialisePool();
			cullTimer = cullTimer ?? createSurrogate().AddComponent<CountdownTimer>();
			cullTimer.Target = cullInterval;
			cullTimer.Timeout += onCullTimerTimeout;
			if(cullTimer.IsRunning) cullTimer.RestartTimer();
			else cullTimer.StartTimer();
		}

		protected override bool CanGrow => true;
		protected override void AddNewObjects(int amountToAdd) {
			for(int added = 0; added < amountToAdd; added++) {
				PooledObject instance = newPooledObjectInstance();
				extras.Add(instance);
			}
		}

		protected override GameObject GetObjectToSpawn() => base.HasAvailableSpawnees() ? base.GetObjectToSpawn() : getBurstInstance();

		private protected override void reclaimRecycledObject(PooledObject recycled) {
			if(extras.Contains(recycled)) {
				extras.Remove(recycled);
				cullQueue.Push(recycled);
			}
			else {
				base.reclaimRecycledObject(recycled);
			}
		}

		private GameObject getBurstInstance() => cullQueue.Count > 0 ? reviveObjectMarkedForCull() : extras[0].gameObject;

		private static GameObject createSurrogate() => new GameObject("Timer Surrogate") {
			hideFlags = HideFlags.HideAndDontSave
		};

		private void onCullTimerTimeout() => cullInstances(maxCulledInstances);

		private void cullInstances(int cullCount) {
			int culled = 0;
			while(culled++ < cullCount && cullQueue.Count == 0) {
				Destroy(cullQueue.Pop().gameObject);
			}
		}

		private GameObject reviveObjectMarkedForCull() {
			PooledObject markedObject = cullQueue.Pop();
			extras.Add(markedObject);
			return markedObject.gameObject;
		}
	}
}
