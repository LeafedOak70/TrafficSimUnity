using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile{
    
    public List<Tile> upNeighbours;
    public List<Tile> downNeighbours;
    public List<Tile> leftNeighbours;
    public List<Tile> rightNeighbours;


    public TileType north;
    public TileType south;
    public TileType east;
    public TileType west;
    public bool visited;
    public DistrictType districtType;
    public int x;
    public int y;
}