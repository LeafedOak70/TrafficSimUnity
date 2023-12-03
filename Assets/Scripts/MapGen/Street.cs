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


public class Street {
    public int id;
    public List<Tile> streetArray;
    public List<Tile> biruArray;
    public Street(){
        streetArray = new List<Tile>();
        biruArray = new List<Tile>();
        id= 0;
    }

}