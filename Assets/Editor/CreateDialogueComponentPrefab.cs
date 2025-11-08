// File: Assets/Editor/CreateDialogueComponentPrefab.cs
using UnityEditor;
using UnityEngine;
using System.IO;

public static class CreateDialogueComponentPrefab
{
    private const string SourcePrefabPath = "Assets/Prefabs/Dialogue/DialogueComponent.prefab";

    [MenuItem("Assets/Create/Dialogue Component", false, 10)]
    private static void CreatePrefab()
    {
        string folder = GetSelectedFolderPath();

        // Load your existing prefab
        var srcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SourcePrefabPath);
        if (srcPrefab == null)
        {
            Debug.LogError($"Could not find prefab at path: {SourcePrefabPath}");
            return;
        }

        // Create a unique name in the target folder
        string newPath = AssetDatabase.GenerateUniqueAssetPath(
            Path.Combine(folder, "DialogueComponent.prefab")
        );

        // Instantiate a temporary copy to save as a new prefab
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(srcPrefab);
        PrefabUtility.SaveAsPrefabAsset(instance, newPath);

        // Clean up the temporary instance
        Object.DestroyImmediate(instance);
        AssetDatabase.Refresh();

        // Select and ping the new asset
        var newPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
        ProjectWindowUtil.ShowCreatedAsset(newPrefab);
        EditorGUIUtility.PingObject(newPrefab);

        Debug.Log($"Created new DialogueComponent prefab at: {newPath}");
    }

    private static string GetSelectedFolderPath()
    {
        string folder = "Assets";
        if (Selection.assetGUIDs != null && Selection.assetGUIDs.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            if (AssetDatabase.IsValidFolder(path))
                folder = path;
            else
                folder = Path.GetDirectoryName(path).Replace("\\", "/");
        }
        return folder;
    }
}
