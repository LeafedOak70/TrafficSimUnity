using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour{
    public enum TerrainType { None, Grass, Road, Building };
    public List<Tile> upNeighbours;
    public List<Tile> downNeighbours;
    public List<Tile> leftNeighbours;
    public List<Tile> rightNeighbours;


    public TerrainType north;
    public TerrainType south;
    public TerrainType east;
    public TerrainType west;
}