using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Replicator.Editor {
	/// <summary>
	/// Editor for VariantPools, taking into account that the base class's prefab attribute should be treated as element 0 of the variants.
	/// </summary>
	[CustomEditor(typeof(VariantPool))]
	public class VariantPoolEditor : UnityEditor.Editor {
		private ReorderableList variantsList;
		private int lastSelectedIndex;
		private void OnEnable() { // Set up reorderable list with blank elements and add callbacks
			variantsList = new ReorderableList(getDummyElements(), typeof(GameObject));
			variantsList.drawHeaderCallback  = drawListHeader;
			variantsList.onAddCallback       = addListElement;
			variantsList.onRemoveCallback    = removeElement;
			variantsList.onCanRemoveCallback = canRemoveElement;
			variantsList.onChangedCallback   = listChanged;
			variantsList.onReorderCallback   = reorderElements;
			variantsList.drawElementCallback = drawListElement;
			variantsList.onSelectCallback    = captureSelectedIndex;
		}

		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();
			variantsField();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("capacity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("preLoad"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("growth"));
			serializedObject.ApplyModifiedProperties();
		}

		private void variantsField() { // edit all the variants as a single array
			SerializedProperty basePrefab = serializedObject.FindProperty("prefab");
			SerializedProperty variants = serializedObject.FindProperty("variants");
			variants.isExpanded = EditorGUILayout.Foldout(variants.isExpanded, "Variants");
			if(variants.isExpanded) {
				EditorGUI.indentLevel++;
				variantsList.DoLayoutList();
				EditorGUI.indentLevel--;
			}
		}

		private IList getDummyElements() { // Get an IList of appropriate length to represent all variants
			IList dummies = new ArrayList();
			int size = serializedObject.FindProperty("variants").arraySize + 1;
			for(int i = 0; i < size; i++) dummies.Add(new object());
			return dummies;
		}

		private SerializedProperty getElement(int index) { // Get variant at a given index, accounting for the base prefab
			return index == 0 ? serializedObject.FindProperty("prefab") : serializedObject.FindProperty("variants").GetArrayElementAtIndex(--index);
		}
		// Callbacks for reorderable list
		private void drawListHeader(Rect rect) => EditorGUI.LabelField(rect, "Variants");
		private void addListElement(ReorderableList list) => serializedObject.FindProperty("variants").arraySize++;
		// Can't remove the base prefab, can only nullify it
		private bool canRemoveElement(ReorderableList list) => serializedObject.FindProperty("variants").arraySize > 0 && list.index > 0;
		// Refresh dummies length when list changed
		private void listChanged(ReorderableList list) => list.list = getDummyElements();
		// Used for reordering
		private void captureSelectedIndex(ReorderableList list) => lastSelectedIndex = list.index;
		private void removeElement(ReorderableList list){
			SerializedProperty prop = serializedObject.FindProperty("variants");
			prop.GetArrayElementAtIndex(list.index - 1).objectReferenceValue = null;
			prop.DeleteArrayElementAtIndex(list.index - 1);
		}
		private void drawListElement(Rect rect, int index, bool isActive, bool isFocused) {
			GUIContent label = new GUIContent($"Variant {index + 1}");
			SerializedProperty prop = serializedObject.FindProperty(index == 0 ? "prefab" : "variants");
			prop = index == 0 ? prop : prop.GetArrayElementAtIndex(--index);
			EditorGUI.PropertyField(rect, prop, label);
		}

		private void reorderElements(ReorderableList list) {
			SerializedProperty a = getElement(lastSelectedIndex), b = getElement(list.index);
			Object temp = a.objectReferenceValue;
			a.objectReferenceValue = b.objectReferenceValue;
			b.objectReferenceValue = temp;
		}
	}
}
