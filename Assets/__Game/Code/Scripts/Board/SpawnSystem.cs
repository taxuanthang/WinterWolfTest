using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnSystem
{
    private Cell[,] m_cells;
    private int boardSizeX;
    private int boardSizeY;

    [Header("Dependencies")]
    private ItemSkinDatabase m_skinDatabase;

    private PrefabDatabase m_prefabDatabase;

    private ItemPool m_itemPool;


    public SpawnSystem(
        Cell[,] cells,
        int width,
        int height,
        PrefabDatabase prefabDatabase,
        ItemSkinDatabase skinDatabase,
        ItemPool itemPool)
    {
        m_cells = cells;
        boardSizeX = width;
        boardSizeY = height;
        m_prefabDatabase = prefabDatabase;
        m_skinDatabase = skinDatabase;
        m_itemPool = itemPool;
    }


    internal void Fill()
    {
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                Cell cell = m_cells[x, y];
                GameObject prefab = m_prefabDatabase.GetPrefab(0);

                NormalItem item = new NormalItem();
                item.Initialize(prefab, m_itemPool);

                item.SetSkinDatabase(m_skinDatabase);

                List<NormalItem.eNormalType> types = new List<NormalItem.eNormalType>();
                if (cell.NeighbourBottom != null)
                {
                    NormalItem nitem = cell.NeighbourBottom.Item as NormalItem;
                    if (nitem != null)
                    {
                        types.Add(nitem.ItemType);
                    }
                }

                if (cell.NeighbourLeft != null)
                {
                    NormalItem nitem = cell.NeighbourLeft.Item as NormalItem;
                    if (nitem != null)
                    {
                        types.Add(nitem.ItemType);
                    }
                }

                item.SetType(Utils.GetRandomNormalTypeExcept(types.ToArray()));
                item.SetView();
                item.SetViewRoot(cell.transform);

                cell.Assign(item);
                cell.ApplyItemPosition(false);
            }
        }
    }
    internal void FillGapsWithNewItems()
    {
        // get existing normal items count
        Dictionary<NormalItem.eNormalType, int> counts = new Dictionary<NormalItem.eNormalType, int>();

        foreach (NormalItem.eNormalType type in Enum.GetValues(typeof(NormalItem.eNormalType)))
        {
            counts[type] = 0;
        }

        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                if (m_cells[x, y].Item is NormalItem n)
                {
                    counts[n.ItemType]++;
                }
            }
        }
        // fill gaps with new items
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                Cell cell = m_cells[x, y];
                if (!cell.IsEmpty) continue;

                GameObject prefab = m_prefabDatabase.GetPrefab(0);

                NormalItem item = new NormalItem();
                item.Initialize(prefab, m_itemPool);

                item.SetSkinDatabase(m_skinDatabase);
                // collect forbidden neighbour types
                HashSet<NormalItem.eNormalType> forbidden = new HashSet<NormalItem.eNormalType>();

                AddNeighbourType(cell.NeighbourUp, forbidden);
                AddNeighbourType(cell.NeighbourBottom, forbidden);
                AddNeighbourType(cell.NeighbourLeft, forbidden);
                AddNeighbourType(cell.NeighbourRight, forbidden);

                // get candidate types
                List<NormalItem.eNormalType> candidates =
                    Enum.GetValues(typeof(NormalItem.eNormalType))
                        .Cast<NormalItem.eNormalType>()
                        .Where(t => !forbidden.Contains(t))
                        .ToList();

                NormalItem.eNormalType chosenType;

                if (candidates.Count > 0)
                {
                    // choose the least used type
                    chosenType = candidates
                        .OrderBy(t => counts[t])
                        .First();

                }
                else
                {
                    // fallback
                    chosenType = Utils.GetRandomNormalType();
                }

                item.SetType(chosenType);
                item.SetView();
                item.SetViewRoot(cell.transform);

                cell.Assign(item);
                cell.ApplyItemPosition(true);

                // update count
                counts[chosenType]++;
            }
        }
    }

    private void AddNeighbourType(Cell neighbour, HashSet<NormalItem.eNormalType> set)
    {
        if (neighbour != null && neighbour.Item is NormalItem n)
        {
            set.Add(n.ItemType);
        }
    }
}