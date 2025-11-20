using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TEngine.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class SpritePostprocessor : AssetPostprocessor
    {
        private static List<string> m_resourcesToDelete = new List<string>();

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            m_resourcesToDelete.Clear();
            var config = AtlasConfiguration.Instance;

            if (!config.autoGenerate) return;

            try
            {
                ProcessAssetChanges(
                    importedAssets: importedAssets,
                    deletedAssets: deletedAssets,
                    movedAssets: movedAssets,
                    movedFromPaths: movedFromAssetPaths
                );
            }
            catch (Exception e)
            {
                Debug.LogError($"Atlas processing error: {e.Message}\n{e.StackTrace}");
            }
            finally
            {
                bool isDelete = m_resourcesToDelete.Count > 0;
                foreach (var res in m_resourcesToDelete)
                {
                    AssetDatabase.DeleteAsset(res);
                }
                if (isDelete)
                {
                    Debug.LogError($"<color=red>针对 {config.sourceAtlasRootDir} 路径下资源</color>\n<color=red>移除了空格和同名资源，请检查重新合入相关资源</color>");
                }
                AssetDatabase.Refresh();
            }
        }

        private static void ProcessAssetChanges(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromPaths)
        {
            ProcessAssets(importedAssets, (path) =>
            {
                EditorSpriteSaveInfo.OnImportSprite(path);
                LogProcessed("[Added]", path);
            });

            ProcessAssets(deletedAssets, (path) =>
            {
                EditorSpriteSaveInfo.OnDeleteSprite(path);
                LogProcessed("[Deleted]", path);
            });

            ProcessMovedAssets(movedFromPaths, movedAssets);
        }

        private static void ProcessAssets(string[] assets, Action<string> processor, bool isDelete = false)
        {
            if (assets == null) return;

            foreach (var asset in assets)
            {
                if (ShouldProcessAsset(asset))
                {
                    if (!isDelete && (CheckFileNameContainsSpace(asset) || CheckDuplicateAssetName(asset) || ChangeSpriteTextureType(asset)))
                    {
                        continue;
                    }

                    processor?.Invoke(asset);
                }
            }
        }

        private static bool ChangeSpriteTextureType(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null)
            {
                return false;
            }
            bool isChange = false;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                isChange = true;
            }

            if (AtlasConfiguration.Instance.checkMipmaps)
            {
                if (AtlasConfiguration.Instance.enableMipmaps && !importer.mipmapEnabled)
                {
                    importer.mipmapEnabled = true;
                    isChange = true;
                }
                else if (!AtlasConfiguration.Instance.enableMipmaps && importer.mipmapEnabled)
                {
                    importer.mipmapEnabled = false;
                    isChange = true;
                }
            }

            if (isChange)
            {
                LogProcessed("[Sprite Import Changed Reimport]", path);
                importer.SaveAndReimport();
            }
            return isChange;
        }

        private static bool CheckFileNameContainsSpace(string assetPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(assetPath);

            if (fileName.Contains(" "))
            {
                m_resourcesToDelete.Add(assetPath);
                Debug.LogError($"<color=red>发现资源名存在空格: {assetPath}</color>");
                return true;
            }
            return false;
        }

        private static bool CheckDuplicateAssetName(string assetPath)
        {
            var currentFileName = Path.GetFileNameWithoutExtension(assetPath);

            string rootDir = "";
            var tempRootDirArr = new List<string>(AtlasConfiguration.Instance.sourceAtlasRootDir);
            tempRootDirArr.AddRange(AtlasConfiguration.Instance.rootChildAtlasDir);
            foreach (var rootPath in tempRootDirArr)
            {
                var tempPath = rootPath.Replace("\\", "/");
                if (!assetPath.StartsWith(tempPath))
                {
                    continue;
                }
                rootDir = tempPath;
            }
            // var rootDir = AtlasConfiguration.Instance.sourceAtlasRootDir;
            if (string.IsNullOrEmpty(rootDir))
            {
                return false;
            }

            // 获取当前目录下所有图片文件
            var filesInDirectory = Directory.GetFiles(rootDir, "*.*", SearchOption.AllDirectories)
                .Where(CheckIsValidImageFile)
                .ToArray();
            var normalizedCurrentPath = Path.GetFullPath(assetPath).Replace("\\", "/");
            foreach (var file in filesInDirectory)
            {
                var normalizedFile = Path.GetFullPath(file).Replace("\\", "/");
                if (normalizedFile.Equals(normalizedCurrentPath, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // 跳过自身
                }

                var otherFileName = Path.GetFileNameWithoutExtension(file);
                if (string.Equals(currentFileName, otherFileName, StringComparison.OrdinalIgnoreCase))
                {
                    m_resourcesToDelete.Add(assetPath);
                    Debug.LogError($"<color=red>发现同名资源冲突: 合入资源: {assetPath} 存在资源: {file}</color>");
                    return true;
                }
            }

            return false;
        }

        private static bool CheckIsValidImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            return ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg");
        }

        private static void ProcessMovedAssets(string[] oldPaths, string[] newPaths)
        {
            if (oldPaths == null || newPaths == null) return;

            for (int i = 0; i < oldPaths.Length; i++)
            {
                if (ShouldProcessAsset(oldPaths[i]))
                {
                    EditorSpriteSaveInfo.OnDeleteSprite(oldPaths[i]);
                    LogProcessed("[Moved From]", oldPaths[i]);
                    EditorSpriteSaveInfo.MarkParentAtlasesDirty(oldPaths[i], true);
                }

                if (ShouldProcessAsset(newPaths[i]))
                {
                    if (CheckFileNameContainsSpace(newPaths[i]) || CheckDuplicateAssetName(newPaths[i]) || ChangeSpriteTextureType(newPaths[i]))
                    {
                        continue;
                    }
                    EditorSpriteSaveInfo.OnImportSprite(newPaths[i]);
                    LogProcessed("[Moved To]", newPaths[i]);
                    EditorSpriteSaveInfo.MarkParentAtlasesDirty(newPaths[i], false);
                }
            }
        }

        private static bool ShouldProcessAsset(string assetPath)
        {
            var config = AtlasConfiguration.Instance;

            if (string.IsNullOrEmpty(assetPath)) return false;
            if (assetPath.StartsWith("Packages/")) return false;

            if (!CheckIsShowProcessPath(assetPath)) return false;
            if (CheckIsExcludeFolder(assetPath)) return false;

            if (!IsValidImageFile(assetPath)) return false;

            foreach (var keyword in config.excludeKeywords)
            {
                if (assetPath.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    return false;
            }

            return true;
        }

        private static bool CheckIsShowProcessPath(string assetPath)
        {
            var tempRootDirArr = new List<string>(AtlasConfiguration.Instance.sourceAtlasRootDir);
            tempRootDirArr.AddRange(AtlasConfiguration.Instance.rootChildAtlasDir);
            foreach (var rootPath in tempRootDirArr)
            {
                var tempPath = rootPath.Replace("\\", "/").TrimEnd('/');
                if (!assetPath.StartsWith(tempPath + "/"))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private static bool CheckIsExcludeFolder(string assetPath)
        {
            foreach (var rootPath in AtlasConfiguration.Instance.excludeFolder)
            {
                var tempPath = rootPath.Replace("\\", "/").TrimEnd('/');
                if (assetPath.StartsWith(tempPath + "/"))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsValidImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            return ext switch
            {
                ".png" => true,
                ".jpg" => true,
                ".jpeg" => true,
                _ => false
            };
        }

        private static void LogProcessed(string operation, string path)
        {
            if (AtlasConfiguration.Instance.enableLogging)
            {
                Debug.Log($"{operation} {Path.GetFileName(path)}\nPath: {path}");
            }
        }
    }
}