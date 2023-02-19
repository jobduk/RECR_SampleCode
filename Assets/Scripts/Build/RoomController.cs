using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ������ �����ϴ� Tile�� �������� ���� class
/// </summary>
[System.Serializable]
public class TilemapAreaInformation
{
    public Tilemap tileMap;
    public Vector2Int startPosition;
    public Vector2Int size;
    public CustomizationItem defaultItem;

    public IntSaver equipItemNumberSaver;

    public void Init()
    {
        if (equipItemNumberSaver.GetValue() == -1)
            SetTile(defaultItem);
        else
            SetTile(DataDictionary.Instance.customizationItems[equipItemNumberSaver.GetValue()]);
    }

    public void SetTile(CustomizationItem customizationItem)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                tileMap.SetTile((Vector3Int)startPosition + new Vector3Int(x, y, 0), customizationItem.tileInformation.tileAsset);
            }
        }
        equipItemNumberSaver.SetValue(customizationItem.myIdNumber);
    }

    public CustomizationItem GetEquipCustomItem()
    {
        if (equipItemNumberSaver.GetValue() == -1)
            return defaultItem;
        return
            DataDictionary.Instance.customizationItems[equipItemNumberSaver.GetValue()];
    }
}

/// <summary>
/// Room���� Tilemap Customization�� �ʿ��� �������� ���� class
/// </summary>
public class RoomController : MonoBehaviour
{
    public TilemapAreaInformation tilemapAreaInformation_Ground;
    public TilemapAreaInformation tilemapAreaInformation_Wall;

    public void Init()
    {
        tilemapAreaInformation_Ground.Init();
        tilemapAreaInformation_Wall.Init();
    }
}