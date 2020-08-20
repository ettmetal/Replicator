using UnityEngine;

namespace Replicator {
	/// <summary>
	/// Asset representing and providing a mixed pool of GameObjects, which will spawn in the sequence provided.
	/// </summary>
	[CreateAssetMenu(menuName = Strings.SequentialPoolMenuName, fileName = Strings.PoolFileName, order = 204)]
	public class SequentialPool : VariantPool {
		private int lastSpawnedIndex = 0;

		/// <summary>
		/// Create a new <see cref="SequentialPool"/> with the given parameters.
		/// </summary>
		public static SequentialPool Create(GameObject[] allVariants, ushort capacity = 0, ushort preLoad = 0, GrowthStrategy growth = 0) {
			SequentialPool newPool = CreateInstance<SequentialPool>();
			newPool.Initialise(allVariants, capacity, preLoad, growth);
			return newPool;
		}

		protected override int GetSpawnIndex(int[] availableVariantIndicies) {
			lastSpawnedIndex = ++lastSpawnedIndex % VariantCount;
			return closestAvailableToDesired(lastSpawnedIndex, availableVariantIndicies);
		}

		private int closestAvailableToDesired(int desired, int[] available) {
			int closest = int.MaxValue;
			int minDistance = int.MaxValue;
			foreach(int index in available) {
				int distance = Mathf.Abs(index - desired);
				if(distance <= minDistance) {
					minDistance = distance;
					closest = index;
				}
				if(distance == 0) {
					break;
				}
			}
			return closest;
		}
	}
}
