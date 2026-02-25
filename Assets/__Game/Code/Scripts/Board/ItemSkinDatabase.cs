using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSkinDatabase", menuName = "Match3/Item Skin Database")]
public class ItemSkinDatabase : ScriptableObject
{
    public Sprite[] normalItemSprites; // size = 7

    public Sprite[] bonusItemSprites; // size = 3


    public Sprite GetNormalSprite(int index)
    {

        return normalItemSprites[index];
    }

    public Sprite GetBonusSprite(int index)
    {

        return bonusItemSprites[index];
    }

}
