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
    using Utils;
    
    public class AStarPathFinder
    {
        private List<Tile> mapTiles;
        private int width;
        private int height;
        private int straightCost = 10;
        public AStarPathFinder(List<Tile> allTiles, int w, int h){
            mapTiles = allTiles;
            width = w;
            height = h;
        }
        public List<Tile> FindPath(Tile startTile, Tile targetTile)
        {
            // if(startTile.tiletype != TileType.Road){Debug.Log($"This starting tile not road at tile x: {startTile.x} - y: {startTile.y}" );}
            // if(targetTile.tiletype != TileType.Road){Debug.Log($"This end tile not road at tile x: {targetTile.x} - y: {targetTile.y}");}
            PriorityQueue<Tile, int> openList = new PriorityQueue<Tile, int>();
            List<Tile> closedList = new List<Tile>();
            // openList.Add(startTile);
            
            foreach(Tile tile in mapTiles){
                tile.gCost = 8888888;
                tile.CalculateFCost();
                tile.prevTile = null;
                tile.inSet = false;
            }
            // Debug.Log($"Starting from tile x: {startTile.x} - y: {startTile.y}");
            startTile.gCost = 0;
            startTile.hCost = ManhattanCostEstimate(startTile, targetTile);
            startTile.CalculateFCost();
            openList.Enqueue(startTile, startTile.fCost);
            while(openList.Count > 0){
                // Tile currentTile = GetTileWithLowestFScore(openList);
                Tile currentTile = openList.Dequeue();
                // Debug.Log($"Checking tile x: {currentTile.x} - y: {currentTile.y}");
                if(currentTile.x == targetTile.x && currentTile.y == targetTile.y){
                    // Debug.Log($"Reached End as current Tile is at x: {currentTile.x},y: {currentTile.y} nad target is x: {targetTile.x},y: {targetTile.y}");
                    return CalculatePath(currentTile);
                }
                
                // openList.Remove(currentTile);
                closedList.Add(currentTile);
                
                foreach(Tile neighbour in getFourNeighbours(currentTile)){
                    if(closedList.Contains(neighbour)) continue;
                    if(neighbour.tiletype != TileType.Road){
                        closedList.Add(neighbour);
                        continue;
                    }
                    if(neighbour.tiletype == TileType.Road){
                        // Debug.Log($"Found road neighbour at tile x: {neighbour.x} - y: {neighbour.y}");
                    }
                    int tentativeGCost = currentTile.gCost + ManhattanCostEstimate(currentTile, neighbour);
                    if(tentativeGCost <  neighbour.gCost){
                        neighbour.prevTile = currentTile;
                        // Debug.Log($"Current is at x: {currentTile.x},y: {currentTile.y}");
                        // Debug.Log($"Neighbour is at x: {neighbour.x},y: {neighbour.y}");
                        // Debug.Log($"Neighbour's previous is at x: {neighbour.prevTile.x},y: {neighbour.prevTile.y}");

                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = ManhattanCostEstimate(neighbour, targetTile);
                        neighbour.CalculateFCost();
                        // Debug.Log($"Has better g Cost");

                        if(neighbour.inSet == false){
                            openList.Enqueue(neighbour, neighbour.fCost);
                            neighbour.inSet = true;
                            // Debug.Log($"Adding to dings");
                        }

                    }
                }
            }

            return null;

        
        }
        public int ManhattanCostEstimate(Tile start, Tile target)
        {
            
            return (int)(Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y) * straightCost);
        }
        
        Tile GetTileWithLowestFScore(List<Tile> openSet)
        {
            Tile lowestTile = openSet[0];
            int lowestF = openSet[0].fCost;
            foreach (Tile tile in openSet)
            {
                if(tile.fCost < lowestF){
                    lowestF = tile.fCost;
                    lowestTile = tile;
                }
            }

            return lowestTile;
        }
        List<Tile> CalculatePath(Tile endTile){
            List<Tile> path = new List<Tile>();
            path.Add(endTile);
            Tile currTile = endTile;
            // Debug.Log($"Attempting to print end to start, from x:{endTile.x}, y:{endTile.y}");
            while(currTile.prevTile != null){
                path.Add(currTile.prevTile);
                // Debug.Log($"go x:{currTile.x}, y:{currTile.y}");
                currTile = currTile.prevTile;
            }
            path.Reverse();
            return path;
        }
        public List<Tile> getFourNeighbours(Tile tile){
            int[] neighbourX = {0,0,-1,1};
            int[] neighbourY = {1,-1,0,0};
            // if(tile == null){Debug.Log("It s a nunll tile llol");}
            List<Tile> neighbours = new List<Tile>();
            for( int i = 0; i < 4; i++){
                int newX = tile.x + neighbourX[i];
                int newY = tile.y + neighbourY[i];
                //Debug.Log($"Neighbour tile x:{newX} - y:{newY}");
                if (newX >= 0 && newX < width && newY >= 0 && newY < height){
                    foreach(Tile tileT in mapTiles){
                        if(tileT.x == newX && tileT.y == newY){
                            if(tileT.streetId == tile.streetId){
                                neighbours.Add(tileT);
                            }
                        }
                    }
                }
                
            }
            return neighbours;
        }
    }
