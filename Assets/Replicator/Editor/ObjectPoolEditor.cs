using UnityEditor;

namespace Replicator.Editor {
	[CustomEditor(typeof(ObjectPool))]
	public class ObjectPoolEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("capacity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("preLoad"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("growth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("hideUnspawned"));
			serializedObject.ApplyModifiedProperties();
		}
	}
}
