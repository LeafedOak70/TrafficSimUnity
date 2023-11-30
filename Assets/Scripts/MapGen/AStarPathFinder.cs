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
public class AStarPathFinder
{
    private Tile[,] mapTiles;
    private int width;
    private int height;
    public AStarPathFinder(Tile[,] allTiles, int w, int h){
        mapTiles = allTiles;
        width = w;
        height = h;
    }
    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        HashSet<Tile> openSet = new HashSet<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> gScore = new Dictionary<Tile, float>();
        Dictionary<Tile, float> fScore = new Dictionary<Tile, float>();

        openSet.Add(startTile);
        gScore[startTile] = 0;
        fScore[startTile] = HeuristicCostEstimate(startTile, targetTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = GetTileWithLowestFScore(openSet, fScore);

            if (currentTile == targetTile)
                return ReconstructPath(cameFrom, targetTile);

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            foreach (Tile neighborTile in GetNeighborTiles(currentTile))
            {
                if (closedSet.Contains(neighborTile))
                    continue;

                float tentativeGScore = gScore[currentTile] + DistanceBetween(currentTile, neighborTile);

                if (!openSet.Contains(neighborTile) || tentativeGScore < gScore[neighborTile])
                {
                    cameFrom[neighborTile] = currentTile;
                    gScore[neighborTile] = tentativeGScore;
                    fScore[neighborTile] = gScore[neighborTile] + HeuristicCostEstimate(neighborTile, targetTile);

                    if (!openSet.Contains(neighborTile))
                        openSet.Add(neighborTile);
                }
            }
        }

        return null; // No path found
    }

    float HeuristicCostEstimate(Tile start, Tile target)
    {
        // You can use different heuristic functions based on your needs
        return Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y);
    }

    float DistanceBetween(Tile tile1, Tile tile2)
    {
        // Adjust this based on your needs
        return Mathf.Sqrt(Mathf.Pow(tile1.x - tile2.x, 2) + Mathf.Pow(tile1.y - tile2.y, 2));
    }

    Tile GetTileWithLowestFScore(HashSet<Tile> openSet, Dictionary<Tile, float> fScore)
    {
        Tile lowestTile = null;
        float lowestScore = float.MaxValue;

        foreach (Tile tile in openSet)
        {
            if (fScore.ContainsKey(tile) && fScore[tile] < lowestScore)
            {
                lowestTile = tile;
                lowestScore = fScore[tile];
            }
        }

        return lowestTile;
    }

    List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile currentTile)
    {
        List<Tile> path = new List<Tile> { currentTile };

        while (cameFrom.ContainsKey(currentTile))
        {
            currentTile = cameFrom[currentTile];
            path.Insert(0, currentTile);
        }

        return path;
    }

    List<Tile> GetNeighborTiles(Tile tile)
    {
        // Implement this based on your map structure
        // Get the neighboring tiles of the given tile
        List<Tile> neighbors = new List<Tile>();

        // Example: Assuming a grid map
        int[] neighborX = { 0, 0, -1, 1 };
        int[] neighborY = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = tile.x + neighborX[i];
            int newY = tile.y + neighborY[i];

            // Check if the neighbor is within the valid range
            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                // Add the valid neighboring tile to the list
                Tile neighborTile = mapTiles[newX, newY];
                neighbors.Add(neighborTile);
            }
        }

        return neighbors;
    }
}
