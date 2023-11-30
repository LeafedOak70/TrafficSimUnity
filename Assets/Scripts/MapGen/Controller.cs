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
    public SpriteManager spriteManager;
    public Car car;
    public PerlinNoiseGenerator perlinGen;

    public bool testBool;
    public int width = 128;
    public int height = 128;
    public int scale = 128;


    private void Start(){
        mapGen.generateMap(width, height, scale);
        test();
        //generateCars()

    }
    private void Update(){
        if(testBool){test();}
    }
    public void test(){
        foreach(Street street in mapGen.streetList){
            if(street.biruArray.Count < 10){return;}
            int rand1 = RandomU.Range(0,street.biruArray.Count);
            int rand2 = RandomU.Range(0,street.biruArray.Count);
            if(getStreetTile(street.biruArray[rand1]) != null && getStreetTile(street.biruArray[rand2])){
                Tile streetTile1 = getStreetTile(street.biruArray[rand1]);
                Tile streetTile2 = getStreetTile(street.biruArray[rand2]);
            }else{Debug.Log($"Was null couldnt find street from building for street id {street.id}");}
            
            
        }
    }
    public Tile getStreetTile(Tile tile){
        int[] neighbourX = {0,0,-1,1};
        int[] neighbourY = {1,-1,0,0};
        for( int i = 0; i < 4; i++){
                int newX = tile.x + neighbourX[i];
                int newY = tile.y + neighbourY[i];
                //Debug.Log($"Neighbour tile x:{newX} - y:{newY}");
                if (newX >= 0 && newX < width && newY >= 0 && newY < height){
                    if(mapGen.mapTileData[newX,newY].tiletype == TileType.Road){
                        return mapGen.mapTileData[newX,newY];
                    }
                }
            
        }
        return null;

    }


}