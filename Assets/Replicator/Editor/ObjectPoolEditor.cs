using UnityEngine;
using UnityEditor;

namespace Replicator.Editor {
	[CustomEditor(typeof(ObjectPool))]
	public class ObjectPoolEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();
			prefabField(serializedObject.FindProperty("prefab"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("capacity"));
			preLoadField(serializedObject.FindProperty("preLoad"), serializedObject.FindProperty("capacity"));
			growthField(serializedObject.FindProperty("growth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("hideUnspawned"));
			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void prefabField(SerializedProperty prefab) => EditorGUILayout.PropertyField(prefab);

		protected virtual void growthField(SerializedProperty growth) => EditorGUILayout.PropertyField(growth);

		// Sets the preLoad value equal to capacity, so long as it has not been manually edited
		protected void preLoadField(SerializedProperty preLoad, SerializedProperty capacity) {
			EditorGUI.BeginChangeCheck();
			GUIContent label = EditorGUI.BeginProperty(new Rect(), null, preLoad);
			int displayValue = preLoad.intValue == ushort.MaxValue ? capacity.intValue : preLoad.intValue;
			int newValue = EditorGUILayout.IntField(label, displayValue);
			if(EditorGUI.EndChangeCheck()) preLoad.intValue = newValue;
		}
	}
}
