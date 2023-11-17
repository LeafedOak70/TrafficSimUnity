using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public PerlinNoiseGenerator perlinClass;
    public GameObject downtownPrefab;
    public GameObject urbanPrefab;
    public GameObject ruralPrefab;
    public GameObject villagePrefab;
    //public GameObject roadPrefab;
    public List<District> districtList;
    public Tile[,] mapTileData;//raw district data
    public bool generateFromDistricts;

    private void Update()
    {
        float[,] perlinMap = perlinClass.getPerlinMap();
        ConvertFromPerlin(perlinMap);
        IdentifyDistricts();
        GenerateTiles();
    }
    void IdentifyDistricts(){
        int districtID = 0;
        districtList = new List<District>();

        for (int x = 0; x < perlinClass.width; x++)
        {
            for (int y = 0; y < perlinClass.height; y++)
            {
                if(!mapTileData[x,y].visited){
                    District district = new District();
                    district.id = districtID;
                    districtList.Add(district);
                    FloodFill(x,y,district.id, mapTileData[x,y].districtType);
                    districtID++;
                }
            }
        }
    }
    void FloodFill(int x, int y, int districtID, DistrictType district){
        if (x < 0 || x >= perlinClass.width || y < 0 || y >= perlinClass.height)
        {
            return;
        }
        if(mapTileData[x,y].visited ||mapTileData[x,y].districtType != district){
            return;
        }
        //Debug.Log($"FloodFill: x={x}, y={y}, districtID={districtID}, district={district}");

        mapTileData[x,y].visited = true;
        districtList[districtID].tileArray.Add(mapTileData[x,y]);
        FloodFill(x+1,y,districtID,mapTileData[x,y].districtType);
        FloodFill(x-1,y,districtID,mapTileData[x,y].districtType);
        FloodFill(x,y+1,districtID,mapTileData[x,y].districtType);
        FloodFill(x,y-1,districtID,mapTileData[x,y].districtType);
    }
    void GenerateTiles(){
        if(generateFromDistricts){
            foreach (District district in districtList)
            {
                

                foreach (Tile tile in district.tileArray)
                {
                    GameObject prefabToInstantiate = GetPrefabForTile(tile.districtType);
                    Instantiate(prefabToInstantiate, new Vector3(tile.x, tile.y, 0), Quaternion.identity);
                }
            }
        }else{
            foreach(Tile tile in mapTileData){
                GameObject prefabToInstantiate = GetPrefabForTile(tile.districtType);
                Instantiate(prefabToInstantiate, new Vector3(tile.x, tile.y, 0), Quaternion.identity);
            }
        }
    }
    GameObject GetPrefabForTile(DistrictType districtType){
        switch (districtType)
    {
        case DistrictType.Downtown:
            return downtownPrefab;
        case DistrictType.Urban:
            return urbanPrefab;
        case DistrictType.Rural:
            return ruralPrefab;
        case DistrictType.Villages:
            return villagePrefab;
        default:
            // Default to a generic prefab or handle the case based on your requirements
            return villagePrefab;
    }
    }
    void ConvertFromPerlin(float[,] perlinMap)
    {
        mapTileData = new Tile[perlinClass.width, perlinClass.height];
        for (int x = 0; x < perlinClass.width; x++)
        {
            for (int y = 0; y < perlinClass.height; y++)
            {
                float sample = perlinMap[x, y];
                mapTileData[x,y] = ChooseDistrictType(sample);
                mapTileData[x,y].x = x;
                mapTileData[x,y].y = y;
                //Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
    Tile ChooseDistrictType(float sample)
    {
        Tile tile = new Tile();
        tile.visited = false;
        if (sample < perlinClass.downtownThreshold)
        {
            tile.districtType = DistrictType.Downtown;
            
        }
        else if (sample < perlinClass.urbanThreshold)
        {
            tile.districtType = DistrictType.Urban;
        }
        else if (sample < perlinClass.ruralThreshold)
        {
            tile.districtType = DistrictType.Rural;
        }
        else
        {
            tile.districtType = DistrictType.Villages;
        }   
        return tile;   
        
    }

}
