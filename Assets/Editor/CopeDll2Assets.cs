using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HybridCLR;
using System.IO;

public class CopeDll2Assets : Editor
{
    [MenuItem("Tools/����Dll��Assets/ActiveBuildTarget")]
    static void CopeByActive()
    {
        Copy(EditorUserBuildSettings.activeBuildTarget);
    }
    [MenuItem("Tools/����Dll��Assets/Win32")]
    static void CopeByStandaloneWindows32()
    {
        Copy(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/����Dll��Assets/Win64")]
    static void CopeByStandaloneWindows64()
    {
        Copy(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Tools/����Dll��Assets/Android")]
    static void CopeByAndroid()
    {
        Copy(BuildTarget.Android);
    }
    [MenuItem("Tools/����Dll��Assets/IOS")]
    static void CopeByIOS()
    {
        Copy(BuildTarget.iOS);
    }

    static void Copy(BuildTarget target)
    {
        List<string> copyDlls = new List<string>()
        {
            "HotFix.dll",
        }; 
        string outDir = BuildConfig.GetHotFixDllsOutputDirByTarget(target);
        string exportDir = Application.dataPath + "/Res/Dlls";
        if (!Directory.Exists(exportDir))
        {
            Directory.CreateDirectory(exportDir);
        }
        foreach (var copyDll in copyDlls)
        {
            File.Copy($"{outDir}/{copyDll}", $"{exportDir}/{copyDll}.bytes", true);
        }

        string aotDllDir = $"{BuildConfig.AssembliesPostIl2CppStripDir}/{target}";
        foreach (var dll in LoadDll.aotDlls)
        {
            string dllPath = $"{aotDllDir}/{dll}";
            if (!File.Exists(dllPath))
            {
                Debug.LogError($"ab�����AOT����Ԫ����dll:{dllPath} ʱ��������,�ļ������ڡ���Ҫ����һ��������������ɲü����AOT dll");
                continue;
            }
            string dllBytesPath = $"{exportDir}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
        }
        AssetDatabase.Refresh();
        Debug.Log("�ȸ�Dll���Ƴɹ���");
    }
}
