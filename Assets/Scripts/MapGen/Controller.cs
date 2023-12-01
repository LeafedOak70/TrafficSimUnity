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

public class Controller : MonoBehaviour{
    public MapGen mapGen;
    // public SpriteManager spriteManager;
    // public Car car;
    // public PerlinNoiseGenerator perlinGen;
    
    public bool testBool;
    public int width = 128;
    public int height = 128;
    public int scale = 128;
    public Tile[,] mapTileData;


    private void Awake(){
        mapTileData = new Tile[width, height];
        mapGen.generateMap();
        foreach(Street street in mapGen.streetList){
            
            getTwoPoints(street);
        }
        
        // test();
        //generateCars()

    }
    // private void Update(){
    //     if(testBool){test();}
    // }
    public void getTwoPoints(Street street){
        
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
            Tile streetTile1 = street.biruArray[rand1];
            Tile streetTile2 = street.biruArray[rand2];
            foreach(Tile tile in list1){
                if(tile.tiletype == TileType.Road){
                    streetTile1 = tile;
                }
            }
            foreach(Tile tile in list2){
                if(tile.tiletype == TileType.Road){
                    streetTile2 = tile;
                }
            }
            AStarPathFinder aStar = new AStarPathFinder(mapGen.mapTileData, width, height); 
            List<Tile> path = aStar.FindPath(streetTile1, streetTile2);
            Debug.Log($"Have gotten two tiles in street {street.id} at x1: {streetTile1.x} - y1: {streetTile1.y} and x2: {streetTile2.x} - y2: {streetTile2.y}");
            Debug.Log($"For path for street id : {street.id} steps needed are {path.Count}");
            foreach(Tile tile in path){
                Debug.Log($"Go to x: {tile.x} - y: {tile.y}");
            }
            
            
            
                
            
        }
            
        

    }
    
    


}