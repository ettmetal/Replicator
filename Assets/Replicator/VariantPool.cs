﻿#pragma warning disable 649 // Prevent field not initialized warnings

using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	/// <summary>Asset representing and providing a mixed pool of GameObjects.</summary>
	public abstract class VariantPool : ObjectPool {
		[SerializeField]
		private GameObject[] variants;

		private Dictionary<GameObject, Stack<GameObject>> variantPools;

		/// <summary>The total number of variants provided by the pool</summary>
		public int VariantCount => variants.Length + 1;

		protected override void initialisePool() {
			base.initialisePool();
			variantPools = new Dictionary<GameObject, Stack<GameObject>>();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				variantPools.Add(variant, new Stack<GameObject>());
			}
		}

		protected override void registerSelf() {
			base.registerSelf();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				PoolRegistry.pools.Add(variant, this);
			}
		}

		protected override void deregisterSelf() {
			base.deregisterSelf();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				PoolRegistry.pools.Remove(variant);
			}
		}

		protected override void addNewObjects(int amountToPreload) {
			int preloadPerVariant = amountToPreload / (variants.Length + 1);
			int variantsWithExtra = amountToPreload % (variants.Length + 1);
			base.addNewObjects(preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				populatePool(variant, preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			}
		}

		protected override GameObject getObjectToSpawn() {
			int spawnIndex = getSpawnIndex(availableVariantIndicies());
			if(spawnIndex == 0) return base.getObjectToSpawn();
			return variantPools[variants[spawnIndex - 1]].Pop();
		}

		protected abstract int getSpawnIndex(int[] availableVariantIndicies);

		protected override bool hasAvailableSpawnees() {
			foreach(Stack<GameObject> pool in variantPools.Values) if(pool.Count > 0) return true;
			return base.hasAvailableSpawnees();
		}

		private void populatePool(GameObject pooledObject, int amountToAdd) {
			for(int i = 0; i < amountToAdd; i++) {
				variantPools[pooledObject].Push(instantiateInactive(pooledObject));
			}
		}

		private int[] availableVariantIndicies() {
			List<int> collector = new List<int>();
			if(base.hasAvailableSpawnees()) {
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
