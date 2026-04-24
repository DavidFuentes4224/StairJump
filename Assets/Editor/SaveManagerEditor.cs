using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Delete Save"))
		{
			SaveManager manager = (SaveManager)target;
			manager.DeleteSave();
		}
	}
}