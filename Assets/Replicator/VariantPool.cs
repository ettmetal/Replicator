#pragma warning disable 649 // Prevent field not initialized warnings

using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	/// <summary>
	/// Asset representing and providing a mixed pool of GameObjects.
	/// </summary>
	[CreateAssetMenu(menuName = Strings.VariantPoolMenuName, fileName = Strings.PoolFileName, order = 204)]
	public class VariantPool : ObjectPool {
		[SerializeField]
		private GameObject[] variants;
		
		private Dictionary<GameObject, Stack<GameObject>> variantPools;

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
				GameObjectExtensions.poolRegistry.Add(variant, this);
			}
		}

		protected override void deregisterSelf() {
			base.deregisterSelf();
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				GameObjectExtensions.poolRegistry.Remove(variant);
			}
		}

		protected override void preloadObjects(int amountToPreload) {
			int preloadPerVariant = amountToPreload / (variants.Length + 1);
			int variantsWithExtra = amountToPreload % (variants.Length + 1);
			base.preloadObjects(preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			foreach(GameObject variant in variants) {
				if(variant == null) continue;
				populatePool(variant, preloadPerVariant + (variantsWithExtra-- > 0 ? 1 : 0));
			}
		}

		protected override GameObject getObjectToSpawn() {
			int typeToSpawn = Random.Range(-1, variants.Length);
			if(typeToSpawn < 0) return base.getObjectToSpawn();
			return variantPools[variants[typeToSpawn]].Pop();
		}

		private void populatePool(GameObject pooledObject, int amountToAdd){
			for(int i = 0; i < amountToAdd; i++) {
				variantPools[pooledObject].Push(instantiateInactive(pooledObject));
			}
		}
	}
}
