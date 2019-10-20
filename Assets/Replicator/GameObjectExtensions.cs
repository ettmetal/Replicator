using UnityEngine;

public static partial class GameObjectExtensions {
	/// <summary>
	/// Gets the first component of type <typeparamref name="T"/> attached to the GameObject. Adds a component of that type if none exists.
	/// </summary>
	/// <typeparam name="T">The type of component to get or add. Must inherit UnityEngine.Component.</typeparam>
	public static T GetOrAddComponent<T>(this GameObject self) where T : Component {
			return self.GetComponent<T>() ?? self.AddComponent<T>();
		}
}
