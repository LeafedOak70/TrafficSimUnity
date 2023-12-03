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

public class Car : MonoBehaviour{
    public float speed = 5f;
    public float rotationSpeed = 180f;
    public Sprite horizontalSprite;
    public Sprite verticalSprite;
    private SpriteRenderer spriteRenderer;
    private Vector2U destination;
    private bool isMoving = false;
    public SpriteManager spriteManager;
    public List<Tile> path;
    public Tile start;
    public Tile end;
    public Vector3U direction;

    
    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10000;
        if (spriteManager != null)
        {
            setSprite();
        }
    }
    public void setSprite(){
        Sprite[] sprites = spriteManager.getCarSprites();
        this.horizontalSprite = sprites[0];
        this.verticalSprite = sprites[1];
    }
    
    void Update(){
        if(isMoving){
            MoveToDestination();
        }
    }
    
    public void SpawnAndMove(Tile start, Tile end)
    {
        // Debug.Log($"Spawn waypoint {start.vecBottomRight.x}:{start.vecBottomRight.y}");
        // Set the initial position and destination for the car
        Vector2U dir = getDirection(start, path[1]);
        // Debug.Log($"Start tile at {start.x}:{start.y}");
        // Debug.Log($"Path 0 at {path[0].x}:{path[0].y}");
        setStart(start);
        setDirection(dir);
        Vector3U spawnPoint = getWaypointwithDirection(dir, start);
        // Debug.Log($"Spawning a car at {spawnPoint.x}:{spawnPoint.y}");
        SetInitialPosition(spawnPoint);
        //SetDestination(targetLocation);

        // Enable movement for the car
        isMoving = true;
    }
    void setDirection(Vector3U dir){
        direction = dir;
    }
    void setStart(Tile start){
        this.start = start;
    }
    void SetInitialPosition(Vector3U newPosition)
    {
        // Debug.Log($"Spawning a car at {newPosition.x}:{newPosition.y}");
        transform.position = new Vector3U(newPosition.x, newPosition.y, transform.position.z);
    }
    Vector3U getWaypointwithDirection(Vector2U dir, Tile tile){
        Vector3U returnPoint = new Vector3U();
        if(dir.y == 0){//Going horizontal
            if(dir.x == -1){//Going left
            Debug.Log($"Hori-Left waypoint");
                returnPoint = tile.vecTopLeft;
                Debug.Log($"Getting waypoint {tile.vecTopLeft.x}:{tile.vecTopLeft.y}");
            }else if(dir.x == 1){//Going right
            Debug.Log($"Hori-Right waypoint");
                returnPoint = tile.vecBottomRight;
                Debug.Log($"Getting waypoint {tile.vecBottomRight.x}:{tile.vecBottomRight.y}");
            }   
        }else if(dir.x == 0){//Going vertical
            if(dir.y == 1){//Going Up
            Debug.Log($"Veri-Left waypoint");
                returnPoint = tile.vecTopRight;
                Debug.Log($"Getting waypoint {tile.vecTopRight.x}:{tile.vecTopRight.y}");
            }else if(dir.y == -1){//Going Down
            Debug.Log($"Veri-Left waypoint");
                returnPoint = tile.vecBottomLeft;
                Debug.Log($"Getting waypoint {tile.vecBottomLeft.x}:{tile.vecBottomLeft.y}");
            } 
        }
        
        return returnPoint;
        
    }
    Vector2U getDirection(Tile from, Tile to){
        Vector2U dir = new Vector2U();
        if(from.x > to.x){dir.x = -1;}
        if(from.x < to.x){dir.x = 1;}
        if(from.x == to.x){dir.x = 0;}

        if(from.y > to.y){dir.y = -1;}
        if(from.y < to.y){dir.y = 1;}
        if(from.y == to.y){dir.y = 0;}

        return dir;


    }

    void SetDestination(Vector2U newDest){
        destination = newDest;
        isMoving = true;
    }
    void MoveToDestination(){
        Vector2U direction = (destination - (Vector2U)transform.position).normalized;

        // Check if the car has reached the destination
        if (Vector2U.Distance(transform.position, destination) < 0.1f)
        {
            isMoving = false;
            return;
        }

        // Update the sprite based on movement direction
        UpdateSprite(direction);

        // Move the car
        transform.Translate(direction * speed * Time.deltaTime);
        //spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
    void UpdateSprite(Vector2U movement)
    {
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            // Moving horizontally
            spriteRenderer.sprite = horizontalSprite;
        }
        else
        {
            // Moving vertically
            spriteRenderer.sprite = verticalSprite;
        }
    }


    
}