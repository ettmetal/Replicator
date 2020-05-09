using System.Collections.Generic;
using UnityEngine;

namespace Replicator {

	[CreateAssetMenu(menuName = Strings.BurstPoolMenuName, fileName = Strings.PoolFileName, order = 0)] // TODO: update order
	public class BurstPool : ObjectPool {
		private HashSet<PooledObject> extras = new HashSet<PooledObject>();

		protected override void initialisePool() {
			base.initialisePool();
			extras = new HashSet<PooledObject>();
		}
		protected override bool canGrow => true;
		protected override void addNewObjects(int amountToAdd) {
			for(int added = 0; added < amountToAdd; added++) {
				extras.Add(newPooledObjectInstance());
			}
		}

		private protected override void reclaimRecycledObject(PooledObject recycled) {
			if(extras.Contains(recycled)) {
				extras.Remove(recycled);
				Destroy(recycled); // TODO: Improve by giving created objects a lifetime, and they are only culled after that lifetime
			}
			else base.reclaimRecycledObject(recycled);
		}
	}
}
