using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatUIManager : MonoBehaviour
{
    public TMP_Text totalCars;
    public TMP_Text totalDistance;
    public TMP_Text totalAvgDistance;
    void Awake(){
        
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
  
}
