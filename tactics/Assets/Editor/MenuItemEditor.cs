using UnityEngine;
using UnityEditor;

namespace Universal.UI
{
	[CustomPropertyDrawer(typeof(MenuItem))]
	public class MenuItemEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var button = property.FindPropertyRelative("button");
			var target = property.FindPropertyRelative("target");

			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width / 2, position.height), button, GUIContent.none);
			EditorGUI.PropertyField(new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height), target, GUIContent.none);
		}
	}
}