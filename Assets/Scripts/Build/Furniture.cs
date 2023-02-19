using UnityEngine;

/// <summary>
/// 가구에 필요한 정보들을 갖고 기능들을 수행하는 class
/// </summary>
public class Furniture : PoolObjectBase
{
    public CustomizationItem customizationItem;
    public Vector2Int furnitureSize = new Vector2Int(1,1);

    public BoxCollider2D boxCollider2D;
    public UnityEngine.Rendering.SortingGroup sortingGroup;
    public SpriteRenderer spriteRenderer;

    public FurnitureDrag furnitureDrag;

    public void SetPreview()
    {
        boxCollider2D.enabled = false;
        furnitureDrag.boxCollider2D.enabled = false;
        sortingGroup.sortingLayerName = "UI";
        sortingGroup.sortingOrder = 0;

        var color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }

    public void SetDefault()
    {
        boxCollider2D.enabled = true;
        furnitureDrag.boxCollider2D.enabled = true;
        sortingGroup.sortingLayerName = "Default";
        sortingGroup.sortingOrder = 0;

        var color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public void SetTilizedPosition(Vector2 worldPosition)
    {
        thisTransform.position = BuildManager.GetTilizedWorldPosition(worldPosition);
    }

    public int saveIndex;

    string furniturePositionSaveStr = "furniturePositionSave";

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(furniturePositionSaveStr + "." + saveIndex + ".X", transform.position.x);
        PlayerPrefs.SetFloat(furniturePositionSaveStr + "." + saveIndex + ".Y", transform.position.y);
    }

    public void LoadPosition()
    {
        thisObject.SetActive(true);

        Vector2 posi = new Vector2();
        posi.x = PlayerPrefs.GetFloat(furniturePositionSaveStr + "." + saveIndex + ".X", transform.position.x);
        posi.y = PlayerPrefs.GetFloat(furniturePositionSaveStr + "." + saveIndex + ".Y", transform.position.y);

        thisTransform.position = posi;
    }
}