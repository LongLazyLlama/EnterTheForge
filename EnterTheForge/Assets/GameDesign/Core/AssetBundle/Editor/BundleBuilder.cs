using UnityEditor;
using UnityEngine;

using System.IO;

using GameDesign.Core.AssetBundle;

public class BundleBuilder : Editor
{
    static string _pathAssembly;
    static GameInfoData _gameInfoData;

    [MenuItem("Game Design/Export/Build AssetBundles")]
    static void BuildAssetbundles()
    {
        if ( SetAssembly())
        {
            SetLayerMatrix();
            AssetDatabase.SaveAssets();
            string path = EditorUtility.OpenFolderPanel("Where to save?", "", "");
            if (!string.IsNullOrEmpty(path))//In case they canceled the action
            {
                BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
                Debug.Log("Export was succesfull.");
            }
            ClearCache();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "No assembly found", "ok");
        }
     }

    static void SetLayerMatrix()
    {
        if (_gameInfoData)
        {
            uint maxLayers = 32;

            LayerCollision lc = new LayerCollision();
            lc.LayerMatrix = new bool[maxLayers * maxLayers];
            for (int x = 0; x < maxLayers; ++x)
            {
                for (int y = 0; y < maxLayers - x; ++y)
                {
                    lc.LayerMatrix[(x * maxLayers) + y] = !Physics.GetIgnoreLayerCollision(x, y);
                }
            }

            _gameInfoData.LayerInfo = JsonUtility.ToJson(lc);
        }
    }

    static bool SetAssembly()
    {
        _gameInfoData = null;
        string[] assets = AssetDatabase.FindAssets("t:GameInfoData");
        if (assets != null && assets.Length > 0)
        {
            GameInfoData info = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0])) as GameInfoData;
            if (info)
            {
                _gameInfoData = info;
                string assemblyDef = _gameInfoData.AssemblyDefinition;
                DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath);
                dirInfo = new DirectoryInfo(Path.Combine(dirInfo.Parent.ToString(), "Library\\ScriptAssemblies"));
                string path = dirInfo + "\\" + assemblyDef + ".dll";

                if (File.Exists(path))//Is there an assembly?
                {
                    string nameAssembly = "assembly.bytes";
                    _pathAssembly = Path.Combine(Application.dataPath, nameAssembly);

                    ClearCache();//In case the file already exist

                    File.Copy(path, _pathAssembly);
                    AssetDatabase.Refresh();
                    _gameInfoData.AssemblyTextAsset = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/"+nameAssembly, typeof(TextAsset));
                    if(_gameInfoData.AssemblyTextAsset != null)
                    {
                        return true;
                    }
                }
                Debug.LogWarning("Assembly does not exist, or wrong name...");
            }
            Debug.LogWarning("Couldn't find you 'Game Info Data'");
        }

        return false;
    }

    static void ClearCache()
    {
        if (!string.IsNullOrEmpty(_pathAssembly))
        {
            if (File.Exists(_pathAssembly + ".meta"))
            {
                File.Delete(_pathAssembly + ".meta");
            }
            if (File.Exists(_pathAssembly))
            {
                File.Delete(_pathAssembly);
            }

            AssetDatabase.Refresh();
        }
    }
}
