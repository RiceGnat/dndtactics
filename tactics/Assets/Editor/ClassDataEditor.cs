using UnityEngine;
using UnityEditor;

namespace DnDTactics.Data
{
	[CustomPropertyDrawer(typeof(ClassData))]
	public class ClassDataEditor : PropertyDrawer
	{
		private const float lines = 4;
		private static float lineOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var @class = property.FindPropertyRelative("class");
			var className = property.FindPropertyRelative("className");
			var classArtwork = property.FindPropertyRelative("classArtwork");
			var classPortrait = property.FindPropertyRelative("classPortrait");
			var unique = property.FindPropertyRelative("unique");

			var quarterWidth = position.width / 4;

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 90;
			EditorGUI.PropertyField(new Rect(position.x, position.y, quarterWidth, EditorGUIUtility.singleLineHeight), @class, GUIContent.none);
			EditorGUI.PropertyField(new Rect(position.x + quarterWidth, position.y, quarterWidth * 3, EditorGUIUtility.singleLineHeight), className, new GUIContent("Display as"));
			EditorGUI.PropertyField(new Rect(position.x + quarterWidth, position.y + lineOffset, quarterWidth * 3, EditorGUIUtility.singleLineHeight), classArtwork, new GUIContent("Artwork"));
			EditorGUI.PropertyField(new Rect(position.x + quarterWidth, position.y + lineOffset * 2, quarterWidth * 3, EditorGUIUtility.singleLineHeight), classPortrait, new GUIContent("Portrait"));
			EditorGUI.PropertyField(new Rect(position.x + quarterWidth, position.y + lineOffset * 3, quarterWidth * 3, EditorGUIUtility.singleLineHeight), unique, new GUIContent("Unique"));
			EditorGUIUtility.labelWidth = oldLabelWidth;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return lineOffset * lines;
		}
	}
}