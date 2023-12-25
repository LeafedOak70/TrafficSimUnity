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
    public Button totalButton;
    public Button avgButton;
    public bool start = false;
    public CarSpawner carSpawner;
    public GameObject avgStats;
    public GameObject totalStats;
    public bool avgBool;
    public bool totalBool;
    public Button lowButton;
    public Button midButton;
    public Button highButton;
    public string currentRate;
    public int ogRate;
    void Awake(){
        totalButton.onClick.AddListener(showTotals);
        avgButton.onClick.AddListener(showAvg);

        lowButton.onClick.AddListener(setRateLow);
        midButton.onClick.AddListener(setRateMid);
        highButton.onClick.AddListener(setRateHigh);
        
    }
    void Update(){
        if(start){
            if(currentRate == "low"){
                if(ogRate == 2){
                    setRealStats();
                }else{
                    setLowStats();
                }
            }else if(currentRate == "mid"){
                if(ogRate == 1){
                    setRealStats();
                }else{
                    setMidStats();
                }
            }else if(currentRate == "high"){
                if(ogRate == 0){
                    setRealStats();
                }else{
                    setHighStats();
                }
            }
            // setRealStats();
        }
    }
    public void showAvg(){
        avgBool = true;
        totalBool = false;
        SetChildrenActive(avgStats,true);
        SetChildrenActive(totalStats, false);
    }
    public void showTotals(){
        avgBool = false;
        totalBool = true;
        SetChildrenActive(avgStats,false);
        SetChildrenActive(totalStats, true);
    }
    public void setRateLow(){currentRate = "low";}
    public void setRateMid(){currentRate = "mid";}
    public void setRateHigh(){currentRate = "high";}
    public void setRealStats(){
        setTotalCars(carSpawner.spawnedCars);
        setTotalDistance(carSpawner.totalDrove);
        setTotalTime(carSpawner.totalTime);
        setTotalTrafficTime(carSpawner.totalTraffic);

        setAvgDistance(carSpawner.totalDrove/carSpawner.spawnedCars);
        setTotalAvgTime(carSpawner.totalTime/carSpawner.spawnedCars);
        setTotalAvgTraffic(carSpawner.totalTraffic/carSpawner.spawnedCars);
        setTotalAvgDriving((carSpawner.totalTime-carSpawner.totalTraffic)/carSpawner.spawnedCars);
    }
    public void setHighStats(){
        setTotalCars(1158);
        setTotalTime(58071);
        setTotalDistance(137077);
        setTotalTrafficTime(11274);
        
        setAvgDistance(118);
        setTotalAvgTime(50);
        setTotalAvgDriving(27);
        setTotalAvgTraffic(22);  
    }
    public void setMidStats(){
        setTotalCars(460);
        setTotalDistance(36075);
        setTotalTime(20650);
        setTotalTrafficTime(11274);
        
        setAvgDistance(78);
        setTotalAvgTime(44);
        setTotalAvgDriving(20);
        setTotalAvgTraffic(24);  
    }
    public void setLowStats(){
        setTotalCars(227);
        setTotalDistance(25153);
        setTotalTime(11550);
        setTotalTrafficTime(6506);
        
        setAvgDistance(110);
        setTotalAvgTime(50);
        setTotalAvgDriving(22);
        setTotalAvgTraffic(28);  
    }
    private void SetChildrenActive(GameObject obj, bool active)
    {
        obj.SetActive(active);

        foreach (Transform child in obj.transform)
        {
            SetChildrenActive(child.gameObject, active);
        }
    }
    public void setTotalCars(int num){
        totalCars.text = num.ToString();
    }
    public void setTotalDistance(int num){
        totalDistance.text = num.ToString()+"m";
    }
    public void setAvgDistance(int num){
        totalAvgDistance.text = num.ToString()+"m";
    }
     public void setTotalTime(int num)
    {
        totalTime.text = num.ToString()+"min";
    }

    public void setTotalTrafficTime(int num)
    {
        totalTrafficTime.text = num.ToString()+"min";
    }

    public void setTotalAvgTraffic(int num)
    {
        totalAvgTraffic.text = num.ToString()+"min";
    }
    public void setTotalAvgTime(int num)
    {
        totalAvgTime.text = num.ToString()+"min";
    }

    public void setTotalAvgDriving(int num)
    {
        totalAvgDriving.text = num.ToString()+"min";
    }
  
}
