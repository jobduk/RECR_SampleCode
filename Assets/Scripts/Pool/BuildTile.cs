using UnityEngine;

/// <summary>
/// 가구의 설치 가능여부를 보여주는 타일 class
/// </summary>
public class BuildTile : PoolObjectBase
{
    public SpriteRenderer spriteRenderer;
    public Sprite greenTile;
    public Sprite redTile;

    public void ActiveObject(Vector2 position ,bool isRed = false)
    {
        thisTransform.position = position;
        if (isRed)
            spriteRenderer.sprite = redTile;
        else
            spriteRenderer.sprite = greenTile;

        thisObject.SetActive(true);
    }
}
