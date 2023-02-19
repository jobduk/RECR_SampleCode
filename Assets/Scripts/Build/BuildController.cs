using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 가구의 설치, 드래그, 제거 등의 기능을하는 class
/// </summary>
public class BuildController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public TutorialFurniture tutorialFurniture;

    [System.NonSerialized]
    public bool isBuildModeOn;
    TilemapAreaInformation targetTilemapAreaInformation;
    Furniture targetFurniture;

    public BoxCollider2D touchCol;

    public void BuildModeOn(TilemapAreaInformation tilemapAreaInformation, Furniture furniture)
    {
        isBuildModeOn = true;
        targetTilemapAreaInformation = tilemapAreaInformation;
        targetFurniture = furniture;
        SetBuildTiles();
        touchCol.enabled = true;
    }

    public void ChangeTargetFurniture(Furniture furniture)
    {
        targetFurniture = furniture;
    }

    void SetBuildTiles()
    {
        ClearBuildTiles();
        for (int y = 0; y < targetTilemapAreaInformation.size.y; y++)
        {
            for (int x = 0; x < targetTilemapAreaInformation.size.x; x++)
            {
                var setTilePosition = targetTilemapAreaInformation.startPosition + new Vector2Int(x, y);
                var setWorldPosition = BuildManager.TilePositionToWorldPosition(setTilePosition);

                if (CheckCanBuildPosition(setWorldPosition))
                {
                    var buildTile = BuildManager.Instance.buildTilePoolParent.DoPop<BuildTile>();
                    buildTile.ActiveObject(setWorldPosition);
                }
            }
        }
    }

    public bool CheckCanBuildPosition(Vector2 position)
    {
        var col = Physics2D.OverlapPoint(position, BuildManager.Instance.buildLayerMask);
        var tilePosition = BuildManager.WorldPositionToTilePosition(position);

        if (!CheckPositionInTilemapArea(tilePosition))
            return false;

        if (col != null)
            return false;
        else
        {
            if (tilePosition == BuildManager.WorldPositionToTilePosition(GameManager.Instance.characterMain.transform.position))
                return false;
            else
                return true;
        }
    }

    public bool CheckPositionInTilemapArea(Vector2Int tilePosition)
    {
        if (tilePosition.x < targetTilemapAreaInformation.startPosition.x || tilePosition.x >= (targetTilemapAreaInformation.startPosition.x + targetTilemapAreaInformation.size.x) ||
            tilePosition.y < targetTilemapAreaInformation.startPosition.y || tilePosition.y >= (targetTilemapAreaInformation.startPosition.y + targetTilemapAreaInformation.size.y))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckCanBuildFurniture(Furniture furniture)
    {
        Vector2Int center = BuildManager.WorldPositionToTilePosition(furniture.thisTransform.position);
        for (int y = 0; y < furniture.furnitureSize.y; y++)
        {
            for (int x = 0; x < furniture.furnitureSize.x; x++)
            {
                Vector2 checkPosition = BuildManager.TilePositionToWorldPosition(center + new Vector2Int(x, y));
                if (!CheckCanBuildPosition(checkPosition))
                    return false;
            }
        }

        return true;
    }

    public void MakeRedTiles(Furniture furniture)
    {
        Vector2Int center = BuildManager.WorldPositionToTilePosition(furniture.thisTransform.position);
        for (int y = 0; y < furniture.furnitureSize.y; y++)
        {
            for (int x = 0; x < furniture.furnitureSize.x; x++)
            {
                Vector2 checkPosition = BuildManager.TilePositionToWorldPosition(center + new Vector2Int(x, y));
                if (!CheckCanBuildPosition(checkPosition))
                {
                    var buildTile = BuildManager.Instance.buildTilePoolParent.DoPop<BuildTile>();
                    buildTile.ActiveObject(checkPosition, true);
                }       
            }
        }
    }

    public bool CheckFurnitureInTilemapArea(Furniture furniture)
    {
        if (furniture.customizationItem.customizationType == CustomizationType.Furinture_Wall)
            return targetTilemapAreaInformation == GameManager.Instance.titleController.roomController.tilemapAreaInformation_Wall;
        else
            return targetTilemapAreaInformation == GameManager.Instance.titleController.roomController.tilemapAreaInformation_Ground;
    }

    void ClearBuildTiles()
    {
        BuildManager.Instance.buildTilePoolParent.ClearAll();
    }

    public void BuildModeOff()
    {
        isBuildModeOn = true;
        ClearBuildTiles();
        touchCol.enabled = false;
    }

    Furniture previewFurniture;
    bool isPreviewOn = false;

    void SetPreviewFurniturePosition(Vector2 targetPosition)
    {
        previewFurniture.SetTilizedPosition(targetPosition);
        SetBuildTiles();
        MakeRedTiles(previewFurniture);
    }

    void PopPreviewFurniture(Vector2 targetPosition)
    {
        SetFurniturePreview(targetFurniture.DoPop<Furniture>(), targetPosition);
        previewFurniture.thisObject.SetActive(true);
    }

    void SetFurniturePreview(Furniture furniture, Vector2 targetPosition)
    {
        isPreviewOn = true;

        previewFurniture = furniture;
        previewFurniture.SetPreview();

        SetPreviewFurniturePosition(targetPosition);
    }

    void InstallPreviewFurniture(Vector2 targetPosition)
    {
        SetPreviewFurniturePosition(targetPosition);
        previewFurniture.SetDefault();
        BuildManager.Instance.AddFurnitrue(previewFurniture, true);
        SetBuildTiles();

        isPreviewOn = false;

        tutorialFurniture.TryPlayTutorial();
    }

    void FurnitureToDrag(Furniture furniture, Vector2 targetPosition)
    {
        SetFurniturePreview(furniture, targetPosition);
        BuildManager.Instance.RemoveFurniture(furniture, true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var touchPosition = eventData.pointerCurrentRaycast.worldPosition;
        var rayCol = Physics2D.OverlapPoint(touchPosition, BuildManager.Instance.furnitureDragLayerMask);

        if (rayCol == null)
        {
            if (CheckPositionInTilemapArea(BuildManager.WorldPositionToTilePosition(touchPosition)))
                PopPreviewFurniture(touchPosition);
        }
        else
        {
            var dragFurniture = rayCol.GetComponent<FurnitureDrag>().furniture;
            if (CheckFurnitureInTilemapArea(dragFurniture))
                FurnitureToDrag(dragFurniture, touchPosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var touchPosition = eventData.pointerCurrentRaycast.worldPosition;

        if (isPreviewOn)
        {
            if(CheckCanBuildFurniture(previewFurniture))
                InstallPreviewFurniture(touchPosition);
            else
            {
                previewFurniture.PushThis();
                isPreviewOn = false;
            }
            SetBuildTiles();
            isDragOn = false;
            GameManager.Instance.titleController.customPanel_Room.trashCan.DoOff();
        }
    }

    bool isDragOn = false;

    public void OnDrag(PointerEventData eventData)
    {
        if (isPreviewOn)
        {
            var touchPosition = eventData.pointerCurrentRaycast.worldPosition;
            SetPreviewFurniturePosition(touchPosition);
            if (!isDragOn)
            {
                isDragOn = true;
                GameManager.Instance.titleController.customPanel_Room.trashCan.DoOn();
            }
        }
    }
}