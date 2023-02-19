using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

/// <summary>
/// Sprite를 모아놓는 class
/// </summary>
[CreateAssetMenu]
public class SpriteCollector : ScriptableObject
{
    public Sprite[] sprites;

#if UNITY_EDITOR
    [InspectorButton]
    public void GetSprites()
    {
        List<Sprite> tmpSpriteList = new List<Sprite>();
        HashSet<string> pathHashSet = new HashSet<string>();

        string Path = AssetDatabase.GetAssetPath(this);
        int index = 0;
        for (int i = 0; i < Path.Length; i++)
        {
            if (Path[Path.Length - i - 1] == '/')
            {
                index = Path.Length - i-1;
                break;
            }
        }
        Path = Path.Substring(0, index);

        var tmp = AssetDatabase.FindAssets("", new string[] { Path });

        for (int i = 0; i < tmp.Length; i++)
        {
            string dPath = AssetDatabase.GUIDToAssetPath(tmp[i]);
            if (pathHashSet.Contains(dPath))
                continue;
            pathHashSet.Add(dPath);
            List<Sprite> tmpList = AssetDatabase.LoadAllAssetsAtPath(dPath).OfType<Sprite>().ToList();

            for (int k = 0; k < tmpList.Count; k++)
                tmpSpriteList.Add(tmpList[k]);
        }

        tmpSpriteList.Sort((v1, v2) => (AssetDatabase.GetAssetPath(v1) + v1.name).CompareTo(AssetDatabase.GetAssetPath(v2) + v2.name));
        sprites = tmpSpriteList.ToArray();

        EditorUtility.SetDirty(this);
    }
#endif
}