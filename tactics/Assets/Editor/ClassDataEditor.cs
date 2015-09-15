using UnityEngine;
using UnityEditor;

namespace DnDTactics.Data
{
	[CustomPropertyDrawer(typeof(ClassData))]
	public class ClassDataEditor : PropertyDrawer
	{
		private const float lines = 8;
		private static float lineOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var name = property.FindPropertyRelative("name");
			var artwork = property.FindPropertyRelative("artwork");
			var portrait = property.FindPropertyRelative("portrait");
			var unique = property.FindPropertyRelative("unique");
			var baseStats = property.FindPropertyRelative("baseStats");
			var pointCosts = property.FindPropertyRelative("pointCosts");
			var movement = property.FindPropertyRelative("movement");

			Rect pos = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 100;
			EditorGUI.PropertyField(pos, name, new GUIContent("Name"));
			pos.y += lineOffset;
			EditorGUI.PropertyField(pos, unique, new GUIContent("Unique"));
			pos.y += lineOffset;
			EditorGUI.PropertyField(pos, artwork, new GUIContent("Artwork"));
			pos.y += lineOffset;
			EditorGUI.PropertyField(pos, portrait, new GUIContent("Portrait"));
			pos.y += lineOffset;

			Rect gridPos = new Rect(pos);
			gridPos.x += EditorGUIUtility.labelWidth;
			gridPos.width = (position.width - EditorGUIUtility.labelWidth) / 6;
			GUI.Label(gridPos, "STR");
			gridPos.x += gridPos.width;
			GUI.Label(gridPos, "CON");
			gridPos.x += gridPos.width;
			GUI.Label(gridPos, "DEX");
			gridPos.x += gridPos.width;
			GUI.Label(gridPos, "INT");
			gridPos.x += gridPos.width;
			GUI.Label(gridPos, "WIS");
			gridPos.x += gridPos.width;
			GUI.Label(gridPos, "CHA");

			pos.y += lineOffset;
			EditorGUI.PrefixLabel(pos, new GUIContent("Base Stats"));

			EditorGUI.indentLevel--;
			gridPos.y += lineOffset;
			gridPos.x = pos.x + EditorGUIUtility.labelWidth;
			baseStats.arraySize = 6;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(0), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(1), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(2), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(3), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(4), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, baseStats.GetArrayElementAtIndex(5), new GUIContent());
			EditorGUI.indentLevel++;

			pos.y += lineOffset;
			EditorGUI.PrefixLabel(pos, new GUIContent("Point Costs"));

			EditorGUI.indentLevel--;
			gridPos.y += lineOffset;
			gridPos.x = pos.x + EditorGUIUtility.labelWidth;
			pointCosts.arraySize = 6;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(0), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(1), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(2), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(3), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(4), new GUIContent());
			gridPos.x += gridPos.width;
			EditorGUI.PropertyField(gridPos, pointCosts.GetArrayElementAtIndex(5), new GUIContent());
			EditorGUI.indentLevel++;

			pos.y += lineOffset;
			EditorGUI.PropertyField(pos, movement, new GUIContent("Movement"));

			EditorGUIUtility.labelWidth = oldLabelWidth;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return lineOffset * lines;
		}
	}
}