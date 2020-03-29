using UnityEngine;

namespace Replicator {
	/// <summary>
	/// Asset representing and providing a mixed pool of GameObjects, which will spawn in the sequence provided.
	/// </summary>
	[CreateAssetMenu(menuName = Strings.SequentialPoolMenuName, fileName = Strings.PoolFileName, order = 204)]
	public class SequentialPool : VariantPool {
		private int lastSpawnedIndex = 0;

		protected override int getSpawnIndex(int[] availableVariantIndicies) {
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
