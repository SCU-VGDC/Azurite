using Unity.Loading;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ManagerContentPack", menuName = "Content/ManagerContentPack")]
public class ManagerContentPack : ScriptableObject
{
    public Loadable<GameManager> GameManagerPrefab;
    public Loadable<Camera> MainCameraPrefab;
    public Loadable<Player> PlayerPrefab;

    [MenuItem("Build/Content Directories/Build Manager Content")]
    public static void Build()
    {
        var param = new BuildContentDirectoryParameters
        {
            outputPath = "Builds/ManagerContent",
            rootAssetPaths = new string[] { "Assets/ContentPacks/ManagerContentPack.asset" },
            options = BuildContentOptions.None,
        };

        var report = BuildPipeline.BuildContentDirectory(param);
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            Debug.Log($"Manager content build succeeded: {report.summary.totalSize} bytes");
        else
            Debug.LogError("Manager content build failed.");
    }
}
