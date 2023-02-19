using System.Collections.Generic;
using UnityEngine;

#region SaverClasses
[System.Serializable]
public class IntSaver
{
    [SerializeField]
    string saveName;
    [SerializeField]
    int deafaultValue;
    bool isInit;
    int nowValue;

    public int GetValue()
    {
        if (!isInit)
        {
            isInit = true;
            nowValue = PlayerPrefs.GetInt(saveName, deafaultValue);
        }
        return nowValue;
    }

    public void SetValue(int value)
    {
        if (!isInit)
            isInit = true;
        nowValue = value;
        PlayerPrefs.SetInt(saveName, nowValue);
    }

    public IntSaver(string saveName, int def = 0)
    {
        this.saveName = saveName;
        deafaultValue = def;
    }
}

[System.Serializable]
public class BoolSaver
{
    [SerializeField]
    string saveName;
    [SerializeField]
    bool deafaultValue;
    bool isInit;
    bool nowValue;

    public void SetNotInit()
    {
        isInit = false;
        nowValue = deafaultValue;
    }

    public bool GetValue()
    {
        if (!isInit)
        {
            isInit = true;
            nowValue = IntToBool(PlayerPrefs.GetInt(saveName, BoolToInt(deafaultValue)));
        }
        return nowValue;
    }
    public void SetValue(bool value)
    {
        if (!isInit)
        {
            isInit = true;
        }
        nowValue = value;
        PlayerPrefs.SetInt(saveName, BoolToInt(value));
    }

    public BoolSaver(string name, bool def = false)
    {
        saveName = name;
        deafaultValue = def;
    }

    bool IntToBool(int i)
    {
        if (i == 0)
            return false;
        else
            return true;
    }

    int BoolToInt(bool b)
    {
        if (b)
            return 1;
        else
            return 0;
    }
}

[System.Serializable]
public class Vector2Saver
{
    [SerializeField]
    string saveName;
    [SerializeField]
    Vector2 deafaultValue;
    bool isInit;
    Vector2 nowValue;

    public Vector2 GetValue()
    {
        if (!isInit)
        {
            isInit = true;
            nowValue = GetVetor2(deafaultValue);
        }
        return nowValue;
    }

    public void SetValue(Vector2 v2)
    {
        if (!isInit)
            isInit = true;
        nowValue = v2;
        SetVetor2(nowValue);
    }

    public Vector2Saver(string saveName, Vector2 defV2 = default)
    {
        this.saveName = saveName;
        deafaultValue = defV2;
    }

    string saveNameX;
    string saveNameY;

    bool isSaveNameInit = false;
    void CheckSaveNameInit()
    {
        if (!isSaveNameInit)
        {
            saveNameX = saveName + ".X";
            saveNameY = saveName + ".Y";
            isSaveNameInit = true;
        }
    }

    bool isWriteV2Init = false;
    Vector2 tempV2 = new Vector2();

    Vector2 GetVetor2(Vector2 v2 = default)
    {
        CheckSaveNameInit();
        if (!isWriteV2Init)
        {
            isWriteV2Init = true;
            tempV2.x = PlayerPrefs.GetFloat(saveNameX, v2.x);
            tempV2.y = PlayerPrefs.GetFloat(saveNameY, v2.x);
        }
        return tempV2;
    }

    void SetVetor2(Vector2 v2)
    {
        CheckSaveNameInit();
        if (!isWriteV2Init)
            isWriteV2Init = true;
        tempV2 = v2;
        PlayerPrefs.SetFloat(saveNameX, tempV2.x);
        PlayerPrefs.SetFloat(saveNameY, tempV2.y);
    }
}
#endregion

/// <summary>
/// Customization Item 등의 Asset 정보들을 갖고 분류해주는 singleton class
/// </summary>
public class DataDictionary : MySingleton<DataDictionary>
{
    public CustomizationItem[] customizationItems;

    public Dictionary<CustomizationType, List<CustomizationItem>> customTypeToItems;

    public void Init()
    {
        ItemsAddToDictionary();
    }

    void ItemsAddToDictionary()
    {
        int typeLength = System.Enum.GetValues(typeof(CustomizationType)).Length;
        customTypeToItems = new Dictionary<CustomizationType, List<CustomizationItem>>(typeLength);

        for (int i = 0; i < typeLength; i++)
        {
            CustomizationType targetType = (CustomizationType)i;
            customTypeToItems.Add(targetType, new List<CustomizationItem>());
        }

        for (int i = 0; i < customizationItems.Length; i++)
        {
            CustomizationItem targetItem = customizationItems[i];
            targetItem.myIdNumber = i;
            targetItem.myTypeIdNumber = customTypeToItems[targetItem.customizationType].Count;
            customTypeToItems[targetItem.customizationType].Add(targetItem);
        }
    }
}