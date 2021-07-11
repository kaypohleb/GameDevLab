using UnityEngine;

[CreateAssetMenu(fileName =  "GameConstants", menuName =  "ScriptableObjects/GameConstants", order =  1)]
public  class GameConstants : ScriptableObject
{
	// for Scoring system
    public int startingLives = 3;
    public float startingTime = 300f;
    public int startingScore = 0;
    
    int currentScore;
    int currentTime;
    int currentLives;

    [Range(5f, 20f)] public float minGombaSpawnPoint = 9f;
    [Range(5f, 20f)] public float maxGombaSpawnPoint = 15f;

    public int breakTimeStep =  30;
    public int breakDebrisTorque =  10;
    public int breakDebrisForce =  10;
    public int spawnNumberOfDebris =  10;
    public float explosionTTL = 0.25f;
    public int growScore = 200;
    public int killScore = 100;
}