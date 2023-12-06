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
using UnityEditor.Experimental.GraphView;

public class SpriteManager : MonoBehaviour{
    public Sprite horiRoad;
    public Sprite veriRoad;
    public Sprite fourRoad;
    public Sprite[] tRoad;
    public Sprite[] lRoad;
    public Sprite[] roadEnd;
    public Sprite[] biru;
    public Sprite errorSprite;
    public Sprite[] carHoriSprites;
    public Sprite[] carVeriSprites;
    public Sprite nothing;

    public Sprite[] getCarSprites(){
        int rand1 = RandomU.Range(0,carHoriSprites.Length);
        Sprite[] carS = new Sprite[2];
        carS[0] = carHoriSprites[rand1];
        carS[1] = carVeriSprites[rand1];
        return carS;
    }
    public void getNothingSprite(Tile tile, SpriteRenderer spriteRenderer){
        if(tile.districtType == DistrictType.Downtown || tile.districtType == DistrictType.Urban){
            int randomBiru = RandomU.Range(0,biru.Length);
            spriteRenderer.sprite = biru[randomBiru];  
        }else{
            spriteRenderer.sprite = nothing;
        }
        
    }
    public void getBiruSprite(Tile tile, SpriteRenderer spriteRenderer){
        
        int randomBiru = RandomU.Range(0,biru.Length);
        spriteRenderer.sprite = biru[randomBiru];
    }
    public void getRoadSprite(Tile tile, SpriteRenderer spriteRenderer){//This also assigns waypoints :P
        Vector3U vecTopLeft = new Vector3U();
        Vector3U vecTopRight = new Vector3U();
        Vector3U vecBotLeft = new Vector3U();
        Vector3U vecBotRight = new Vector3U();
        RoadType currRoad = RoadType.None;

        //Four rotations of the T junction
        if(tile.east == TileType.Road &&tile.west == TileType.Road &&tile.south == TileType.Road && tile.north != TileType.Road ){
            spriteRenderer.sprite = tRoad[0];
            currRoad = RoadType.TRoad;
        }else if(tile.east != TileType.Road &&tile.west == TileType.Road &&tile.south == TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad[3];
            currRoad = RoadType.TRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.east == TileType.Road &&tile.west == TileType.Road &&tile.south != TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad[2];
            currRoad = RoadType.TRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.east == TileType.Road &&tile.west != TileType.Road &&tile.south == TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad[1];
            currRoad = RoadType.TRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }
        //Four rotations of the End Road
        else if(tile.south != TileType.Road &&tile.east!= TileType.Road && tile.west == TileType.Road &&tile.north!= TileType.Road){//Right Down
             spriteRenderer.sprite = roadEnd[0];
             currRoad = RoadType.End;
        }else if(tile.south != TileType.Road &&tile.east!= TileType.Road && tile.west != TileType.Road &&tile.north== TileType.Road){//Left Down-Rotate Right Once
            spriteRenderer.sprite = roadEnd[3];
            currRoad = RoadType.End;
            // spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.south != TileType.Road &&tile.east== TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Up Left-Twice
            spriteRenderer.sprite = roadEnd[2];
            currRoad = RoadType.End;
            // spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.south == TileType.Road &&tile.east!= TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Up Right-Thrice
            spriteRenderer.sprite = roadEnd[1];
            currRoad = RoadType.End;
            // spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }
        //Four rotations of L road
        else if(tile.south == TileType.Road &&tile.east== TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Right Down
            spriteRenderer.sprite = lRoad[0];
            currRoad = RoadType.LRoad;
        }else if(tile.south == TileType.Road &&tile.west== TileType.Road&& tile.east != TileType.Road &&tile.north!= TileType.Road){//Left Down-Rotate Right Once
            spriteRenderer.sprite = lRoad[3];
            currRoad = RoadType.LRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.north == TileType.Road &&tile.west== TileType.Road&& tile.south != TileType.Road &&tile.east!= TileType.Road){//Up Left-Twice
            spriteRenderer.sprite = lRoad[2];
            currRoad = RoadType.LRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.north == TileType.Road &&tile.east== TileType.Road&& tile.south != TileType.Road &&tile.west!= TileType.Road){//Up Right-Thrice
            spriteRenderer.sprite = lRoad[1];
            currRoad = RoadType.LRoad;
            // spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }

        else if(tile.north == TileType.Road &&tile.south == TileType.Road &&tile.east == TileType.Road &&tile.west == TileType.Road){
            spriteRenderer.sprite = fourRoad;
            currRoad = RoadType.Four;
        }else if(tile.north == TileType.Road &&tile.south== TileType.Road && tile.east != TileType.Road &&tile.west != TileType.Road){
            spriteRenderer.sprite = veriRoad;
            vecTopLeft.x = (float)(tile.x-0.2); vecTopLeft.y = (float)(tile.y+0.35);
            vecTopRight.x = (float)(tile.x+0.2); vecTopRight.y = (float)(tile.y+0.35);
            vecBotLeft.x = (float)(tile.x-0.2); vecBotLeft.y = (float)(tile.y-0.35);
            vecBotRight.x = (float)(tile.x+0.2); vecBotRight.y = (float)(tile.y-0.35);
            currRoad = RoadType.Veri;
        }else if(tile.east == TileType.Road &&tile.west == TileType.Road && tile.north != TileType.Road &&tile.south != TileType.Road){
            spriteRenderer.sprite = horiRoad;
            vecTopLeft.x = (float)(tile.x-0.35); vecTopLeft.y = (float)(tile.y+0.2);
            vecTopRight.x = (float)(tile.x+0.35); vecTopRight.y = (float)(tile.y+0.2);
            vecBotLeft.x = (float)(tile.x-0.35); vecBotLeft.y = (float)(tile.y-0.2);
            vecBotRight.x = (float)(tile.x+0.35); vecBotRight.y = (float)(tile.y-0.2);
            currRoad = RoadType.Hori;

        }else if(tile.east != TileType.Road &&tile.west != TileType.Road && tile.north != TileType.Road &&tile.south != TileType.Road){
            tile.tiletype = TileType.Building;
            currRoad = RoadType.None;
        }
        if(currRoad == RoadType.End){
            vecTopLeft.x = (float)(tile.x-0.15); vecTopLeft.y = (float)(tile.y+0.2);
            vecTopRight.x = (float)(tile.x+0.15); vecTopRight.y = (float)(tile.y+0.2);
            vecBotLeft.x = (float)(tile.x-0.15); vecBotLeft.y = (float)(tile.y-0.2);
            vecBotRight.x = (float)(tile.x+0.15); vecBotRight.y = (float)(tile.y-0.2);
        }else if(currRoad == RoadType.LRoad || currRoad == RoadType.Four || currRoad == RoadType.TRoad){
            vecTopLeft.x = (float)(tile.x-0.15); vecTopLeft.y = (float)(tile.y+0.15);
            vecTopRight.x = (float)(tile.x+0.15); vecTopRight.y = (float)(tile.y+0.15);
            vecBotLeft.x = (float)(tile.x-0.15); vecBotLeft.y = (float)(tile.y-0.15);
            vecBotRight.x = (float)(tile.x+0.15); vecBotRight.y = (float)(tile.y-0.15);
        }
        tile.roadType = currRoad;
        tile.vecBottomLeft = vecBotLeft;
        tile.vecBottomRight = vecBotRight;
        tile.vecTopLeft = vecTopLeft;
        tile.vecTopRight = vecTopRight;
        tile.wayPoints.Add(vecTopLeft);
        tile.wayPoints.Add(vecTopRight);
        tile.wayPoints.Add(vecBotLeft);
        tile.wayPoints.Add(vecBotRight);
    }



}