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

public class SpriteManager : MonoBehaviour{
    public Sprite horiRoad;
    public Sprite veriRoad;
    public Sprite fourRoad;
    public Sprite tRoad;
    public Sprite lRoad;
    public Sprite roadEnd;
    public Sprite errorSprite;

    public void getRoadSprite(Tile tile, SpriteRenderer spriteRenderer){
        //Four rotations of the T junction
        if(tile.east == TileType.Road &&tile.west == TileType.Road &&tile.south == TileType.Road && tile.north != TileType.Road ){
            spriteRenderer.sprite = tRoad;
        }else if(tile.east != TileType.Road &&tile.west == TileType.Road &&tile.south == TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.east == TileType.Road &&tile.west == TileType.Road &&tile.south != TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.east == TileType.Road &&tile.west != TileType.Road &&tile.south == TileType.Road && tile.north == TileType.Road){
            spriteRenderer.sprite = tRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }
        //Four rotations of the End Road
        else if(tile.south != TileType.Road &&tile.east!= TileType.Road && tile.west == TileType.Road &&tile.north!= TileType.Road){//Right Down
            spriteRenderer.sprite = roadEnd;
        }else if(tile.south != TileType.Road &&tile.east!= TileType.Road && tile.west != TileType.Road &&tile.north== TileType.Road){//Left Down-Rotate Right Once
            spriteRenderer.sprite = roadEnd;
            spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.south != TileType.Road &&tile.east== TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Up Left-Twice
            spriteRenderer.sprite = roadEnd;
            spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.south == TileType.Road &&tile.east!= TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Up Right-Thrice
            spriteRenderer.sprite = roadEnd;
            spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }
        //Four rotations of L road
        else if(tile.south == TileType.Road &&tile.east== TileType.Road && tile.west != TileType.Road &&tile.north!= TileType.Road){//Right Down
            spriteRenderer.sprite = lRoad;
        }else if(tile.south == TileType.Road &&tile.west== TileType.Road&& tile.east != TileType.Road &&tile.north!= TileType.Road){//Left Down-Rotate Right Once
            spriteRenderer.sprite = lRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 90f);
        }else if(tile.north == TileType.Road &&tile.west== TileType.Road&& tile.south != TileType.Road &&tile.east!= TileType.Road){//Up Left-Twice
            spriteRenderer.sprite = lRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 180f);
        }else if(tile.north == TileType.Road &&tile.east== TileType.Road&& tile.south != TileType.Road &&tile.west!= TileType.Road){//Up Right-Thrice
            spriteRenderer.sprite = lRoad;
            spriteRenderer.transform.Rotate(Vector3U.back, 270f);
        }

        else if(tile.north == TileType.Road &&tile.south == TileType.Road &&tile.east == TileType.Road &&tile.west == TileType.Road){
            spriteRenderer.sprite = fourRoad;
        }else if(tile.north == TileType.Road &&tile.south== TileType.Road && tile.east != TileType.Road &&tile.west != TileType.Road){
            spriteRenderer.sprite = veriRoad;
        }else if(tile.east == TileType.Road &&tile.west == TileType.Road && tile.north != TileType.Road &&tile.south != TileType.Road){
            spriteRenderer.sprite = horiRoad;
        }
    }



}