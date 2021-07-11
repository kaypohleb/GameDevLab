using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> powerupIcons;
    public PowerUpCollection powerups;
	public IntReference sceneNumber;
	public IntEvent usePowerup;
	// Start is called before the first frame update
	void  Start()
	{
		if(sceneNumber==1){
			powerups.init(powerupIcons.Count);
			for (int i =  0; i<powerupIcons.Count; i++){
				powerupIcons[i].SetActive(false);	
			}
		}else{
			for (int i =  0; i<powerupIcons.Count; i++){
			if(powerups.collection[i]==null){
				powerupIcons[i].SetActive(false);	
			}else{
				powerupIcons[i].GetComponent<Image>().sprite  =  powerups.collection[i].sprite;
	    		powerupIcons[i].SetActive(true);	
			}
		}
		}
		
		
	}
	
    public  void  addPowerup(PowerUp p){
		int type = (int)p.powerType;
	    if (type  <  powerupIcons.Count){
	    	powerupIcons[type].GetComponent<Image>().sprite  =  p.sprite;
	    	powerupIcons[type].SetActive(true);	
	    }
        powerups.addPowerup(p);
    }

	public  void  removePowerup(int index){
	    if (index  <  powerupIcons.Count){
			powerupIcons[index].GetComponent<Image>().sprite  =  null;
	        powerupIcons[index].SetActive(false);
	    }
    }

    void  cast(int i){
		if (powerups.collection[i] !=  null){
			powerups.removePowerup(i);
			removePowerup(i);
			usePowerup.Raise(i);
		}
	}
    public void  consumePowerup(KeyCode k){
	        switch(k){
		    case  KeyCode.Q:
		    	cast(0);
		    	break;
		    case  KeyCode.E:
		    	cast(1);
		    	break;
		    default:
		    	break;
	    }
    }
}
