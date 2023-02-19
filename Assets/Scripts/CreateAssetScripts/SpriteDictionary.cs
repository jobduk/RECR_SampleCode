using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite들을 index값으로 Dictionary에 매칭시켜놓는 class
/// </summary>
[CreateAssetMenu]
public class SpriteDictionary : ScriptableObject
{
    Dictionary<Sprite, int> spriteToIndexDic;
    public SpriteCollector defaultSpriteCollector;

    public Dictionary<Sprite, int> GetSpirteToInt()
    {
        if (spriteToIndexDic == null)
            MakeDictionary();
        return spriteToIndexDic;
    }

    public void MakeDictionary()
    {
        spriteToIndexDic = new Dictionary<Sprite, int>(defaultSpriteCollector.sprites.Length);

        for (int i = 0; i < defaultSpriteCollector.sprites.Length; i++)
            spriteToIndexDic.Add(defaultSpriteCollector.sprites[i], i);
    }
}