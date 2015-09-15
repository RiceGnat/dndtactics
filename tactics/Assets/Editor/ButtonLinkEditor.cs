using UnityEngine;
using UnityEditor;

namespace DnDTactics.UI
{
	[CustomPropertyDrawer(typeof(Universal.UI.ButtonLink))]
	public class ButtonLinkEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty button = property.FindPropertyRelative("button");
			SerializedProperty target = property.FindPropertyRelative("target");

			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width / 2, position.height), button, GUIContent.none);
			EditorGUI.PropertyField(new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height), target, GUIContent.none);
		}
	}
}