using UnityEngine;

namespace Replicator {
	/// <summary>A multi-object pool which will return a random variant each time a spawn is requested</summary>
	[CreateAssetMenu(menuName = Strings.RandomPoolMenuName, fileName = Strings.PoolFileName, order = 204)]
	public class RandomPool : VariantPool {
		/// <summary>
		/// Create a new <see cref="RandomPool"/> with the given parameters.
		/// </summary>
		public static RandomPool Create(GameObject[] allVariants, ushort capacity = 0, ushort preLoad = 0, GrowthStrategy growth = 0) {
			RandomPool newPool = CreateInstance<RandomPool>();
			newPool.Initialise(allVariants, capacity, preLoad, growth);
			return newPool;
		}
		protected override int GetSpawnIndex(int[] availableVariantIndecies) =>
			availableVariantIndecies[Random.Range(0, availableVariantIndecies.Length)];
	}
}
