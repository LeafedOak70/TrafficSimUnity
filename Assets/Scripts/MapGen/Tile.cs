using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour{
    
    public List<Tile> upNeighbours;
    public List<Tile> downNeighbours;
    public List<Tile> leftNeighbours;
    public List<Tile> rightNeighbours;

    public TileType tiletype;
    public TileType north;
    public TileType south;
    public TileType east;
    public TileType west;
    public bool canRoad;
    public bool visited;
    public int districtID;
    public DistrictType districtType;
    public int x;
    public int y;
    public int id;
    public Tile(){
        this.visited = false;
        this.canRoad = true;
    }
    public Tile(Tile tile){
        this.leftNeighbours = tile.leftNeighbours;
        this.rightNeighbours = tile.rightNeighbours;
        this.downNeighbours = tile.downNeighbours;
        this.upNeighbours = tile.upNeighbours;

        this.tiletype = tile.tiletype;
        this.east = tile.east;
        this.west =tile.west;
        this.north = tile.north;
        this.south =tile.south;

        this.canRoad = tile.canRoad;
        this.visited =tile.visited;
        this.districtID = tile.districtID;
        this.districtType =tile.districtType;

        this.x = tile.x;
        this.y =tile.y;
        this.id = tile.id;
    }
    public void copyTile(Tile tile){
        this.leftNeighbours = tile.leftNeighbours;
        this.rightNeighbours = tile.rightNeighbours;
        this.downNeighbours = tile.downNeighbours;
        this.upNeighbours = tile.upNeighbours;

        this.tiletype = tile.tiletype;
        this.east = tile.east;
        this.west =tile.west;
        this.north = tile.north;
        this.south =tile.south;

        this.canRoad = tile.canRoad;
        this.visited =tile.visited;
        this.districtID = tile.districtID;
        this.districtType =tile.districtType;

        this.x = tile.x;
        this.y =tile.y;
        this.id = tile.id;
    }
}