using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum CustomizationType
{
    Tile,
    Wallpaper,
    Furniture_Ground,
    Furinture_Wall,
    Hair,
    Cloth,
    Dye
}

/// <summary>
/// Customization에 필요한 아이템의 정보, 기능들을 담고 있는 class
/// </summary>
[CreateAssetMenu]
public class CustomizationItem : ScriptableObject
{
    [HideInInspector]
    public int myIdNumber;
    [HideInInspector]
    public int myTypeIdNumber;

    public CustomizationType customizationType;
    public Sprite iconSprite;

    [ShowIf("IsTileItem")]
    public TileInformation tileInformation;
    [ShowIf("IsCharacterCustomItem")]
    public CustomCharacterInformation customCharacterInformation;
    [ShowIf("IsFurniture")]
    public Furniture furniturePrefab;

    [System.Serializable]
    public class TileInformation
    {
        public UnityEngine.Tilemaps.TileBase tileAsset;
        public Sprite titleIconSprite;
    }

    [System.Serializable]
    public class CustomCharacterInformation
    {
        public SpriteCollector spriteCollector;
    }

    public bool IsTileItem()
    {
        return CheckTileItem(customizationType);
    }

    public bool IsCharacterCustomItem()
    {
        return CheckCharacterCustomItem(customizationType);
    }

    public bool IsFurniture()
    {
        return CheckFurniture(customizationType);
    }

    public static bool CheckTileItem(CustomizationType customType)
    {
        return customType == CustomizationType.Tile || customType == CustomizationType.Wallpaper;
    }

    public static bool CheckCharacterCustomItem(CustomizationType customType)
    {
        return customType == CustomizationType.Hair || customType == CustomizationType.Cloth;
    }

    public static bool CheckFurniture(CustomizationType customType)
    {
        return customType == CustomizationType.Furinture_Wall || customType == CustomizationType.Furniture_Ground;
    }

#if UNITY_EDITOR
    [InspectorButton]
    public void CacheResources()
    {
        string filePath = AssetDatabase.GetAssetPath(this);

        if (filePath.Contains("Tile_Ground"))
            customizationType = CustomizationType.Tile;
        else if (filePath.Contains("Tile_Wallpaper"))
            customizationType = CustomizationType.Wallpaper;
        else if (filePath.Contains("Furnitures_Ground"))
            customizationType = CustomizationType.Furniture_Ground;
        else if (filePath.Contains("Furnitures_Wall"))
            customizationType = CustomizationType.Furinture_Wall;
        else if (filePath.Contains("Hairs"))
            customizationType = CustomizationType.Hair;
        else if (filePath.Contains("Clothes"))
            customizationType = CustomizationType.Cloth;
        else if (filePath.Contains("Dyes"))
            customizationType = CustomizationType.Dye;

        iconSprite = FindAsset<Sprite>("Icon");
        if (IsCharacterCustomItem())
        {
            customCharacterInformation.spriteCollector = FindAsset<SpriteCollector>();
        }
        else
        {
            if (IsTileItem())
            {
                tileInformation.titleIconSprite = FindAsset<Sprite>("IconTitle");
                tileInformation.tileAsset = FindAsset<UnityEngine.Tilemaps.TileBase>("TileAsset");
            }
            else if (IsFurniture())
            {
                furniturePrefab = FindAsset<GameObject>().GetComponent<Furniture>();
                furniturePrefab.customizationItem = this;
                EditorUtility.SetDirty(furniturePrefab);
            }
        }

        EditorUtility.SetDirty(this);
    }

    T FindAsset<T>(string assetName = "") where T : Object
    {
        string filePath = AssetDatabase.GetAssetPath(this);
        string folderPath = filePath.Remove(filePath.LastIndexOf('/'));

        var assets = AssetDatabase.FindAssets("t:" + typeof(T).Name, new string[] { folderPath });

        for (int i = 0; i < assets.Length; i++)
        {
            string dPath = AssetDatabase.GUIDToAssetPath(assets[i]);
            T getAsset = AssetDatabase.LoadAssetAtPath(dPath, typeof(T)) as T;
            if (getAsset != null)
            {
                if (getAsset.name == assetName || assetName == "")
                    return getAsset;
            }
        }

        return null;
    }
#endif
}