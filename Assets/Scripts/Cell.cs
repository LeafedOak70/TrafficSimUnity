using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool collapsed;
    public int x;
    public int y;
    public Tile[] tileOptions;
    public int index;

    public void CreateCell(bool collapseState, int x, int y, Tile[] tiles, int index)
    {
        collapsed = collapseState;
        this.x = x;
        this.y = y;
        tileOptions = tiles;
        this.index = index;
    }
    public void setTiles(Tile[] tiles)
    {
        tileOptions = tiles;
    }
    
}