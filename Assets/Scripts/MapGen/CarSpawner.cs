using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2U =  UnityEngine.Vector2;
using Vector3U =  UnityEngine.Vector3;
using QuaternionU = UnityEngine.Quaternion;
using RandomU = UnityEngine.Random;
using System.Linq;
using UnityEditor.PackageManager;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class CarSpawner : MonoBehaviour{
    public List<Street> streetList;
    public List<Tile> gameList;
    public List<GameObject> carObjects = new List<GameObject>();
    public int width;
    public int height;
    public Tile[,] mapTileData;
    public GameObject carPrefab;


    public void populizeCity(int width, int height,List<Street> streetL, List<Tile> gameList, Tile[,] mapArr){
        this.width = width;
        this.height = height;
        this.streetList = streetL;
        this.gameList = gameList;
        mapTileData = mapArr;
        
        
        findStartEnd();

    }

    public void spawnCar(Tile start, Tile end){
        foreach(Tile tile in gameList){
            if(tile.x == start.x && tile.y == start.y){
                // Debug.Log($"Start waypoint {tile.vecTopLeft.x}:{tile.vecTopLeft.y}");
                start = tile;
                // Debug.Log($"New Start waypoint {start.vecTopLeft.x}:{start.vecTopLeft.y}");
            }
            if(tile.x == end.x && tile.y == end.y){
                // Debug.Log($"Spawn waypoint {tile.x}:{tile.y}");
                end = tile;
            }
        }
        List<Tile> path = new List<Tile>();
        AStarPathFinder aStar = new AStarPathFinder(gameList, width, height); 
        path = aStar.FindPath(start, end);
        //Create car
        SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
        GameObject carObject = Instantiate(carPrefab, new Vector3U(start.x, start.y, 0), QuaternionU.identity);
        carObjects.Add(carObject);
        Car carComponent = carObject.GetComponent<Car>();
        carComponent.spriteManager = spriteManager;
        carComponent.setSprite();
        carComponent.start = start;
        carComponent.end = end;
        carComponent.path = path;
        carComponent.SpawnAndMove(start,end);
        
    }

    public void findStartEnd(){
        
        foreach(Street street in streetList){
            Tile[] twoPoints = new Tile[2];
            twoPoints = getTwoPoints(street);
            if(twoPoints != null){
                Tile spawnTile = twoPoints[0];
                Tile targetTile = twoPoints[1];
                spawnCar(spawnTile, targetTile);
              
            }
            

        }
    }
    
    public void printPath(List<Tile> path){
            Debug.Log($"Path count is {path.Count}");
            Debug.Log("To get to target go to");
            int step = 1;
            foreach(Tile tile in path){
                Debug.Log($"Step {step} - x:{tile.x}, y:{tile.y}");
                step++;
            }

        }
    public Tile[] getTwoPoints(Street street){
        Tile[] tiles = new Tile[2];
        if(street.biruArray.Count > 10)
        {
            //Debug.Log($"Street {street.id} do has many enough buildings");
            int rand1 = RandomU.Range(0,street.biruArray.Count);
            int rand2 = RandomU.Range(0,street.biruArray.Count);
            while(street.biruArray[rand1].canSpawnCar == false ||street.biruArray[rand2].canSpawnCar == false ){
                rand1 = RandomU.Range(0,street.biruArray.Count);
                rand2 = RandomU.Range(0,street.biruArray.Count);
            }
            List<Tile> list1 = getFourNeighbours(street.biruArray[rand1]);
            List<Tile> list2 = getFourNeighbours(street.biruArray[rand2]);
            Tile streetTile1 = new Tile();
            Tile streetTile2 = new Tile();
            foreach(Tile tile in list1){
                if(tile.tiletype == TileType.Road && tile.streetId == street.id){
                    streetTile1 = tile;
                }
            }
            foreach(Tile tile in list2){
                if(tile.tiletype == TileType.Road && tile.streetId == street.id){
                    streetTile2 = tile;
                }
            }
            // Debug.Log($"Got two points at street:{streetTile1.streetId},{streetTile2.streetId}");
            // Debug.Log($"Points are at x:{streetTile1.x}, y:{streetTile1.y} and x:{streetTile2.x}, y:{streetTile2.y}");
            tiles[0] = streetTile1;
            tiles[1] = streetTile2;
            return tiles;
        }
        else{
            return null;
        }

    }
    public List<Tile> getFourNeighbours(Tile tile){
        int[] neighbourX = {0,0,-1,1};
        int[] neighbourY = {1,-1,0,0};
        List<Tile> neighbours = new List<Tile>();
        for( int i = 0; i < 4; i++){
            int newX = tile.x + neighbourX[i];
            int newY = tile.y + neighbourY[i];
            //Debug.Log($"Neighbour tile x:{newX} - y:{newY}");
            if (newX >= 0 && newX < width && newY >= 0 && newY < height){
                neighbours.Add(mapTileData[newX,newY]);
            }
            
        }
        return neighbours;
    }







}