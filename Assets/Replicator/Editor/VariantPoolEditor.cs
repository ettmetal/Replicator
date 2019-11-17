using UnityEngine;
using UnityEditor;

namespace Replicator.Editor {
	[CustomEditor(typeof(VariantPool))]
	public class VariantPoolEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();
			variantsField();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("capacity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("preLoad"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("grow"));
			serializedObject.ApplyModifiedProperties();
		}

		private void variantsField() { // edit all the variants as a single array
			SerializedProperty basePrefab = serializedObject.FindProperty("prefab");
			SerializedProperty variants = serializedObject.FindProperty("variants");
			variants.isExpanded = EditorGUILayout.Foldout(variants.isExpanded, "Variants");
			if(variants.isExpanded) {
				EditorGUI.indentLevel++;
				// edit number of variants
				EditorGUI.BeginChangeCheck();
				int newArraySize = EditorGUILayout.DelayedIntField("Elements", variants.arraySize + 1);
				if(EditorGUI.EndChangeCheck()) {
					resizeArray(variants, Mathf.Max(newArraySize - 1, 0)); // Minimum 1 variant, the base
				}
				// draw array
				for(int i = -1; i < variants.arraySize; i++) {
					// The base variant is always in the first spot
					SerializedProperty current = i < 0 ? basePrefab : variants.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(current, new GUIContent($"Element {i + 1}"));
				}
				EditorGUI.indentLevel--;
			}
		}

		private static void resizeArray(SerializedProperty array, int newSize) {
			if(!array.isArray) throw new System.InvalidOperationException();
			array.arraySize = newSize;
		}
	}
}
