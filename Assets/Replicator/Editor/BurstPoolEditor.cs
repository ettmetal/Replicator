using UnityEditor;
using UnityEngine;

namespace Replicator.Editor {
	[CustomEditor(typeof(BurstPool), true)]
	public class BurstPoolEditor : ObjectPoolEditor {
		private const string warningMessage = "Burst pools are designed to meet burst needs by instantiating, and culling objects. This will cause both instantiation, and destructions of GameObjects.";
		protected override void beforeFields() => EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
		protected override void growthField(SerializedProperty growth) {
			GUIContent label = new GUIContent(growth.displayName, growth.tooltip);
			GrowthStrategy current = (GrowthStrategy)growth.enumValueIndex;
			GrowthStrategy selected = (GrowthStrategy)EditorGUILayout.EnumPopup(label, current, isEnumValueEnabled, true);
			selected = selected == GrowthStrategy.None ? GrowthStrategy.Single : selected;
			growth.enumValueIndex = (int)selected;
		}

		private bool isEnumValueEnabled(System.Enum strategy) => (GrowthStrategy)strategy != GrowthStrategy.None;
	}
}
