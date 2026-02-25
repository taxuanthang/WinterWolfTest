using System.Collections.Generic;
using System.Linq;
using static Board;

public class MatchSystem
{
    private int boardSizeX;

    private int boardSizeY;
    private int m_matchMin;

    private Cell[,] m_cells;

    public MatchSystem(Cell[,] cells, int width, int height, int matchMin)
    {
        m_cells = cells;
        boardSizeX = width;
        boardSizeY = height;
        m_matchMin = matchMin;
    }

    public List<Cell> GetHorizontalMatches(Cell cell)
    {
        List<Cell> list = new List<Cell>();
        list.Add(cell);

        //check horizontal match
        Cell newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourRight;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourLeft;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        return list;
    }

    public List<Cell> GetVerticalMatches(Cell cell)
    {
        List<Cell> list = new List<Cell>();
        list.Add(cell);

        Cell newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourUp;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourBottom;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        return list;
    }

    internal eMatchDirection GetMatchDirection(List<Cell> matches)
    {
        if (matches == null || matches.Count < m_matchMin) return eMatchDirection.NONE;

        var listH = matches.Where(x => x.BoardX == matches[0].BoardX).ToList();
        if (listH.Count == matches.Count)
        {
            return eMatchDirection.VERTICAL;
        }

        var listV = matches.Where(x => x.BoardY == matches[0].BoardY).ToList();
        if (listV.Count == matches.Count)
        {
            return eMatchDirection.HORIZONTAL;
        }

        if (matches.Count > 5)
        {
            return eMatchDirection.ALL;
        }

        return eMatchDirection.NONE;
    }

    internal List<Cell> FindFirstMatch()
    {
        List<Cell> list = new List<Cell>();

        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                Cell cell = m_cells[x, y];

                var listhor = GetHorizontalMatches(cell);
                if (listhor.Count >= m_matchMin)
                {
                    list = listhor;
                    break;
                }

                var listvert = GetVerticalMatches(cell);
                if (listvert.Count >= m_matchMin)
                {
                    list = listvert;
                    break;
                }
            }
        }

        return list;
    }

    internal List<Cell> GetPotentialMatches()
    {
        List<Cell> result = new List<Cell>();
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                Cell cell = m_cells[x, y];

                //check right
                /* example *\
                  * * * * *
                  * * * * *
                  * * * ? *
                  * & & * ?
                  * * * ? *
                \* example  */

                if (cell.NeighbourRight != null)
                {
                    result = GetPotentialMatch(cell, cell.NeighbourRight, cell.NeighbourRight.NeighbourRight);
                    if (result.Count > 0)
                    {
                        break;
                    }
                }

                //check up
                /* example *\
                  * ? * * *
                  ? * ? * *
                  * & * * *
                  * & * * *
                  * * * * *
                \* example  */
                if (cell.NeighbourUp != null)
                {
                    result = GetPotentialMatch(cell, cell.NeighbourUp, cell.NeighbourUp.NeighbourUp);
                    if (result.Count > 0)
                    {
                        break;
                    }
                }

                //check bottom
                /* example *\
                  * * * * *
                  * & * * *
                  * & * * *
                  ? * ? * *
                  * ? * * *
                \* example  */
                if (cell.NeighbourBottom != null)
                {
                    result = GetPotentialMatch(cell, cell.NeighbourBottom, cell.NeighbourBottom.NeighbourBottom);
                    if (result.Count > 0)
                    {
                        break;
                    }
                }

                //check left
                /* example *\
                  * * * * *
                  * * * * *
                  * ? * * *
                  ? * & & *
                  * ? * * *
                \* example  */
                if (cell.NeighbourLeft != null)
                {
                    result = GetPotentialMatch(cell, cell.NeighbourLeft, cell.NeighbourLeft.NeighbourLeft);
                    if (result.Count > 0)
                    {
                        break;
                    }
                }

                /* example *\
                  * * * * *
                  * * * * *
                  * * ? * *
                  * & * & *
                  * * ? * *
                \* example  */
                Cell neib = cell.NeighbourRight;
                if (neib != null && neib.NeighbourRight != null && neib.NeighbourRight.IsSameType(cell))
                {
                    Cell second = LookForTheSecondCellVertical(neib, cell);
                    if (second != null)
                    {
                        result.Add(cell);
                        result.Add(neib.NeighbourRight);
                        result.Add(second);
                        break;
                    }
                }

                /* example *\
                  * * * * *
                  * & * * *
                  ? * ? * *
                  * & * * *
                  * * * * *
                \* example  */
                neib = null;
                neib = cell.NeighbourUp;
                if (neib != null && neib.NeighbourUp != null && neib.NeighbourUp.IsSameType(cell))
                {
                    Cell second = LookForTheSecondCellHorizontal(neib, cell);
                    if (second != null)
                    {
                        result.Add(cell);
                        result.Add(neib.NeighbourUp);
                        result.Add(second);
                        break;
                    }
                }
            }

            if (result.Count > 0) break;
        }

        return result;
    }

    private List<Cell> GetPotentialMatch(Cell cell, Cell neighbour, Cell target)
    {
        List<Cell> result = new List<Cell>();

        if (neighbour != null && neighbour.IsSameType(cell))
        {
            Cell third = LookForTheThirdCell(target, neighbour);
            if (third != null)
            {
                result.Add(cell);
                result.Add(neighbour);
                result.Add(third);
            }
        }

        return result;
    }

    private Cell LookForTheSecondCellHorizontal(Cell target, Cell main)
    {
        if (target == null) return null;
        if (target.IsSameType(main)) return null;

        //look right
        Cell second = null;
        second = target.NeighbourRight;
        if (second != null && second.IsSameType(main))
        {
            return second;
        }

        //look left
        second = null;
        second = target.NeighbourLeft;
        if (second != null && second.IsSameType(main))
        {
            return second;
        }

        return null;
    }

    private Cell LookForTheSecondCellVertical(Cell target, Cell main)
    {
        if (target == null) return null;
        if (target.IsSameType(main)) return null;

        //look up        
        Cell second = target.NeighbourUp;
        if (second != null && second.IsSameType(main))
        {
            return second;
        }

        //look bottom
        second = null;
        second = target.NeighbourBottom;
        if (second != null && second.IsSameType(main))
        {
            return second;
        }

        return null;
    }

    private Cell LookForTheThirdCell(Cell target, Cell main)
    {
        if (target == null) return null;
        if (target.IsSameType(main)) return null;

        //look up
        Cell third = CheckThirdCell(target.NeighbourUp, main);
        if (third != null)
        {
            return third;
        }

        //look right
        third = null;
        third = CheckThirdCell(target.NeighbourRight, main);
        if (third != null)
        {
            return third;
        }

        //look bottom
        third = null;
        third = CheckThirdCell(target.NeighbourBottom, main);
        if (third != null)
        {
            return third;
        }

        //look left
        third = null;
        third = CheckThirdCell(target.NeighbourLeft, main); ;
        if (third != null)
        {
            return third;
        }

        return null;
    }

    private Cell CheckThirdCell(Cell target, Cell main)
    {
        if (target != null && target != main && target.IsSameType(main))
        {
            return target;
        }

        return null;
    }
}