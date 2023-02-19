using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주인공 캐릭터의 Customization을 돕는 class
/// </summary>
public class CustomizationController : MonoBehaviour
{
    [System.Serializable]
    public class EquipInformation
    {
        public CustomizationController customizationController;
        public CustomizationType customizationType;
        public IntSaver equipItemNumberSaver;
        public CustomizationItem defaultItem;
        public SpriteChanger[] spriteChangers;

        public void Init()
        {
            equipItemNumberSaver = new IntSaver("CharacterCustomEquipSaver." + customizationType.ToString(), defaultItem.myIdNumber);
            SetEquipCustomItem(GetEquipCustomItem());
        }

        public CustomizationItem GetEquipCustomItem()
        {
            return DataDictionary.Instance.customizationItems[equipItemNumberSaver.GetValue()];
        }

        public void SetEquipCustomItem(CustomizationItem customizationItem)
        {
            equipItemNumberSaver.SetValue(customizationItem.myIdNumber);

            for(int i =0; i < spriteChangers.Length; i++)
            {
                spriteChangers[i].mySpriteCollector = customizationItem.customCharacterInformation.spriteCollector;
            }
        }
    }

    public EquipInformation[] characterCustomInformations;
    public Dictionary<CustomizationType, EquipInformation> customTypeToEquipInfo;

    public void Init()
    {
        customTypeToEquipInfo = new Dictionary<CustomizationType, EquipInformation>();
        for (int i =0;i< characterCustomInformations.Length; i++)
        {
            characterCustomInformations[i].Init();
            customTypeToEquipInfo.Add(characterCustomInformations[i].customizationType, characterCustomInformations[i]);
        }
    }
}
