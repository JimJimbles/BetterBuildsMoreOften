using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    static string[] Scenes = FindEnabledEditorScenes();

    static string ProjectName = "My Cool Game";
    static string BuildPath = "C:/Builds/";

    [MenuItem("Tools/Test CI Build")]
    static void TestGenericBuild()
    {
        string target_dir = BuildPath + ProjectName + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "/" + ProjectName + ".exe";

        GenericBuild(Scenes, target_dir, BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64,
            BuildOptions.None);
    }
    
    static void BashBuild()
    {
        var outputPath = GetCommandLineArg("customBuildPath");
        var buildName = GetCommandLineArg("customBuildName");

        string target_dir = Path.Combine(outputPath, buildName) + ".exe";
        Debug.Log("Automated Build attempting to build to " + target_dir);
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = Scenes;
        buildPlayerOptions.locationPathName = target_dir;
        buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget; //In batch mode we can only compile for the target we've imported for
        buildPlayerOptions.targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
    }
    

    static void GenericBuild(string[] scenes, string target_dir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = target_dir;
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.targetGroup = buildTargetGroup;
        buildPlayerOptions.options = build_options;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    
    static void PerformWindowsCIBuild()
    {
        var outputPath = GetCommandLineArg("buildPath");
        
        
        //TODO Take in build number/ branch name from jenkins instead of date
        string target_dir = Path.Combine(outputPath, ProjectName + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm"), ProjectName) + ".exe";
        Debug.Log("Automated Build attempting to build to " + target_dir);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = Scenes;
        buildPlayerOptions.locationPathName = target_dir;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;
        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
    }
    
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
    
    private static string GetCommandLineArg(string argName)
    {
        var args = Environment.GetCommandLineArgs();
        for (var i = 0; i < args.Length; i++)
        {
            if (Regex.IsMatch(args[i], $"^-{argName}$") && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
        throw new Exception($"Could not find command-line argument \"{argName}\"");
    }

}
