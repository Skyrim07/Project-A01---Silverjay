using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public sealed class GlobalManager : MonoSingleton<GlobalManager>
{
    [HideInInspector] public SKLocalizationAsset localAsset;
    [HideInInspector] public LanguageSupport language;
    public LanguageSupport defaultLanaguage;
    IEnumerator Start()
    {
        language = defaultLanaguage;
        SKEnvironment.curLanguage = language;
        localAsset = new SKLocalizationAsset(CommonUtils.SKLoadObjectFromJson<SKLocalizationAssetJson>("SKLocalizationAsset.json"));
        InitializeLocalizationAssetDictionary();

        yield return new WaitForSeconds(1);
        SKLocalizationManager.LocalizeAll(language);
    }

    public void SetLanguage(LanguageSupport language)
    {
        this.language = language;
        SKEnvironment.curLanguage = this.language;
    }

    private void InitializeLocalizationAssetDictionary()
    {
        foreach (var item in localAsset.textConfigs)
        {
            CommonUtils.InsertOrUpdateKeyValueInDictionary(localAsset.textConfigDict, item.id, item);
        }
        foreach (var item in localAsset.imageConfigs)
        {
            CommonUtils.InsertOrUpdateKeyValueInDictionary(localAsset.imageConfigDict, item.id, item);
        }
    }
    public string GetLocalizationText(int localID)
    {
        string s = localAsset.textConfigDict[localID].localTexts[(int)language];
        s = s.Replace(@"\r", "\r");
        s = s.Replace(@"\n", "\n");
        return s;
    }
    public string GetLocalizationText(int localID, LanguageSupport language)
    {
        string s = localAsset.textConfigDict[localID].localTexts[(int)language];
        s = s.Replace(@"\r", "\r");
        s = s.Replace(@"\n", "\n");
        return s;
    }
    public Texture2D GetLocalizationImage(int localID)
    {
        return localAsset.imageConfigDict[localID].localImages[(int)language];
    }
    public Texture2D GetLocalizationImage(int localID, LanguageSupport language)
    {
        return localAsset.imageConfigDict[localID].localImages[(int)language];
    }
}
