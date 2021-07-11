using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public int spawnNumber = 2;
    public GameConstants gameConstants;
    
    private void Awake() {
        for (int j =  0; j  <  spawnNumber; j++){
	        spawnFromPooler(ObjectType.gombaEnemy);
        }
    }
    void Update(){
        if(getCount()<spawnNumber){
            spawnFromPooler(ObjectType.gombaEnemy);
        }
    }
    void spawnFromPooler(ObjectType i){
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);
        if(item != null){
            item.transform.position = new Vector3(Random.Range(gameConstants.minGombaSpawnPoint, gameConstants.maxGombaSpawnPoint), 5f,0);
            //Debug.Log(item.transform.position);
            item.SetActive(true);
        }
    }
    public void addSpawn(PowerUp p){
        spawnNumber++;
    }
    int getCount(){
        int count = 0;
        foreach (ExistingPoolItem item in ObjectPooler.SharedInstance.pooledObjects){
            if(item.gameObject.activeSelf){
                count++;
            }
        }
        return count;
    }
}
