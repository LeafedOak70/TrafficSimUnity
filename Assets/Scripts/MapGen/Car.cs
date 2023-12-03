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
    
    

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10000;
        SpriteManager spriteManager = GameObject.FindObjectOfType<SpriteManager>();
        Sprite[] sprites = spriteManager.getCarSprites();
        horizontalSprite = sprites[0];
        verticalSprite = sprites[1];

    }
    void Update(){
        if(isMoving){
            MoveToDestination();
        }
    }
    void Move(){

    }
    public void SpawnAndMove(Vector2U spawnLocation, Vector2U targetLocation, List<Tile> path)
    {
        // Set the initial position and destination for the car
        SetInitialPosition(spawnLocation);
        SetDestination(targetLocation);

        // Enable movement for the car
        isMoving = true;
    }
    void SetInitialPosition(Vector2U newPosition)
    {
        transform.position = new Vector3U(newPosition.x, newPosition.y, transform.position.z);
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