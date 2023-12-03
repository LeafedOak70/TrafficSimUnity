using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3U =  UnityEngine.Vector3;

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
    public bool inStreet;
    public int streetId;
    public int gCost;
    public int hCost; 
    public int fCost;
    public bool canSpawnCar;
    public RoadType roadType;
    public Tile prevTile;
    
    //public RoadType roadType;
    public List<Vector3U> wayPoints;
    public Vector3U vecTopLeft;
    public Vector3U vecTopRight;
    public Vector3U vecBottomLeft;
    public Vector3U vecBottomRight;

    public Tile(){
        this.visited = false;
        this.canRoad = true;
        this.inStreet = false;
        wayPoints = new List<Vector3U>();
        this.canSpawnCar = false;
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
        this.inStreet = tile.inStreet;
        this.streetId = tile.streetId;
        this.canSpawnCar = tile.canSpawnCar;

        this.wayPoints = tile.wayPoints;

        this.fCost = tile.fCost;
        this.gCost = tile.hCost;
        this.gCost = tile.gCost;
    }
    public void CalculateFCost(){
        fCost = gCost + hCost;
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
        this.canSpawnCar = tile.canSpawnCar;

        this.x = tile.x;
        this.y =tile.y;
        this.id = tile.id;
        this.inStreet = tile.inStreet;
        this.streetId = tile.streetId;

        this.wayPoints = tile.wayPoints;
    }
}