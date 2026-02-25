using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Match3/Prefab Database")]
public class PrefabDatabase : ScriptableObject
{
    public GameObject[] prefab;

    public GameObject GetPrefab(int index)
    {
        return prefab[index];
    }

}