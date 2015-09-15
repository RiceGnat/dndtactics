using UnityEngine;
using UnityEditor;
using System.Collections;
using DnDTactics.Data;

namespace DnDTactics.Editors
{
	[CustomEditor(typeof(DataManager))]
	public class DataManagerEditor : Editor
	{
		SerializedProperty classData;

		private void OnEnable()
		{
			//data = FindObjectOfType<DataManager>();
			classData = serializedObject.FindProperty("classData");
		}

		public override void OnInspectorGUI()
		{
			classData.isExpanded = EditorGUILayout.Foldout(classData.isExpanded, "Classes");

			if (classData.isExpanded)
			{
				EditorGUI.indentLevel++;

				for (int i = 0; i < classData.arraySize; i++)
				{
					EditorGUILayout.PropertyField(classData.GetArrayElementAtIndex(i));
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(18);
					if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
					{
						classData.DeleteArrayElementAtIndex(i);
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}

				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(18);
				if (GUILayout.Button("Add New Class"))
				{
					classData.arraySize++;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}