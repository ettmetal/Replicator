﻿namespace Replicator {
	internal static partial class Strings {
		#region OBJECT_POOL
		public const string PrefabTooltip = "The prefab that will be used to populate the pool.";
		public const string PreLoadTooltip = "Should the pool preload its objects? The alternative is instantiating them as needed.";
		public const string CapacityTooltip = "The maximum capacity of the pool.";
		public const string GrowTooltip = "Should the pool be allowed to grow? When it reaches capcity, it will continue to instantiate ";
		public const string PoolMenuName = "Object Pool";
		public const string PoolFileName = "New Object Pool";
		public const string CantRecycleFormat = "It is not possible to recyle this GameObject. {0}";
		public const string NotPooled = "It does not belong to a pool.";
		public const string NotInPool = "It does not belong to this pool.";
		public const string UnableToSpawn = "";
		#endregion
		#region POOLED_OBJECT
		public const string SetOwnerOnOwned = "SetOwner attempted on a game object which already belongs to a pool. No change made.";
		#endregion
	}
}
