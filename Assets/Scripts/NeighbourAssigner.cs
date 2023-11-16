// using System;
// using UnityEngine;



// public class NeighbourAssigner : MonoBehaviour
// {
//     // Public variable that references the prefab
//     public Tile[] objectBank;
//     public bool removeNeighbours;
//     public bool reassignNeighbours;
//     public bool resetTypes;

//     void Start()
//     {
//         if(removeNeighbours){removeAllNeighbours();}
//         if(reassignNeighbours){assignNeighbours();}
//         if(resetTypes){removeAllTypes();}
//     }
//     public void removeAllTypes(){
//         for(int i = 0; i < objectBank.Length; i++){
//             objectBank[i].north = Tile.TerrainType.None;
//             objectBank[i].south = Tile.TerrainType.None;
//             objectBank[i].west = Tile.TerrainType.None;
//             objectBank[i].east = Tile.TerrainType.None;
//         }
//     }
//     public void removeAllNeighbours(){
//         for(int i = 0; i < objectBank.Length; i++){
//             objectBank[i].upNeighbours.Clear();
//             objectBank[i].downNeighbours.Clear();
//             objectBank[i].leftNeighbours.Clear();
//             objectBank[i].rightNeighbours.Clear();
//         }
//     }
//     public void assignNeighbours(){
//         for(int i = 0; i < objectBank.Length; i++){

//             for(int b = 0; b < objectBank.Length; b++){
//                 if(objectBank[i].east == objectBank[b].west){
//                 objectBank[i].rightNeighbours.Add(objectBank[b]);
//                 }
//                 if(objectBank[i].north == objectBank[b].south){
//                     objectBank[i].upNeighbours.Add(objectBank[b]);
//                 }
//                 if(objectBank[i].west == objectBank[b].east){
//                     objectBank[i].leftNeighbours.Add(objectBank[b]);
//                 }
//                 if(objectBank[i].south == objectBank[b].north){
//                     objectBank[i].downNeighbours.Add(objectBank[b]);
//                 }
//             }
//         }
//     }
// }
