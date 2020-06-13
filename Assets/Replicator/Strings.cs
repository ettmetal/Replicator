﻿namespace Replicator {
	internal static partial class Strings {
		#region OBJECT_POOL
		public const string PrefabTooltip = "The prefab that will be used to populate the pool.";
		public const string PreLoadTooltip = "The number of objects to pre-load. If set to 0, all objects will be instantiated as needed. Cannot be higher than capacity.";
		public const string CapacityTooltip = "The maximum capacity of the pool.";
		public const string GrowTooltip = "Should the pool be allowed to grow? When it reaches capcity, new instances will be created depending on the selected strategy, e.g. Half adds half the capacity again, Double adds the whole capacity again.";
		public const string PoolMenuName = "Object Pools/Object Pool";
		public const string PoolFileName = "New Object Pool";
		public const string CantRecycleFormat = "It is not possible to recyle this GameObject. {0}";
		public const string NotPooled = "It does not belong to a pool.";
		public const string NotInPool = "It does not belong to this pool.";
		public const string UnableToSpawn = "";
		public const string HideUnspawedTooltip = "Should unspawned objects from this pool be visible in the hierarchy?";
		#endregion
		#region VARIANT_POOLS
		public const string RandomPoolMenuName = "Object Pools/Random Pool";
		public const string SequentialPoolMenuName = "Object Pools/Sequential Pool";
		#endregion
		#region BURST_POOL
		public const string BurstPoolMenuName = "Object Pools/Burst Pool";
		public const string BurstPoolCullMaxTooltip = "The maximum number of instances to cull in any frame";
		public const string BurstPoolCullIntervalTooltip = "The interval, in seconds, to wait between culling expired instances";
		#endregion
		#region POOLED_OBJECT
		public const string SetOwnerOnOwned = "SetOwner attempted on a game object which already belongs to a pool. No change made.";
		#endregion
		#region PARTICLE_SYSTEM
		public const string ParticlesResetTooltip = "Should the particles in the system be cleard (Default) or remain?";
		#endregion
	}
}
