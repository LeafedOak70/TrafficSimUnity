using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2U =  UnityEngine.Vector2;
using Vector3U =  UnityEngine.Vector3;
using QuaternionU = UnityEngine.Quaternion;
using System.Linq;
using UnityEditor.PackageManager;
using Unity.Collections;
using RandomU = UnityEngine.Random;

public class MapGen : MonoBehaviour
{
    public PerlinNoiseGenerator perlinClass;
    public SpriteManager spriteManager;
    public GameObject downtownPrefab;
    public GameObject urbanPrefab;
    public GameObject ruralPrefab;
    public GameObject villagePrefab;
    public GameObject roadPrefab;
    public GameObject noroadPrefab;
    public List<GameObject> instantiatedTiles = new List<GameObject>();
    //public GameObject roadPrefab;
    public List<District> districtList;
    public List<Street> streetList;
    public Tile[,] mapTileData;//raw map data
    public bool generateFromDistricts;
    private int[,] dirArray;
    public int width;
    public int height;
    // private void Start(){
    //     height = 128; width = 128; scale = 128;
    //     generateMap();

    // }

    public void generateMap(int w, int h, int s)
    {
        perlinClass.width = width;
        perlinClass.height = height;
        perlinClass.seed = s;

        this.width = w; this.height = h;
        dirArray = new int[,]{{-1,0},{0,1},{1,0},{0,-1}}; 
        float[,] perlinMap = perlinClass.getPerlinMap(width, height);
        ConvertFromPerlin(perlinMap);
        removeStraggler();//removes little one tile pokey in things
        IdentifyDistricts();
        GenerateRoadOutlines();
        GenerateRoadWithinDistricts();
        IdentifyStreets();
        setRoadNeighbours();
        setBuildingstoSpawnCar();
        
        GenerateDistrictTiles();//This merely generates the blurry outline of the city
        attachRoadSprites();
        
        //copyPrefabstoTiles();

        
        
        
    }
    public Tile[,] getGameTiles(){
        Tile[,] gameTiles = new Tile[width, height];
        foreach(GameObject gameObject in instantiatedTiles){
            Tile tile = gameObject.GetComponent<Tile>();
            gameTiles[tile.x,tile.y] = tile;
            //tile.copyTile(mapTileData[tile.x,tile.y]);

        }
        return gameTiles;
    }
    void setBuildingstoSpawnCar(){

        foreach(Street street in streetList){
            foreach(Tile building in street.biruArray){
                List<Tile> bNeighbours = getFourNeighbours(building);
                foreach(Tile neighbour in bNeighbours){
                    if(neighbour.tiletype == TileType.Road){
                        building.canSpawnCar = true;
                    }
                }
            }
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
    
    void IdentifyStreets(){
        int streetId = 0;
        streetList = new List<Street>();
        foreach(Tile tile in mapTileData){
            if(tile.tiletype == TileType.Road){
                if(tile.inStreet == false){
                    Street street = new Street();
                    street.id = streetId;
                    streetList.Add(street);
                    FloodFillStreet(tile, streetId);
                    streetId++;
                }
            }
        }
    }
    void FloodFillStreet(Tile tile, int id){
        if (tile.x < 0 || tile.x >= width || tile.y < 0 || tile.y >= height)
        {
            return;
        }
        if(tile.inStreet && tile.tiletype == TileType.Road){
            return;
        }
        //Debug.Log($"FloodFill: x={x}, y={y}, districtID={districtID}, district={district}");

        tile.inStreet = true;
        tile.streetId = id;
        streetList[id].streetArray.Add(tile);
        //add heighbouring building
        int[] neighbourX = {0,0,-1,1};
        int[] neighbourY = {1,-1,0,0};
        for( int i = 0; i < 4; i++){
            int newX = tile.x + neighbourX[i];
            int newY = tile.y + neighbourY[i];
            //Debug.Log($"Neighbour tile x:{newX} - y:{newY}");
            if (newX >= 0 && newX < width && newY >= 0 && newY < height){
                if(mapTileData[newX,newY].tiletype == TileType.Road){
                FloodFillStreet(mapTileData[newX,newY],id);
                }else if(mapTileData[newX,newY].tiletype == TileType.Building){
                    streetList[id].biruArray.Add(mapTileData[newX,newY]);
                    mapTileData[newX,newY].streetId = id;
                }
            }
            
        }

    }
    void attachRoadSprites(){
        foreach(GameObject gameObject in instantiatedTiles){
            Tile tile = gameObject.GetComponent<Tile>();
            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            int sortingOrder = Mathf.RoundToInt(transform.position.y * 100f);
            sprite.sortingOrder = sortingOrder;
            if(tile.tiletype == TileType.Road){
                spriteManager.getRoadSprite(tile, sprite);
            }
            if(tile.tiletype == TileType.Building){
                spriteManager.getBiruSprite(tile, sprite);
            }
            if(tile.tiletype == TileType.None){
                
                spriteManager.getNothingSprite(tile, sprite);
            }

        }
    }
    void setRoadNeighbours(){
        foreach(Tile tile in mapTileData){
            if(tile.tiletype == TileType.Road){
                for(int i = 0; i < 4; i++){
                    int newX = tile.x + dirArray[i,0];
                    int newY = tile.y + dirArray[i,1];
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                    {}else{
                        if(i == 0){tile.west = mapTileData[newX,newY].tiletype;}
                        if(i == 1){tile.north = mapTileData[newX,newY].tiletype;}
                        if(i == 2){tile.east = mapTileData[newX,newY].tiletype;}
                        if(i == 3){tile.south = mapTileData[newX,newY].tiletype;}
                    }
                }
            }
        }
    }
    void GenerateRoadWithinDistricts(){
        foreach(District district in districtList){
            // Debug.Log($"in {district.districtType}, id {district.id}");
            //find middle tile of district
                
                // Debug.Log($"Starting road monster at x: {avgX} : y: {avgY}");
                int spawnNum = 3;
            if(district.districtType == DistrictType.Downtown){
                int totalX = 0;
                int totalY = 0;;
                foreach(Tile tile in district.tileArray){
                    if(tile.districtType == district.districtType){
                        totalX += tile.x;
                        totalY += tile.y;
                    }
                }
                int avgX = totalX/district.tileArray.Count;
                int avgY = totalY/district.tileArray.Count;
                Vector2U up = new Vector2U(0,1);
                roadMonster(avgX, avgY,5,5,up,district.districtType,spawnNum);
            }else if(district.districtType == DistrictType.Urban){
                int rand1 = RandomU.Range(0,district.tileArray.Count);
                int rand2 = RandomU.Range(0,district.tileArray.Count);
                int rand3 = RandomU.Range(0,district.tileArray.Count);
                int rand4 = RandomU.Range(0,district.tileArray.Count);
                Vector2U up = new Vector2U(0,1);
                Vector2U down = new Vector2U(0,-1);
                Vector2U right = new Vector2U(1,0);
                Vector2U left = new Vector2U(-1,0);
                Tile tile1 = district.tileArray[rand1];
                Tile tile2 = district.tileArray[rand2];
                Tile tile3 = district.tileArray[rand3];
                Tile tile4 = district.tileArray[rand4];
                Debug.Log($"Starting road monster at Urban at x:{tile1.x}, y:{tile1.y}");
                Debug.Log($"Starting road monster at Urban at x:{tile2.x}, y:{tile2.y}");
                Debug.Log($"Starting road monster at Urban at x:{tile3.x}, y:{tile3.y}");
                Debug.Log($"Starting road monster at Urban at x:{tile4.x}, y:{tile4.y}");
                roadMonster(tile1.x, tile1.y,7,7,up,district.districtType,spawnNum);
                roadMonster(tile2.x, tile2.y,7,7,down,district.districtType,spawnNum);
                roadMonster(tile3.x, tile3.y,7,7,left,district.districtType,spawnNum);
                roadMonster(tile4.x, tile4.y,7,7,right,district.districtType,spawnNum);
            }
        }
    }
    void roadMonster(int x, int y, int timerL, int timerR, Vector2U vector, DistrictType districtType, int spawnNum){
        //check whether visiting tile is valid
        // Debug.Log($"x: {x}, y: {y}, timerL: {timerL}, timerR: {timerR}");
        // Debug.Log($"vec ATS TART: ({vector.x}, {vector.y})");
        if (x < 1 || x > width-2 || y < 1 || y > height-2)
        {
            // Debug.Log($"x: {x}, OUT OF BOUNDS");
            return;
        }
        if(districtType != mapTileData[x,y].districtType){
            return;
        }
        // if(DistrictType.Downtown != mapTileData[x,y].districtType  || DistrictType.Urban != mapTileData[x,y].districtType){
        //     return;
        //  }
        if(mapTileData[x,y].canRoad == false || mapTileData[x,y].tiletype == TileType.Road){
            return;
        }
        //set to road and set visited
        mapTileData[x,y].tiletype = TileType.Road;
        mapTileData[x,y].canRoad = false;

        //generate random number between 0 to 1
        float randomNumber1 = UnityEngine.Random.Range(0f, 1f);
        float roundedNumber1 = Mathf.Round(randomNumber1 * 100f) / 100f;

        float randomNumber2 = UnityEngine.Random.Range(0f, 1f);
        float roundedNumber2 = Mathf.Round(randomNumber2 * 100f) / 100f;
        if(roundedNumber1 < 0.8){//80% of the time lower timer by 1
            timerL--;
        }else{
            timerL -=2;
        }
        if(roundedNumber2 < 0.8){//80% of the time lower timer by 1
            timerR--;
        }else{
            timerR -=2;
        }
        //Reset timer and create new branch in new direction
        if(timerL < 1 && spawnNum > 0){
            timerL=5;
            Vector2U vecLeft = getVector(vector,0);
            // Debug.Log($"vec before getVector: ({vector.x}, {vector.y})");
            // Debug.Log($"vecLeft after getVector: ({vecLeft.x}, {vecLeft.y})");
            // Debug.Log($"COORDS before getVector: ({x}, {y})");
            // Debug.Log($"COORDS after getVector: ({x+(int)vecLeft.x}, {y+(int)vecLeft.y})");
            //mapTileData[x+(int)vecLeft.x,y+(int)vecLeft.y].tiletype = TileType.Road;
            spawnNum--;
            roadMonster(x+(int)vecLeft.x,y+(int)vecLeft.y,5, 5,vecLeft,districtType,spawnNum);
        }else{
            //set left tile to not become road
            Vector2U vecLeft = getVector(vector,0);
            // Debug.Log("MAKE LEFT ROAD NO");
            int leftX = x+(int)vecLeft.x;
            int leftY = y+(int)vecLeft.y;
            mapTileData[leftX,leftY].canRoad = false;
            mapTileData[leftX,leftY].tiletype = TileType.Building;
        }
        if(timerR < 1 && spawnNum > 0){
            timerR=5;
            Vector2U vecRight = getVector(vector,1);//0 for get left
            // Debug.Log("MAKE RIGHT ROAD NO");
            //mapTileData[x+(int)vecRight.x,y+(int)vecRight.y].tiletype = TileType.Road;
            spawnNum--;
            roadMonster(x+(int)vecRight.x,y+(int)vecRight.y,5, 5,vecRight,districtType,spawnNum);
        }else{
            //set right tile to not become road
            Vector2U vecRight = getVector(vector,1);//0 for get left
            int rightX = x+(int)vecRight.x;
            int righty = y+(int)vecRight.y;
            mapTileData[rightX,righty].canRoad = false;
            mapTileData[rightX,righty].tiletype = TileType.Building;
        }
        int newX = x+(int)vector.x;
        int newY = y+(int)vector.y;
        // Debug.Log($"NEWx: {newX}, NEWy: {newY}, timerL: {timerL}, timerR: {timerR}");
        // Debug.Log($"vec at END: ({vector.x}, {vector.y})");
        spawnNum = 3;
        roadMonster(newX,newY,timerL, timerR,vector,districtType,spawnNum);


    }
    Vector2U getVector(Vector2U vector, int dir){// dir 0 = left, 1 = right
        Vector2U newVec = new Vector2U();
        int[] vecCoords = {(int)vector.x, (int)vector.y};
        int index = 99;
        for(int i = 0; i < 4;i++){
            int[] current = {dirArray[i,0], dirArray[i,1]};
            if(Enumerable.SequenceEqual(current,vecCoords)){
                index = i;
                break;
            }
        }
        if(index != 99){
            int newIndex = 0;
            if(dir == 0){
                newIndex = (index - 1 + dirArray.GetLength(0)) % dirArray.GetLength(0);
                
            }else if(dir==1){
                newIndex = (index + 1) % (dirArray.Length/2);
                // Debug.Log($"Old index is {index}, length is : {dirArray.Length}");
                // Debug.Log($"New index is {newIndex}");
            }
            newVec.x = dirArray[newIndex, 0];
            newVec.y = dirArray[newIndex, 1];
            return newVec;

        }else{
            // Debug.Log("NEVER FOUNND THIS THING D:");
        }
        // Debug.Log("COUDLNT MAKE NEW VECTOR");
        return newVec;
    }
    
    void removeStraggler(){
        foreach(Tile tile in mapTileData){
            DistrictType currDistrict = tile.districtType;
            DistrictType[] districtValues = (DistrictType[])Enum.GetValues(typeof(DistrictType));
            int currentIndex = Array.IndexOf(districtValues, currDistrict);
            int prevIndex = (currentIndex - 1 + districtValues.Length) % districtValues.Length;
            int frontIndex = (currentIndex + 1) % districtValues.Length;
            DistrictType innerDistrict = districtValues[prevIndex];
            DistrictType outterDistrict = districtValues[frontIndex];
            int[] neighbourX = {0,0,-1,1};
            int[] neighbourY = {1,-1,0,0};
            int limit1 = 0;
            int limit2 = 0;
            for(int i = 0; i < 4;i++){
                int nXcoord = tile.x + neighbourX[i];//Coords for neigbour
                int nYcoord = tile.y + neighbourY[i];
                if (nXcoord >= 0 && nXcoord < width && nYcoord >= 0 && nYcoord < height){
                // Check if the neighbor is within the valid range
                    if (mapTileData[nXcoord, nYcoord].districtType == innerDistrict){
                        limit1++;
                    }
                    if (mapTileData[nXcoord, nYcoord].districtType == outterDistrict){
                        limit2++;
                    }
                    if(limit1 == 3){
                        tile.districtType = innerDistrict;
                        break;
                    }
                    if(limit2 == 3){
                        tile.districtType = outterDistrict;
                        break;
                    }
            }
            }

        }

    }
    //Generate roads alone district outlines
    void GenerateRoadOutlines(){
        foreach (Tile tile in mapTileData)
        {
            if(checkNeighbours(tile)){
                tile.tiletype = TileType.Road;
                tile.canRoad = false;
            }
        }
    }
    //compare given tile to it's surrounding 9 neighbours
    bool checkNeighbours(Tile tile){
        //Get the two districts to compare
        if (tile.x < 0 || tile.x >= width || tile.y < 0 || tile.y >= height)
        {
            return false;
        }
        DistrictType currDistrict = tile.districtType;
        DistrictType[] districtValues = (DistrictType[])Enum.GetValues(typeof(DistrictType));
        int currentIndex = Array.IndexOf(districtValues, currDistrict);
        int prevIndex = (currentIndex + 1) % districtValues.Length;
        DistrictType innerDistrict = districtValues[prevIndex];
        //Compare 8 neighbours to given tile
        int[] neighbourX = {-1,-1,-1,0,0,1,1,1};
        int[] neighbourY = {1,0,-1,1,-1,1,0,-1};
        for(int i = 0; i < 8;i++){
            
            int nXcoord = tile.x + neighbourX[i];//Coords for neigbour
            int nYcoord = tile.y + neighbourY[i];
            
            if (nXcoord >= 0 && nXcoord < width && nYcoord >= 0 && nYcoord < height){
            // Check if the neighbor is within the valid range
                if (mapTileData[nXcoord, nYcoord].districtType == innerDistrict){
                    return true; // The tile is within the border of the district
                }
            }
            

        }
        return false;//Not within the border
                

    }
    void IdentifyDistricts(){
        int districtID = 0;
        districtList = new List<District>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(!mapTileData[x,y].visited){
                    District district = new District();
                    district.id = districtID;
                    district.districtType = mapTileData[x,y].districtType;
                    districtList.Add(district);
                    FloodFill(x,y,district.id, mapTileData[x,y].districtType);
                    districtID++;
                }
            }
        }
    }
    void FloodFill(int x, int y, int districtID, DistrictType district){
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }
        if(mapTileData[x,y].visited ||mapTileData[x,y].districtType != district){
            return;
        }
        //Debug.Log($"FloodFill: x={x}, y={y}, districtID={districtID}, district={district}");

        mapTileData[x,y].visited = true;
        mapTileData[x,y].districtID = districtID;
        districtList[districtID].tileArray.Add(mapTileData[x,y]);
        FloodFill(x+1,y,districtID,mapTileData[x,y].districtType);
        FloodFill(x-1,y,districtID,mapTileData[x,y].districtType);
        FloodFill(x,y+1,districtID,mapTileData[x,y].districtType);
        FloodFill(x,y-1,districtID,mapTileData[x,y].districtType);
    }
    void GenerateDistrictTiles(){
        if(generateFromDistricts){
            foreach (District district in districtList)
            {
                foreach (Tile tile in district.tileArray)
                {
                    GameObject prefabToInstantiate;
                    if(tile.tiletype == TileType.Road){
                        prefabToInstantiate = roadPrefab;

                    }else if(tile.canRoad == false){
                        prefabToInstantiate= noroadPrefab;
                    }else{
                        prefabToInstantiate= GetPrefabForTile(tile.districtType);
                    }
                    GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, new Vector3U(tile.x, tile.y, 0), QuaternionU.identity);
                    instantiatedTiles.Add(instantiatedPrefab);
                    Tile tileComponent = instantiatedPrefab.GetComponent<Tile>();
                    if (tileComponent != null)
                    {
                        tileComponent.copyTile(tile);
                    }
                }
            }
        }else{
            foreach(Tile tile in mapTileData){
                GameObject prefabToInstantiate = GetPrefabForTile(tile.districtType);
                
                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, new Vector3U(tile.x, tile.y, 0), QuaternionU.identity);
                instantiatedTiles.Add(instantiatedPrefab);
                Tile tileComponent = instantiatedPrefab.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    tileComponent.copyTile(tile);
                }
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
        mapTileData = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sample = perlinMap[x, y];
                mapTileData[x,y] = ChooseDistrictType(sample);
                mapTileData[x,y].x = x;
                mapTileData[x,y].y = y;
                mapTileData[x,y].id = x +y;
                mapTileData[x,y].gCost = 999999;
                mapTileData[x,y].CalculateFCost();
                //Instantiate(tilePrefab, new Vector3U(x, y, 0), QuaternionU.identity);
            }
        }
    }
    Tile ChooseDistrictType(float sample)
    {
        Tile tile = new Tile();
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
