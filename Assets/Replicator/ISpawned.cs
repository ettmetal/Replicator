namespace Replicator {
	public interface ISpawned {
		/// <summary>
		/// Callback called after the associated GameObject is spawned, but before it is returned.
		/// </summary>
		void OnSpawn();
	}
}
