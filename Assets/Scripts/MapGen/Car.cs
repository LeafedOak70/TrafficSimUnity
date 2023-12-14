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
    public float speed = 100f;
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
    private Rigidbody2D rb2d;
    public Vector3U direction;
    public List<Vector3U> waypoints = new List<Vector3U>();
    private bool isStopTimerActive = false;
    private float stopTimerDuration = 2f;
    private float stopTimer;
    private bool hasStartedMoving = false;

    
    private void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f; // Disable gravity for 2D physics
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10000;
        if (spriteManager != null)
        {
            setSprite();
        }
    }
    
    
    void Update()
    {
        if (isMoving)
        {
            if (!hasStartedMoving)
            {
                // Delay the initial collision check to avoid immediate stopping upon spawning
                hasStartedMoving = true;
            }
            else if (!isStopTimerActive)
            {
                CheckCollision();  // Check for collision before moving
                MoveToDestination();
            }
            else
            {
                // Update the timer
                stopTimer -= Time.deltaTime;

                // If the timer has elapsed, resume movement
                if (stopTimer <= 0f)
                {
                    isStopTimerActive = false;
                    stopTimer = 0f; // Reset the timer
                }
            }
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
            Vector3U currentPosition = rb2d.position;

            // Calculate the direction and distance to the target waypoint
            Vector3U directionToWaypoint = (targetWaypoint - currentPosition).normalized;
            float distanceToWaypoint = Vector3U.Distance(currentPosition, targetWaypoint);

            // Move towards the waypoint
            rb2d.MovePosition(new Vector3U(rb2d.position.x, rb2d.position.y) + directionToWaypoint * speed * Time.deltaTime);



            
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
            
        }
        else
        {
            // If there are no more waypoints, stop moving
            if (waypoints.Count == 0)
            {
                

                //  Debug.Log($"Car reached destination. At {end.x}:{end.y}");

                // Set the GameObject inactive
                gameObject.SetActive(false);
            }
            isMoving = false;
        }
    }

   void CheckCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rb2d.velocity.normalized, 1f);

        if (hit.collider != null && hit.collider.CompareTag("Car"))
        {
            // Stop the car
            rb2d.velocity = Vector2U.zero;

            // Optionally, adjust the car's orientation
            Vector2U currentDirection = rb2d.velocity.normalized;
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            QuaternionU targetRotation = QuaternionU.AngleAxis(angle, Vector3U.forward);
            transform.rotation = targetRotation;

            // Start the stop timer
            isStopTimerActive = true;
            stopTimer = stopTimerDuration;
        }
    }

    void generateWaypoint()
    {
        for(int i =1; i < path.Count-1; i++){
            addWaypoints(path[i-1],path[i],path[i+1]);//TODO: This doesn't add the last tiles waypoint add that exception

        }
        
    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            // Adjust the position to avoid collision
            Vector2U avoidDirection = (rb2d.position - (Vector2U)collision.transform.position).normalized;
            rb2d.MovePosition(rb2d.position + avoidDirection * speed * Time.deltaTime);
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