using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	// TODO: Cleanup on scene change
	[CreateAssetMenu(menuName = Strings.BurstPoolMenuName, fileName = Strings.PoolFileName, order = 0)] // TODO: update order
	/// <summary>
	/// An <see cref="ObjectPool"/> which can expand on-demand to meet burst needs, and cull instances after use.
	/// </summary>
	public class BurstPool : ObjectPool {
		[SerializeField, Tooltip(Strings.BurstPoolCullMaxTooltip)]
		private int maxCulledInstances = 10;
		[SerializeField, Tooltip(Strings.BurstPoolCullIntervalTooltip)]
		private float cullInterval = 5f; // Maybe interval in frames, not time?

		private HashSet<PooledObject> extras = new HashSet<PooledObject>();
		private Stack<PooledObject> cullQueue = new Stack<PooledObject>();

		private CountdownTimer cullTimer;

		protected override void initialisePool() {
			base.initialisePool();
			extras = new HashSet<PooledObject>();
			cullTimer = cullTimer ?? createSurrogate().AddComponent<CountdownTimer>();
			cullTimer.Target = cullInterval;
			cullTimer.Timeout += onCullTimerTimeout;
			if(cullTimer.IsRunning) cullTimer.RestartTimer();
			else cullTimer.StartTimer();
		}

		protected override bool canGrow => true;
		protected override void addNewObjects(int amountToAdd) {
			for(int added = 0; added < amountToAdd; added++) {
				PooledObject instance = newPooledObjectInstance();
				extras.Add(instance);
			}
		}

		protected override GameObject getObjectToSpawn() => base.hasAvailableSpawnees() ? base.getObjectToSpawn() : getBurstInstance();

		private protected override void reclaimRecycledObject(PooledObject recycled) {
			if(extras.Contains(recycled)) {
				extras.Remove(recycled);
				cullQueue.Push(recycled);
			}
			else base.reclaimRecycledObject(recycled);
		}

		private GameObject getBurstInstance() => cullQueue.Count > 0 ? cullQueue.Pop().gameObject : extras[0]; // TODO: get available extra without breaking reclaimRecycledObject

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

		private PooledObject reviveRecycledInstance() {
			foreach(PooledObject instance in extras) {
				if(instance.gameObject.activeSelf) return instance;
			}
			return null;
		}
	}
}
