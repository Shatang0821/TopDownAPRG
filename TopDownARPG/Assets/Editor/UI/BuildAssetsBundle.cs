using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAssetsBundle : Editor
{
    [MenuItem("MyTool/BuildAssetsBundle")]

    static void BuildAllAssetsBundle()
    {
        //プラットフォームのAssetsBundleを作る
        BuildPipeline.BuildAssetBundles("./AssetsBundles/win64", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
