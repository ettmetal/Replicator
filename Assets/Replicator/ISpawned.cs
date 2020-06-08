namespace Replicator {
	/// <summary>Interface for hooking into a GameObject's spawn phase.</summary>
	public interface ISpawned {
		/// <summary>
		/// Callback called after the associated GameObject is spawned, but before it is returned.
		/// </summary>
		void OnSpawn();
	}
}
