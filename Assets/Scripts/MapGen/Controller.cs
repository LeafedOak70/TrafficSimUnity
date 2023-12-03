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
    public CarSpawner carSpawner;
    public int width = 128;
    public int height = 128;
    public int scale = 128;
    public List<Tile> gameList;
    public List<Street> streetList;
    public Tile[,] mapArray;
    


    private void Awake(){
        streetList = new List<Street>();
        mapGen.generateMap();
        mapArray = mapGen.mapTileData;
        gameList = mapGen.getGameTiles().Cast<Tile>().ToList();
        streetList = getStreets(gameList);
        
        carSpawner.populizeCity(width, height, streetList, gameList, mapArray);


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
    
}