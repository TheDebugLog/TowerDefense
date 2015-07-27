using UnityEngine;
using UnityEditor;
using System.Collections;

public class PathUtils : Editor {
	private static Vector3 positionClipboard;
	private static Quaternion rotationClipboard;

	[MenuItem ("Utility/Copy Position and Rotation")]
	public static void CopyPositionRotation() {
		positionClipboard = Selection.activeGameObject.transform.position;
		rotationClipboard = Selection.activeGameObject.transform.rotation;
	}
	
	[MenuItem ("Utility/Paste Position and Rotation")]
	public static void PastePositionRotation() {
		Selection.activeGameObject.transform.position = positionClipboard;
		Selection.activeGameObject.transform.rotation = rotationClipboard;
	}

	static void ExportPackage() {
		string[] assets = new string[3];
		
		assets[0] = "Assets/Editor";
		assets[1] = "Assets/Prefabs";
		assets[2] = "Assets/Scripts";
		
		AssetDatabase.ExportPackage(assets, "bin/PathController.unityPackage", ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
	}
}
