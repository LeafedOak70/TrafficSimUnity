using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class District{
    public int id;

    public List<Tile> tileArray;
    public DistrictType districtType;

    public District(){
        tileArray = new List<Tile>();
    }
}