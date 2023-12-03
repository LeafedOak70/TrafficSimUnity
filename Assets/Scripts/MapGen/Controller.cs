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

public class Controller : MonoBehaviour{
    public MapGen mapGen;
    // public SpriteManager spriteManager;
    // public Car car;
    // public PerlinNoiseGenerator perlinGen;
    public bool testBool;
    public int width = 128;
    public int height = 128;
    public int scale = 128;
    public List<Tile> gameList;
    public List<Street> streetList;
    public GameObject carPrefab;
    public List<GameObject> carObjects = new List<GameObject>();


    private void Awake(){
        streetList = new List<Street>();
        mapGen.generateMap();
        gameList = mapGen.getGameTiles().Cast<Tile>().ToList();
        streetList = getStreets(gameList);
        spawnCar();
        
        // test();
        //generateCars()

    }
    // private void Update(){
    //     if(testBool){test();}
    // }
    public void spawnCar(){
        List<Tile> path = new List<Tile>();
        foreach(Street street in streetList){
            Tile[] twoPoints = new Tile[2];
            twoPoints = getTwoPoints(street);
            if(twoPoints != null){
                Tile spawnTile = twoPoints[0];
                Tile targetTile = twoPoints[1];
                Debug.Log($"Received at street:{spawnTile.streetId},{targetTile.streetId}");
                Debug.Log($"Points are at x:{spawnTile.x}, y:{spawnTile.y} and x:{targetTile.x}, y:{targetTile.y}");
                
                Debug.Log($"Spawning car in street {street.id} at x:{spawnTile.x}, y:{spawnTile.y}");
                AStarPathFinder aStar = new AStarPathFinder(gameList, width, height); 
                path = aStar.FindPath(spawnTile, targetTile);
                GameObject carObject = Instantiate(carPrefab, new Vector3U(spawnTile.x, spawnTile.y, 0), QuaternionU.identity);
                carObjects.Add(carObject);
                
            }
            
            
            // printPath(path);

        }
    }
    public List<Street> getStreets(List<Tile> gameTiles){
        //sort all street tiles into array first
        List<Street> listOStreets = new List<Street>();
        List<Tile> sortedList = gameTiles.OrderBy(tile => tile.streetId).ToList();
        int streetId = 0;
        Street street = new Street();
        street.id = streetId;
        foreach(Tile tile in sortedList){
            if(streetId != tile.streetId){
                streetId = tile.streetId;
                listOStreets.Add(street);
                street = new Street();
                street.id= streetId;
            }
            if(tile.tiletype == TileType.Building){
                street.biruArray.Add(tile);
            }else if(tile.tiletype == TileType.Road){
                street.streetArray.Add(tile);
            }

        }


        return listOStreets;
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
            List<Tile> list1 = mapGen.getFourNeighbours(street.biruArray[rand1]);
            List<Tile> list2 = mapGen.getFourNeighbours(street.biruArray[rand2]);
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
}