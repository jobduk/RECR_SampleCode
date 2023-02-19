using UnityEngine;

/// <summary>
/// 2D Sprite Animation에서 Customzization할 수 있게 해주는 class
/// </summary>
public class SpriteChanger : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public SpriteDictionary spriteDictionary;
    public SpriteCollector mySpriteCollector;

    bool isActive = true;

    public void SetActive(bool b)
    {
        isActive = b;
    }

    public void SetCollector(SpriteCollector spriteCollector)
    {
        mySpriteCollector = spriteCollector;
        GraphicUpdate();
    }

    void LateUpdate()
    {
        GraphicUpdate();
    }

    public void GraphicUpdate()
    {
        if (isActive)
        {
            if (mySpriteRenderer.sprite != null)
            {
                if (mySpriteCollector != null)
                {
                    if (spriteDictionary.GetSpirteToInt().ContainsKey(mySpriteRenderer.sprite))
                    {
                        mySpriteRenderer.sprite = mySpriteCollector.sprites[spriteDictionary.GetSpirteToInt()[mySpriteRenderer.sprite]];
                    }
                }
            }
        }
    }

    void Reset()
    {
        if (GetComponent<SpriteRenderer>())
            mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
