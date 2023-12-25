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
using UnityEditor.Build.Content;
using UnityEditor;

public class Controller : MonoBehaviour{
    public MapGen mapGen;
    // public SpriteManager spriteManager;
    // public Car car;
    // public PerlinNoiseGenerator perlinGen;
    public bool testBool;
    public CarSpawner carSpawner;
    private int width;
    private int height;
    private int seed;
    public List<Tile> gameList;
    public List<Street> streetList;
    public Tile[,] mapArray;
    public Camera uiCamera;
    public Camera gameCamera;
    public Clock clock;
    public float timeLength;
    public GameObject statGroup;
    public StatUIManager statUI;
    public int rate;
    private void Awake(){
        streetList = new List<Street>();
        // startMap();
    }
    public void startMap(int w, int h, int s, int t, int r){
        width = w;
        height = h;
        seed = s;
        timeLength = t;
        rate = r;
        mapGen.generateMap(width, height , seed);
        mapArray = mapGen.mapTileData;
        gameList = mapGen.getGameTiles().Cast<Tile>().ToList();
        streetList = getStreets(gameList);

        carSpawner.populizeCity(width, height, streetList, gameList, mapArray, rate);
        clock.startClock(timeLength);
    }
    public void endofSim(){
        carSpawner.stopAllCars();
        statUI.start = true;
        statUI.ogRate = rate;
        
        if(rate == 2){statUI.currentRate = "low";}
        else if(rate == 1){statUI.currentRate = "mid";}
        else if(rate == 0){statUI.currentRate = "high";}
        statUI.setRealStats();
        SetChildrenActive(statGroup, true);
        statUI.showAvg();
    }
    private void SetChildrenActive(GameObject obj, bool active)
    {
        obj.SetActive(active);

        foreach (Transform child in obj.transform)
        {
            SetChildrenActive(child.gameObject, active);
        }
    }
    public void changeCamera(){
        uiCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
        float middleX = width * 0.5f;
        float middleY = height * 0.5f;
        float cameraZ = -10f; // Adjust this value based on your scene setup

        Vector3U targetPosition = new Vector3U(middleX, middleY, cameraZ);
        gameCamera.transform.position = targetPosition;

    }
    public int GetWidth(){return width;}
    public int GetHeight(){return height;}
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