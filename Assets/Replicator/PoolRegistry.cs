using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	/// <summary>
	/// Provide a registry of 'live' pools internally to facilitate mimicking the Instantiate / Destroy API.
	/// </summary>
	internal static class PoolRegistry {

		private static Dictionary<GameObject, ObjectPool> registry = new Dictionary<GameObject, ObjectPool>();
		public static IDictionary<GameObject, ObjectPool> pools => registry;
	}
}
