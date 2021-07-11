using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "PowerUpCollection", menuName = "GameDevLab/PowerUpCollection", order = 0)]
public class PowerUpCollection : ScriptableObject {
    public List<PowerUp> collection;
    public void init(int ls){
        collection = new List<PowerUp>();
        for (int i =  0; i<ls; i++){
			collection.Add(null);
		}
    }
    public  void  addPowerup(PowerUp p){
        int type = (int)p.powerType;
        collection[type] =  p;
    }

    public void removePowerup(int index){
        collection[index] =  null;
    }
    
}
