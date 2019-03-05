using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public Text killCount;
    public Text deathCount;
    
    void Start()
    {
        if(UserAccountManager.IsLoggedIn)
        UserAccountManager.instance.GetData(OnReceivedData);
    }     
    void OnReceivedData(string data)
    {
        
        killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }

 
}
