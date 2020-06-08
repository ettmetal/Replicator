using UnityEngine;

namespace Replicator {
	/// <summary>A multi-object pool which will return a random variant each time a spawn is requested</summary>
	[CreateAssetMenu(menuName = Strings.RandomPoolMenuName, fileName = Strings.PoolFileName, order = 204)]
	public class RandomPool : VariantPool {
		protected override int GetSpawnIndex(int[] availableVariantIndecies) {
			return availableVariantIndecies[Random.Range(0, availableVariantIndecies.Length)];
		}
	}
}
