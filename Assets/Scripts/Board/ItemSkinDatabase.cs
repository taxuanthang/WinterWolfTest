using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSkinDatabase", menuName = "Match3/Item Skin Database")]
public class ItemSkinDatabase : ScriptableObject
{
    public Sprite[] normalItemSprites; // size = 7


    public Sprite GetSprite(int index)
    {

        return normalItemSprites[index];
    }

}
