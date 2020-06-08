namespace Replicator {
	/// <summary>Interface for hooking into a GameObject's recycle phase.</summary>
	public interface IRecycled {
		/// <summary>
		/// Callback to implement cleanup or other actions when the GameObject this script is attached to is recycled.
		/// </summary>
		void OnRecycle();
	}
}
