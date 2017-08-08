﻿using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SevenZip.Compression.LZMA;
using SevenZip;
using System.Diagnostics;

public class Tools {

    [MenuItem("Tools/1.拷贝数据表和脚本")]
    static void CopyTableAndScripts()
    {
        Process p = new Process();
        p.StartInfo.FileName = Application.dataPath + "/../../tools/copyClientFiles.bat";
        p.StartInfo.Arguments = PathDefine.TABLE_ASSET_PATH.Replace("/", "\\");
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.WorkingDirectory = "../tools/";
        p.Start();
    }

    [MenuItem("Tools/2.打资源包")]
    static public void BuildAssetBundle()
    {
        SetPlayer();
        SetEffect();
        SetUI();
        SetTable();
        SetLua();

        string resPkgPath = Application.streamingAssetsPath + "/" + Define.PackageVersion;
        if (!Directory.Exists(resPkgPath))
            Directory.CreateDirectory(resPkgPath);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/" + Define.PackageVersion, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/3.删除临时资源")]
    static void DeleteTempFiles()
    {
        Process p = new Process();
        p.StartInfo.FileName = Application.dataPath + "/../../tools/removeResources.bat";
        p.StartInfo.Arguments = PathDefine.TABLE_ASSET_PATH.Replace("/", "\\");
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.WorkingDirectory = "../tools/";
        p.Start();
    }

//    static void SetScene()
//    {
//        BuildPlayerOptions opti = new BuildPlayerOptions();
//        opti.locationPathName = Application.streamingAssetsPath + "/" + Define.PackageVersion + "/";
//        opti.options = BuildOptions.CompressWithLz4;
//        opti.targetGroup = BuildTargetGroup.Standalone;
//        BuildPipeline.BuildPlayer(opti);
//    }

    static void SetPlayer()
    {
        CollectAllFiles(string.Format("{0}/{1}", Application.dataPath, "Resources/" + PathDefine.PLAYER_ASSET_PATH));
        for(int i=0; i < _AllFiles.Count; ++i)
        {
            string _assetPath = "Assets" + _AllFiles[i].Substring (Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            AssetImporter aimport = AssetImporter.GetAtPath(_assetPath);
            string shortPath = _assetPath.Substring(_assetPath.LastIndexOf("/") + 1);
            aimport.assetBundleName = PathDefine.PLAYER_ASSET_PATH + shortPath.Remove(shortPath.IndexOf(".")) + Define.ASSET_EXT;
            string[] deps = AssetDatabase.GetDependencies(_assetPath);
            for(int j=0; j < deps.Length; ++j)
            {
                if (deps [j].IndexOf(".js") != -1)
                    continue;

                if (deps [j].IndexOf(".cs") != -1)
                    continue;
                
                if (deps [j].Equals(_assetPath))
                    continue;

                if (_AllDependences.Contains(deps [j]))
                    continue;

                _AllDependences.Add(deps[j]);
                UnityEngine.Debug.Log(deps[j]);
            }
        }
        for(int i=0; i < _AllDependences.Count; ++i)
        {
            AssetImporter aimport = AssetImporter.GetAtPath(_AllDependences[i]);
            aimport.assetBundleName = PathDefine.COMMON_ASSET_PATH + AssetDatabase.AssetPathToGUID(_AllDependences[i]) + Define.ASSET_EXT;
        }
        _AllFiles.Clear();
        _AllDirectories.Clear();
        _AllDependences.Clear();
    }

    static void SetEffect()
    {
        CollectAllFiles(string.Format("{0}/{1}", Application.dataPath, "Resources/" + PathDefine.EFFECT_ASSET_PATH));
        for(int i=0; i < _AllFiles.Count; ++i)
        {
            string _assetPath = "Assets" + _AllFiles[i].Substring (Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            AssetImporter aimport = AssetImporter.GetAtPath(_assetPath);
            string shortPath = _assetPath.Substring(_assetPath.LastIndexOf("/") + 1);
            aimport.assetBundleName = PathDefine.EFFECT_ASSET_PATH + shortPath.Remove(shortPath.IndexOf(".")) + Define.ASSET_EXT;
            string[] deps = AssetDatabase.GetDependencies(_assetPath);
            for(int j=0; j < deps.Length; ++j)
            {
                if (deps [j].IndexOf(".js") != -1)
                    continue;

                if (deps [j].IndexOf(".cs") != -1)
                    continue;
                
                if (deps [j].Equals(_assetPath))
                    continue;

                if (_AllDependences.Contains(deps [j]))
                    continue;

                _AllDependences.Add(deps[j]);
                UnityEngine.Debug.Log(deps[j]);
            }
        }
        for(int i=0; i < _AllDependences.Count; ++i)
        {
            AssetImporter aimport = AssetImporter.GetAtPath(_AllDependences[i]);
            aimport.assetBundleName = PathDefine.COMMON_ASSET_PATH + AssetDatabase.AssetPathToGUID(_AllDependences[i]) + Define.ASSET_EXT;
        }
        _AllFiles.Clear();
        _AllDirectories.Clear();
        _AllDependences.Clear();
    }

    static void SetUI()
    {
        CollectAllFiles(string.Format("{0}/{1}", Application.dataPath, "Resources/" + PathDefine.UI_ASSET_PATH));
        for(int i=0; i < _AllFiles.Count; ++i)
        {
            string _assetPath = "Assets" + _AllFiles[i].Substring (Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            AssetImporter aimport = AssetImporter.GetAtPath(_assetPath);
            string shortPath = _assetPath.Substring(_assetPath.LastIndexOf("/") + 1);
            int idx = shortPath.IndexOf("@");
            if (idx != -1)
            {
                shortPath = shortPath.Remove(idx);
                shortPath += "_res";
            }
            else
            {
                shortPath = shortPath.Remove(shortPath.IndexOf("."));
                shortPath += "_desc";
            }
            string finalPath = PathDefine.UI_ASSET_PATH + shortPath + Define.ASSET_EXT;
            aimport.assetBundleName = finalPath;
            string[] deps = AssetDatabase.GetDependencies(_assetPath);
            for(int j=0; j < deps.Length; ++j)
            {
                if (deps [j].IndexOf(".js") != -1)
                    continue;

                if (deps [j].IndexOf(".cs") != -1)
                    continue;
                
                if (deps [j].Equals(_assetPath))
                    continue;

                if (_AllDependences.Contains(deps [j]))
                    continue;

                _AllDependences.Add(deps[j]);
                UnityEngine.Debug.Log(deps[j]);
            }
        }
        for(int i=0; i < _AllDependences.Count; ++i)
        {
            AssetImporter aimport = AssetImporter.GetAtPath(_AllDependences[i]);
            aimport.assetBundleName = PathDefine.COMMON_ASSET_PATH + AssetDatabase.AssetPathToGUID(_AllDependences[i]) + Define.ASSET_EXT;
        }
        _AllFiles.Clear();
        _AllDirectories.Clear();
        _AllDependences.Clear();
    }

    static void SetTable()
    {
        CollectAllFiles(string.Format("{0}/{1}", Application.dataPath, "Resources/" + PathDefine.TABLE_ASSET_PATH));
        for(int i=0; i < _AllFiles.Count; ++i)
        {
            string _assetPath = "Assets" + _AllFiles[i].Substring (Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            AssetImporter aimport = AssetImporter.GetAtPath(_assetPath);
            string shortPath = _assetPath.Substring(_assetPath.LastIndexOf("/") + 1);
            aimport.assetBundleName = PathDefine.TABLE_ASSET_PATH + shortPath.Remove(shortPath.IndexOf(".")) + Define.ASSET_EXT;
            string[] deps = AssetDatabase.GetDependencies(_assetPath);
            for(int j=0; j < deps.Length; ++j)
            {
                if (deps [j].IndexOf(".js") != -1)
                    continue;

                if (deps [j].IndexOf(".cs") != -1)
                    continue;
                
                if (deps [j].Equals(_assetPath))
                    continue;

                if (_AllDependences.Contains(deps [j]))
                    continue;

                _AllDependences.Add(deps[j]);
                UnityEngine.Debug.Log(deps[j]);
            }
        }
        for(int i=0; i < _AllDependences.Count; ++i)
        {
            AssetImporter aimport = AssetImporter.GetAtPath(_AllDependences[i]);
            aimport.assetBundleName = PathDefine.COMMON_ASSET_PATH + AssetDatabase.AssetPathToGUID(_AllDependences[i]) + Define.ASSET_EXT;
        }
        _AllFiles.Clear();
        _AllDirectories.Clear();
        _AllDependences.Clear();
    }
    static void SetLua()
    {
        string sourcePath = string.Format("{0}/{1}/{2}", Application.dataPath, "Resources", PathDefine.LUA_ASSET_PATH);
        if (!Directory.Exists(sourcePath))
        {
            return;
        }

        string[] files = Directory.GetFiles(sourcePath, "*.lua", SearchOption.AllDirectories);
        int len = sourcePath.Length;

        if (sourcePath[len - 1] == '/' || sourcePath[len - 1] == '\\')
        {
            --len;
        }         

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, len);
            string dest = LuaConst.luaResDir + "/" + str;
            //dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], dest, true);
        }
    }



    static List<string> _AllFiles = new List<string>();
    static List<string> _AllDirectories = new List<string>();
    static List<string> _AllDependences = new List<string>();

    static void CollectFiles(string path)
    {
        string[] files = Directory.GetFiles(path);
        for(int i=0; i < files.Length; ++i)
        {
            if (files [i].EndsWith(".meta"))
                continue;

            _AllFiles.Add(files[i]);
        }
    }

    static void CollectAllFiles(string path)
    {
        CollectFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        if (dirs.Length != 0)
            _AllDirectories.AddRange(dirs);
        for(int i=0; i < dirs.Length; ++i)
        {
            CollectAllFiles(dirs[i]);
        }
    }

    [MenuItem("Tools/Clear")]
    static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        UnityEngine.Debug.Log(length);
        string[] oldAssetBundleNames = new string[length];
        for(int i=0; i < length; ++i)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }
        for(int i=0; i < oldAssetBundleNames.Length; ++i)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[i], true);
        }
        length = AssetDatabase.GetAllAssetBundleNames().Length;
        UnityEngine.Debug.Log(length);
    }

    [MenuItem("Tools/TestLoad")]
    static void TestLoad()
    {
        AssetLoader.LoadAsset(PathDefine.PLAYER_ASSET_PATH + "longtaizi");
    }

    [MenuItem ("MyMenu/CompressFile")]
    static void CompressFile () 
    {
        //压缩文件
        CompressFolder(Application.dataPath+"/Resources/Player/Prefabs/",Application.dataPath+"/Resources/Player/Prefabs/longtaizi.prefab");
        AssetDatabase.Refresh();

    }
    [MenuItem ("MyMenu/DecompressFile")]
    static void DecompressFile () 
    {
        //解压文件
        //DecompressFileLZMA(Application.streamingAssetsPath+"/Player.zip",Application.streamingAssetsPath+"/longtaizi.prefab");
        AssetDatabase.Refresh();
    }
//
//
//    private static void CompressFileLZMA(string inFile, string outFile)
//    {
//        Encoder coder = new Encoder();
//        FileStream input_sum = new FileStream(inFile + "/Temp.tmp", FileMode.Create);
//        FileStream output = new FileStream(outFile, FileMode.Create);
//        // Write the encoder properties
//        coder.WriteCoderProperties(output);
//        if (inFile.Contains("."))
//        {
//            input_sum.Close();
//            input_sum = new FileStream(inFile, FileMode.Open);
//            // Write the decompressed file size.
//            output.Write(BitConverter.GetBytes(input_sum.Length), 0, 8);
//            // Encode the file.
//            coder.Code(input_sum, output, input_sum.Length, -1, null);
//            output.Write(new byte[]{0}, 0, 1);
//        }
//        else
//        {
//            FileStream header = new FileStream(inFile + "/header.tmp", FileMode.Create);
//            output.Write(new byte[]{1}, 0, 1);
//            CollectAllFiles(inFile);
//            byte[] buffer;
//            int fileIndex = 0;
//            for(int i = 0; i < _AllFiles.Count; ++i)
//            {
//                FileStream fs = new FileStream(_AllFiles[i], FileMode.Open);
//                buffer = new byte[fs.Length];
//                fs.Read(buffer, 0, fs.Length);
//                input_sum.Write(buffer, fileIndex, buffer.Length);
//                fileIndex += buffer.Length - 1;
//
//                output.Write(BitConverter.GetBytes(buffer.Length), 0, 8);
//                output.Write(System.Text.Encoding.Default.GetBytes(_AllFiles[i]), 0, 128);
//
//            }
//
//            // Encode the file.
//            coder.Code(input_sum, output, input_sum.Length, -1, null);
//        }
//        output.Flush();
//        output.Close();
//        input_sum.Close();
//    }

//    private static void DecompressFileLZMA(string inFile, string outFile)
//    {
//        Decoder coder = new Decoder();
//        FileStream input = new FileStream(inFile, FileMode.Open);
//        FileStream output = new FileStream(outFile, FileMode.Create);
//
//        // Read the decoder properties
//        byte[] properties = new byte[5];
//        input.Read(properties, 0, 5);
//
//        byte[] isDir = new byte[1];
//        input.Read(isDir, 0, 1);
//
//        //文件
//        if (isDir [0] == 0)
//        {
//            
//        }
//        else
//        {
//            
//        }
//
//        // Read in the decompress file size.
//        byte [] fileLengthBytes = new byte[8];
//        input.Read(fileLengthBytes, 0, 8);
//        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
//
//        // Decompress the file.
//        coder.SetDecoderProperties(properties);
//        coder.Code(input, output, input.Length, fileLength, null);
//        output.Flush();
//        output.Close();
//        input.Close();
//    }

    static public void CompressFolder(string FolderToCompress, string destination)
    {
        List<string> subfiles = new List<string>(Directory.GetFiles(FolderToCompress));
        FileInfo fi = new FileInfo(FolderToCompress);
        System.Text.StringBuilder output_7zip_File = new System.Text.StringBuilder(FolderToCompress + Path.DirectorySeparatorChar + fi.Name + @".7z");
        string output_stringBuilder = output_7zip_File.ToString();

        UnityEngine.Debug.Log("Output destination : " + output_stringBuilder);



        foreach (string file in subfiles)
        {
            UnityEngine.Debug.Log("Files to Compress : " + file);
            // compressor.BeginCompressFiles(output_stringBuilder, file);
            CompressFileLZMA(file, output_stringBuilder);
            //AddToArchive(file);
        }



    }


    //___________________________________________________________
    ////////////////////////////////////////////////////////////|
    //         C O M P R E S S  F I L E                         |                                           
    //_________using LZMA algo__________________________________|
    ////////////////////////////////////////////////////////////|
    static public void CompressFileLZMA(string inFile, string outFile)
    {
        Int32 dictionary = 1 << 23;
        Int32 posStateBits = 2;
        Int32 litContextBits = 3; // for normal files
        // UInt32 litContextBits = 0; // for 32-bit data
        Int32 litPosBits = 0;
        // UInt32 litPosBits = 2; // for 32-bit data
        Int32 algorithm = 2;
        Int32 numFastBytes = 128;

        string mf = "bt4";
        bool eos = true;
        bool stdInMode = false;


        CoderPropID[] propIDs = {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };

        object[] properties = {
            (Int32)(dictionary),
            (Int32)(posStateBits),
            (Int32)(litContextBits),
            (Int32)(litPosBits),
            (Int32)(algorithm),
            (Int32)(numFastBytes),
            mf,
            eos
        };


        try
        {
            using (FileStream inStream = new FileStream(inFile, FileMode.Open))
            {
                using (FileStream outStream = new FileStream(outFile, FileMode.Create))
                {
                    SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
                    encoder.SetCoderProperties(propIDs, properties);
                    encoder.WriteCoderProperties(outStream);
                    Int64 fileSize;
                    if (eos || stdInMode)
                        fileSize = -1;
                    else
                        fileSize = inStream.Length;
                    for (int i = 0; i < 8; i++)
                    {
                        outStream.WriteByte((Byte)(fileSize >> (8 * i)));


                    }
                    encoder.Code(inStream, outStream, -1, -1, null);
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("ERROR : " + e.Message);
        }
    }
}

/// <summary>
/// 关卡数据
/// </summary>
public class LevelData
{
    //关卡名称
    public string levelName;
    //物体列表
    public List<DataType> objectsToData = new List< DataType>();

    public void AddObj(string prefabName, GameObject obj)
    {
        DataType data = new DataType(prefabName, obj.transform.position, obj.transform.eulerAngles, obj.transform.localScale);
        objectsToData.Add(data);
    }
}

/// <summary>
/// 物体数据，pos,rot,scale
/// </summary>
public class DataType
{
    public string prefabName;

    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float scaleX;
    public float scaleY;
    public float scaleZ;

    public DataType()
    {
    }

    public DataType( string name, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        prefabName = name;

        posX = position.x;
        posY = position.y;
        posZ = position.z;
        rotX = rotation.x;
        rotY = rotation.y;
        rotZ = rotation.z;
        scaleX = scale.x;
        scaleY = scale.y;
        scaleZ = scale.z;
    }

    public Vector3 GetPos()
    {
        return new Vector3(posX, posY, posZ);
    }

    public Vector3 GetRotation()
    {
        return new Vector3(rotX, rotY, rotZ);
    }

    public Vector3 GetScale()
    {
        return new Vector3(scaleX, scaleY, scaleZ);
    }
}

public class SerializeScene : ScriptableWizard
{
    string assetPath;

    [MenuItem("Tools/Serialize Scene")]
    static void SerializeOpenScene()
    {
        
        //SerializeScene ss = (SerializeScene)ScriptableWizard .DisplayWizard("Serialize Scene", typeof(SerializeScene ));
    }

    void OnWizardCreate()
    {
        // Get the path we'll use to write our assets:
        assetPath = Application.dataPath + "/Resources/" + "SceneInfo/" ;

        // Create the folder that will hold our assets:
        if (! Directory.Exists(assetPath))
        {
            Directory.CreateDirectory(assetPath);
        }

        FindAssets();

        // Make sure the new assets are (re-)imported:
        AssetDatabase.Refresh();
    }

    private void FindAssets()
    {
        List< GameObject> objList = new List <GameObject >();
        LevelData newLevel = new LevelData();

        newLevel.levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        GameObject parent = GameObject.Find( "ObjectRoot" );
        if (parent == null)
        {
            UnityEngine.Debug.LogError( "No ObjectRoot Node!");
            return;
        }

        foreach ( Transform trans in parent.transform)
        {
            UnityEngine.Debug.Log(trans.name);
            AddObjects(trans.name, trans.gameObject, ref newLevel);
        }

        string json = LitJson.JsonMapper.ToJson(newLevel);

        FileInfo file = new FileInfo(assetPath + newLevel.levelName + ".txt");
        try
        {
            file.Delete();
        }
        catch (System.IO. IOException e)
        {

            UnityEngine.Debug.Log(e.Message);
        }

        FileStream fs = new FileStream (assetPath + newLevel.levelName + ".txt", FileMode.OpenOrCreate, FileAccess .Write);
        StreamWriter sw = new StreamWriter (fs);
        sw.Write(json);
        sw.Close();
        fs.Close();
    }

    private void AddObjects( string prefabName, GameObject obj, ref LevelData level)
    {
        if ( PrefabType.PrefabInstance == PrefabUtility .GetPrefabType(obj))
        {
            level.AddObj(prefabName, obj);
        }
        else
        {
            UnityEngine.Debug.Log( "Not a Prefab!");
        }
    }
}