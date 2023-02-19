using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설치된 가구 item들의 정보를 갖거나 세이브 로드 기능등을 실행하는 singleton class
/// </summary>
public class BuildManager : MySingleton<BuildManager>
{
    #region PositionInformations
    static Vector2 tilePivot = Vector2.one * -9;
    static int tileSize = 18;

    public static Vector2Int WorldPositionToTilePosition(Vector2 v2)
    {
        var tmpV2 = new Vector2Int();

        if (v2.x > 0)
            tmpV2.x = (int)(v2.x - tilePivot.x) / tileSize;
        else
            tmpV2.x = (int)(v2.x - tileSize - tilePivot.x) / tileSize;

        if (v2.y > 0)
            tmpV2.y = (int)(v2.y - tilePivot.y) / tileSize;
        else
            tmpV2.y = (int)(v2.y - tileSize - tilePivot.y) / tileSize;

        return tmpV2;
    }

    public static Vector2 TilePositionToWorldPosition(Vector2Int v2)
    {
        var tempV2 = v2 * tileSize;
        return tempV2;
    }

    public static Vector2 GetTilizedWorldPosition(Vector2 v2)
    {
        return TilePositionToWorldPosition(WorldPositionToTilePosition(v2));
    }
    #endregion

    public LayerMask buildLayerMask;
    public LayerMask blockLayerMask;
    public LayerMask furnitureDragLayerMask;

    public PoolParent buildTilePoolParent;
    public PoolParent[] furniturePoolParents;

    public BuildController buildController;
    public RoomController roomController;

    public void Init()
    {
        for (int i =0;i< furniturePoolParents.Length; i++)
            furniturePoolParents[i].poolSample.poolParent = furniturePoolParents[i];
        LoadFurnitrues();
    }

    public List<Furniture> furnitureList;
    public IntSaver furnitureCountSaver = new IntSaver("furnitureSave.CountSaver");

    public void LoadFurnitrues()
    {
        furnitureList = new List<Furniture>(furnitureCountSaver.GetValue());
        for (int i = 0; i < furnitureCountSaver.GetValue(); i++)
        {
            var targetFurniture = GetSaveFurniture(i);
            if (targetFurniture == null)
            {
                furnitureList.Add(null);
            }
            else
            {
                var popFurniture = targetFurniture.DoPop<Furniture>();
                InsertFurniture(popFurniture, i);
                popFurniture.LoadPosition();
            }
        }
    }

    public void InsertFurniture(Furniture furniture, int index, bool doSave = false)
    {
        furniture.saveIndex = index;

        if (furnitureList.Count <= index)
            furnitureList.Add(furniture);
        else
            furnitureList[index] = furniture;

        if (doSave)
        {
            furnitureCountSaver.SetValue(furnitureList.Count);
            furniture.SavePosition();
            SetSaveFurniture(furniture, index);
        }
    }

    public void AddFurnitrue(Furniture furniture, bool doSave = false)
    {
        int index = -1;

        for (int i = 0; i < furnitureList.Count; i++)
        {
            if(furnitureList[i] == null)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            index = furnitureList.Count;
            furnitureList.Add(furniture);
        }
        else
            furnitureList[index] = furniture;

        furniture.saveIndex = index;

        if (doSave)
        {
            furnitureCountSaver.SetValue(furnitureList.Count);
            furniture.SavePosition();
            SetSaveFurniture(furniture, index);
        }
    }

    public void RemoveFurniture(Furniture furniture, bool doSave = false)
    {
        int index = furniture.saveIndex;
        furnitureList[index] = null;

        if (doSave)
        {
            furnitureCountSaver.SetValue(furnitureList.Count);
            SetSaveFurniture(null, index);
        }
    }

    string furnitureSaveIdNumStr = "furnitureSave.idNumber";

    public void SetSaveFurniture(Furniture furniture, int saveIndex)
    {
        if (furniture == null)
            PlayerPrefs.SetInt(furnitureSaveIdNumStr + "." + saveIndex, -1);
        else
            PlayerPrefs.SetInt(furnitureSaveIdNumStr + "." + saveIndex, furniture.customizationItem.myIdNumber);
    }

    public Furniture GetSaveFurniture(int saveIndex)
    {
        int itemIdNum = PlayerPrefs.GetInt(furnitureSaveIdNumStr + "." + saveIndex, -1);

        if (itemIdNum == -1)
            return null;
        else
            return DataDictionary.Instance.customizationItems[itemIdNum].furniturePrefab;
    }

    [InspectorButton]
    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}