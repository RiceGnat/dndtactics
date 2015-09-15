using UnityEngine;
using UnityEditor;

namespace Universal.UI
{
	[CustomPropertyDrawer(typeof(CommandLabelSet))]
	public class CommandLabelSetEditor : PropertyDrawer
	{
		private static float lines = UIPanel.CapturedInputs.Length;
		private static float lineOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var labels = property.FindPropertyRelative("labels");

			property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;
				for (int i = 0; i < lines; i++)
				{
					EditorGUI.PropertyField(new Rect(position.x, position.y + lineOffset * (i + 1), position.width, EditorGUIUtility.singleLineHeight), labels.GetArrayElementAtIndex(i), new GUIContent(UIPanel.CapturedInputs[i].Name));
				}
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return lineOffset * (property.isExpanded ? lines + 1 : 1) - 2;
		}
	}
}