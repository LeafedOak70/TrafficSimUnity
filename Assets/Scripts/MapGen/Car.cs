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
    public float speed = 1f;
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
    public List<Vector3U> waypoints = new List<Vector3U>();

    
    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10000;
        if (spriteManager != null)
        {
            setSprite();
        }
    }
    
    
    void Update(){
        if(isMoving){
            MoveToDestination();
        }
    }
    public void SpawnAndMove(Tile start, Tile end)
    {
        Vector2U dir = getDirection(start, path[1]);
        setStartandEnd(start,end);
        setDirection(dir);
        Vector3U spawnPoint = getWaypointwithDirection(dir, start);
        SetInitialPosition(spawnPoint);
        generateWaypoint();
        isMoving = true;
    }
    void MoveToDestination()
    {
        if (waypoints.Count > 0)
        {

            Vector3U targetWaypoint = waypoints[0];
            Vector3U currentPosition = transform.position;

            // Calculate the direction and distance to the target waypoint
            Vector3U directionToWaypoint = (targetWaypoint - currentPosition).normalized;
            float distanceToWaypoint = Vector3U.Distance(currentPosition, targetWaypoint);

            // Move towards the waypoint
            transform.Translate(directionToWaypoint * speed * Time.deltaTime);

            // Check if the car has reached the waypoint
            if (distanceToWaypoint < 0.1f)
            {
                // Remove the reached waypoint from the list
                waypoints.RemoveAt(0);

                // Update the sprite based on the next movement
                if (waypoints.Count > 0)
                {
                    Vector2U nextMovement = (Vector2U)(waypoints[0] - currentPosition);
                    UpdateSprite(nextMovement);
                }
            }
            if (waypoints.Count == 0 && transform.position.x == end.x && transform.position.y == end.y)
            {
                

                Destroy(gameObject); // Uncomment this line to destroy the car GameObject
                // gameObject.SetActive(false); // Uncomment this line to disable the car GameObject
            }
        }
        else
        {
            // If there are no more waypoints, stop moving

            isMoving = false;
        }
    }

    void generateWaypoint()
    {
        for(int i =1; i < path.Count-1; i++){
            addWaypoints(path[i-1],path[i],path[i+1]);//TODO: This doesn't add the last tiles waypoint add that exception

        }
        
    }
    void addWaypoints(Tile prevTile, Tile currTile, Tile nextTile){
        Vector2U cameFrom = getDirection(prevTile,currTile);
        Vector2U goingTo = getDirection(currTile,nextTile);
        if(cameFrom.x == 0 && cameFrom.y == -1){//Came From Top
            if(goingTo.x == 1 && goingTo.y == 0){//Going right
                waypoints.Add(currTile.vecTopLeft);
                waypoints.Add(currTile.vecBottomLeft);
                waypoints.Add(currTile.vecBottomRight);
            }
            if(goingTo.x == -1 && goingTo.y == 0){//Going left
                waypoints.Add(currTile.vecTopLeft);
            }
            if(goingTo.x == 0 && goingTo.y == -1){//Going down
                waypoints.Add(currTile.vecTopLeft);
                waypoints.Add(currTile.vecBottomLeft);
            }
        }else if(cameFrom.x == 1 && cameFrom.y == 0){//Came From Left
            if(goingTo.x == 0 && goingTo.y == 1){//Going Top
                waypoints.Add(currTile.vecBottomLeft);
                waypoints.Add(currTile.vecBottomRight);
                waypoints.Add(currTile.vecTopRight);
            }
            if(goingTo.x == 1 && goingTo.y == 0){//Going Right
                waypoints.Add(currTile.vecBottomLeft);
                waypoints.Add(currTile.vecBottomRight);
            }
            if(goingTo.x == 0 && goingTo.y == -1){//Going Down
                waypoints.Add(currTile.vecBottomLeft);
            }
        }else if(cameFrom.x == -1 && cameFrom.y == 0){//Came From Right
            if(goingTo.x == -1 && goingTo.y == 0){//Going Left
                waypoints.Add(currTile.vecTopRight);
                waypoints.Add(currTile.vecTopLeft);
            }
            if(goingTo.x == 0 && goingTo.y == 1){//Going Up
                waypoints.Add(currTile.vecTopRight);
            }
            if(goingTo.x == 0 && goingTo.y == -1){//Going Down
                waypoints.Add(currTile.vecTopRight);
                waypoints.Add(currTile.vecTopLeft);
                waypoints.Add(currTile.vecBottomLeft);
            }
        }else if(cameFrom.x == 0 && cameFrom.y == 1){//Came From Bot
            if(goingTo.x == -1 && goingTo.y == 0){//Going Left
                waypoints.Add(currTile.vecBottomRight);
                waypoints.Add(currTile.vecTopRight);
                waypoints.Add(currTile.vecTopLeft);
            }
            if(goingTo.x == 1 && goingTo.y == 0){//Going Right
                waypoints.Add(currTile.vecBottomRight);
            }
            if(goingTo.x == 0 && goingTo.y == 1){//Going up
                waypoints.Add(currTile.vecBottomRight);
                waypoints.Add(currTile.vecTopRight);
            }
        }
    }
    public void setSprite(){
        Sprite[] sprites = spriteManager.getCarSprites();
        this.horizontalSprite = sprites[0];
        this.verticalSprite = sprites[1];
    }
    
    void setDirection(Vector3U dir){
        direction = dir;
    }
    void setStartandEnd(Tile start, Tile end){
        this.start = start;
        this.end = end;
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
            // Debug.Log($"Hori-Left waypoint");
                returnPoint = tile.vecTopLeft;
                // Debug.Log($"Getting waypoint {tile.vecTopLeft.x}:{tile.vecTopLeft.y}");
            }else if(dir.x == 1){//Going right
            // Debug.Log($"Hori-Right waypoint");
                returnPoint = tile.vecBottomRight;
                // Debug.Log($"Getting waypoint {tile.vecBottomRight.x}:{tile.vecBottomRight.y}");
            }   
        }else if(dir.x == 0){//Going vertical
            if(dir.y == 1){//Going Up
            // Debug.Log($"Veri-Left waypoint");
                returnPoint = tile.vecTopRight;
                // Debug.Log($"Getting waypoint {tile.vecTopRight.x}:{tile.vecTopRight.y}");
            }else if(dir.y == -1){//Going Down
            // Debug.Log($"Veri-Left waypoint");
                returnPoint = tile.vecBottomLeft;
                // Debug.Log($"Getting waypoint {tile.vecBottomLeft.x}:{tile.vecBottomLeft.y}");
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