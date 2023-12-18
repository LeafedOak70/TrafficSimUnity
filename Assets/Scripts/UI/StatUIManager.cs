using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatUIManager : MonoBehaviour
{
    public TMP_Text totalCars;
    public TMP_Text totalDistance;
    public TMP_Text totalAvgDistance;
    public TMP_Text totalTime;
    public TMP_Text totalTrafficTime;
    public TMP_Text totalAvgTime;
    public TMP_Text totalAvgDriving;
    public TMP_Text totalAvgTraffic;
    public bool start = false;
    public CarSpawner carSpawner;
    void Awake(){

    }
    void Update(){
        if(start){
            setTotalCars(carSpawner.spawnedCars);
            setTotalDistance(carSpawner.totalDrove);
            setAvgDistance(carSpawner.totalDrove/carSpawner.spawnedCars);
            setTotalTime(carSpawner.totalTime);
            setTotalTrafficTime(carSpawner.totalTraffic);

            setTotalAvgTime(carSpawner.totalTime/carSpawner.spawnedCars);
            setTotalAvgTraffic(carSpawner.totalTraffic/carSpawner.spawnedCars);
            setTotalAvgDriving((carSpawner.totalTime-carSpawner.totalTraffic)/carSpawner.spawnedCars);
        }
    }
    public void setTotalCars(int num){
        totalCars.text = num.ToString();
    }
    public void setTotalDistance(int num){
        totalDistance.text = num.ToString();
    }
    public void setAvgDistance(int num){
        totalAvgDistance.text = num.ToString();
    }
     public void setTotalTime(int num)
    {
        totalTime.text = num.ToString()+"m";
    }

    public void setTotalTrafficTime(int num)
    {
        totalTrafficTime.text = num.ToString()+"m";
    }

    public void setTotalAvgTraffic(int num)
    {
        totalAvgTraffic.text = num.ToString()+"m";
    }
    public void setTotalAvgTime(int num)
    {
        totalAvgTime.text = num.ToString()+"m";
    }

    public void setTotalAvgDriving(int num)
    {
        totalAvgDriving.text = num.ToString()+"m";
    }
  
}
