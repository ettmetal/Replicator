using UnityEditor;
using UnityEngine;

namespace Replicator.Editor {
	[CustomEditor(typeof(BurstPool), true)]
	public class BurstPoolEditor : ObjectPoolEditor {
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
