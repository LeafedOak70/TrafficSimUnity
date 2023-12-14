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
    public List<GameObject> carPool = new List<GameObject>();
    public int width;
    public int height;
    public Tile[,] mapTileData;
    public GameObject carPrefab;
    int totalCar;


    public void populizeCity(int width, int height,List<Street> streetL, List<Tile> gameList, Tile[,] mapArr){
        this.width = width;
        this.height = height;
        this.streetList = streetL;
        this.gameList = gameList;
        mapTileData = mapArr;
        generatePoolCar();

        SpawnCarsInstantly();
        StartCoroutine(SpawnCarsSlowly());
   

    }

    public void generatePoolCar(){
        totalCar = 0;
         foreach(Street street in streetList){
            if(street.biruArray.Count > 10){
                int carNum = Mathf.CeilToInt(street.biruArray.Count / 10.0f);
                totalCar += carNum;
              
            }
        }
        for(int i = 0; i < totalCar; i++){
            GameObject carObject = Instantiate(carPrefab, Vector3U.zero, QuaternionU.identity);
            carObject.name = "Car "+ i;
            carObject.SetActive(false);
            carPool.Add(carObject);
        }
    }
    public void spawnCar(Tile start, Tile end, List <Tile> streetArr){
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
        AStarPathFinder aStar = new AStarPathFinder(streetArr, width, height); 
        path = aStar.FindPath(start, end);
        //Create car
        SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
        GameObject carObject = getCarFromPool();
        if(carObject != null){
            Car carComponent = carObject.GetComponent<Car>();
            carComponent.spriteManager = spriteManager;
            carComponent.setSprite();
            float randomNumber = UnityEngine.Random.Range(0f, 1f);
            float roundedNumber = Mathf.Round(randomNumber * 100f) / 100f;
            float speed = 0;
            if(roundedNumber < 0.2){
                speed=0.5f;
            }
            else if(roundedNumber < 0.7){
                speed = 1;
            }else {
                speed = 2;
            }
            carComponent.speed = speed;
            carComponent.initSpeed = speed;
            carComponent.start = start;
            carComponent.end = end;
            carComponent.path = path;
            carComponent.SpawnAndMove(start,end);
            carObject.SetActive(true);
        }
        
    }
    public GameObject getCarFromPool(){
        foreach (GameObject carObject in carPool)
    {
        if (!carObject.activeInHierarchy)
        {
            return carObject;
        }
    }
    return null;
    }
    // public void SpawnCarsSlowly(){
        public void SpawnCarsInstantly()
        {
            foreach (Street street in streetList)
            {
                if (street.biruArray.Count > 10)
                {
                    int carNum = Mathf.CeilToInt(street.biruArray.Count / 10.0f);
                    for (int i = 0; i < carNum; i++)
                    {
                        Tile spawnTile = getSpawn(street);
                        Tile targetTile = getTarget(street, spawnTile);
                        // Debug.Log($"Got two points at street:{spawnTile.streetId},{targetTile.streetId}");
                        // Debug.Log($"Points are at x:{spawnTile.x}, y:{spawnTile.y} and x:{targetTile.x}, y:{targetTile.y}");
                        spawnCar(spawnTile, targetTile, street.streetArray);
                        // yield return new WaitForSeconds(1.0f);
                    }
                }
            }
        }
    IEnumerator SpawnCarsSlowly(){
        
        foreach(Street street in streetList){
            if(street.biruArray.Count > 10){
                int carNum = Mathf.CeilToInt(street.biruArray.Count / 10.0f);
                for(int i = 0; i < carNum; i++){
                    Tile spawnTile = getSpawn(street);
                    Tile targetTile = getTarget(street,spawnTile);
                    // Debug.Log($"Got two points at street:{spawnTile.streetId},{targetTile.streetId}");
                    // Debug.Log($"Points are at x:{spawnTile.x}, y:{spawnTile.y} and x:{targetTile.x}, y:{targetTile.y}");
                    spawnCar(spawnTile, targetTile, street.streetArray);
                    yield return new WaitForSeconds(1.0f);
                }
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
    public Tile getSpawn(Street street){
        int rand1 = RandomU.Range(0,street.biruArray.Count);
        while(street.biruArray[rand1].canSpawnCar == false ){
            rand1 = RandomU.Range(0,street.biruArray.Count);
        }
        List<Tile> list1 = getFourNeighbours(street.biruArray[rand1]);
        int rand2 = RandomU.Range(0,list1.Count);
        Tile streetTile1 = list1[rand2];
        while(streetTile1.tiletype != TileType.Road || streetTile1.streetId != street.id){
            rand2 = RandomU.Range(0,list1.Count);
            streetTile1 = list1[rand2];
        }
        return streetTile1;
    }
    public Tile getTarget(Street street, Tile spawn){
        Tile streetTile1;
        do{
            int rand1 = RandomU.Range(0,street.biruArray.Count);
            while(street.biruArray[rand1].canSpawnCar == false ){
                rand1 = RandomU.Range(0,street.biruArray.Count);
            }
            List<Tile> list1 = getFourNeighbours(street.biruArray[rand1]);
            int rand2 = RandomU.Range(0,list1.Count);
            streetTile1 = list1[rand2];
            while(streetTile1.tiletype != TileType.Road || streetTile1.streetId != street.id){
                rand2 = RandomU.Range(0,list1.Count);
                streetTile1 = list1[rand2];
            }
        }while(streetTile1.x == spawn.x && streetTile1.y == spawn.y);
        
        return streetTile1;
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