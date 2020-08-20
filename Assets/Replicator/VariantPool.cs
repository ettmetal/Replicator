using System;
using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	/// <summary>Asset representing and providing a mixed pool of GameObjects.</summary>
	public abstract class VariantPool : ObjectPool {
		[SerializeField]
		private GameObject[] variants = null;

		private Dictionary<GameObject, Stack<GameObject>> variantPools;

		/// <summary>The total number of variants provided by the pool</summary>
		public int VariantCount => variants.Length + 1;

		internal void Initialise(GameObject[] allVariants, ushort capacity = 0, ushort preLoad = 0, GrowthStrategy growth = 0) {
			base.Initialise(allVariants[0], capacity, preLoad, growth);
			Array.Copy(allVariants, 1, variants, 0, allVariants.Length);
		}

		protected override void InitialisePool() {
			base.InitialisePool();
			variantPools = new Dictionary<GameObject, Stack<GameObject>>();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				variantPools.Add(variant, new Stack<GameObject>());
			}
		}

		protected override void RegisterSelf() {
			base.RegisterSelf();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				PoolRegistry.Pools.Add(variant, this);
			}
		}

		protected override void DeregisterSelf() {
			base.DeregisterSelf();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				PoolRegistry.Pools.Remove(variant);
			}
		}

		protected override void AddNewObjects(int amountToPreload) {
			int preloadPerVariant = amountToPreload / (variants.Length + 1);
			int variantsWithExtra = amountToPreload % (variants.Length + 1);
			base.AddNewObjects(preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				populatePool(variant, preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			}
		}

		protected override GameObject GetObjectToSpawn() {
			int spawnIndex = GetSpawnIndex(availableVariantIndicies());
			if(spawnIndex == 0) return base.GetObjectToSpawn();
			return variantPools[variants[spawnIndex - 1]].Pop();
		}

		protected abstract int GetSpawnIndex(int[] availableVariantIndicies);

		protected override bool HasAvailableSpawnees() {
			foreach(Stack<GameObject> pool in variantPools.Values) if(pool.Count > 0) return true;
			return base.HasAvailableSpawnees();
		}

		private void populatePool(GameObject pooledObject, int amountToAdd) {
			for(int i = 0; i < amountToAdd; i++) {
				variantPools[pooledObject].Push(InstantiateInactive(pooledObject));
			}
		}

		private int[] availableVariantIndicies() {
			List<int> collector = new List<int>();
			if(base.HasAvailableSpawnees()) {
				collector.Add(0);
			}
			foreach(GameObject variant in variants) {
				if(variant == null || variantPools[variant].Count < 1) {
					continue;
				}
				collector.Add(System.Array.IndexOf(variants, variant) + 1);
			}
			return collector.ToArray();
		}
	}
}
