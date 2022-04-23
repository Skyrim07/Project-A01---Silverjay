using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SKCell
{
    /// <summary>
    /// Store references to various SK assets
    /// </summary>
    public static class SKAssetLibrary
    {
        public const string LOCAL_ASSET_PATH = "Assets/SKCell/Resources/SKCell/SKLocalizationConfigAsset.asset";
        public const string FONT_ASSET_PATH = "Assets/SKCell/Resources/SKCell/SKFontAsset.asset";
        public const string ENV_ASSET_PATH = "Assets/SKCell/Resources/SKCell/SKEnvironmentAsset.asset";
        public const string UI_ANIM_DIR_PATH = "Assets/SKCell/Resources/SKCell/Animations";
        public const string UI_ANIM_PRESET_DIR_PATH = "SKCell/Animations/Presets";
        public const string MATERIAL_PATH = "Assets/SKCell/Resources/SKCell/Materials";

        public const string PREFAB_PATH = "Assets/SKCell/Resources/SKCell/Prefabs";
        public const string RESOURCES_JSON_PATH_SUFFIX = "Assets/SKCell//Resources/SKCell/Json/";
        public const string PANEL_PREFAB_PATH = "/Resources/SKCell/UI/Panels";

        public const string JSON_PATH_SUFFIX = "/Json/";
        public const string RES_JSON_PATH_SUFFIX = "Assets/SKCell/Resources/SKCell/Json/";

        public const string RES_SPRITE_PATH = "SKCell/Sprites/";

        public static void SaveJsonFile(object obj, string fileName)
        {
            string js = JsonUtility.ToJson(obj);
            string path = RES_JSON_PATH_SUFFIX + fileName +".txt";
            File.WriteAllText(path, js);
        }

        public static T LoadJsonFile<T>(string fileName)
        {
            return JsonUtility.FromJson<T>(RES_JSON_PATH_SUFFIX + fileName + ".txt");
        }

        public static Sprite LoadSprite(string fileName)
        {
            return Resources.Load<Sprite>(RES_SPRITE_PATH + fileName);
        }
        public static Texture LoadTexture(string fileName)
        {
            return Resources.Load<Texture>(RES_SPRITE_PATH + fileName);
        }

        private static SKLocalizationAsset localizationAsset;
        public static SKLocalizationAsset LocalizationAsset
        {
            get
            {
#if UNITY_EDITOR
                if (localizationAsset == null)
                    localizationAsset = AssetDatabase .LoadAssetAtPath<SKLocalizationAsset>(LOCAL_ASSET_PATH);
#endif
                if (localizationAsset == null)
                    localizationAsset = new SKLocalizationAsset(CommonUtils.SKLoadObjectFromJson<SKLocalizationAssetJson>("SKLocalizationAsset.json"));

                return localizationAsset;
            }
            set
            {
                localizationAsset = value;
            }
        }
        private static SKFontAsset fontAsset;
        public static SKFontAsset FontAsset
        {
            get
            {
#if UNITY_EDITOR
                if (fontAsset == null)
                    fontAsset = AssetDatabase.LoadAssetAtPath<SKFontAsset>(FONT_ASSET_PATH);
#endif

                if (fontAsset == null)
                    fontAsset = Resources.Load<SKFontAsset>(FONT_ASSET_PATH.Substring(FONT_ASSET_PATH.IndexOf("SKCell")));
                return fontAsset;
            }
        }
        private static SKEnvironmentAsset envAsset;
        public static SKEnvironmentAsset EnvAsset
        {
            get
            {
#if UNITY_EDITOR
                if (envAsset == null)
                    envAsset = AssetDatabase.LoadAssetAtPath<SKEnvironmentAsset>(ENV_ASSET_PATH);
#endif
                if (envAsset == null)
                    envAsset = Resources.Load<SKEnvironmentAsset>(ENV_ASSET_PATH.Substring(ENV_ASSET_PATH.IndexOf("SKCell")));
                return envAsset;
            }
        }
        private static Material gridCellMat;
        public static Material GridCellMat
        {
            get
            {
#if UNITY_EDITOR
                if (gridCellMat == null)
                    gridCellMat = AssetDatabase.LoadAssetAtPath<Material>(MATERIAL_PATH+"/GridCellMat.mat");
#endif
                if (gridCellMat == null)
                    gridCellMat = Resources.Load<Material>(MATERIAL_PATH.Substring(MATERIAL_PATH.IndexOf("SKCell")) + "/GridCellMat");
                return gridCellMat;
            }
        }

        public static void Initialize()
        {
           // localizationAsset = null;
          //  fontAsset=null;
           // envAsset = null;
        }
    }
}
