using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName =  "TileData", menuName =  "ScriptableObjects/TileData", order =  2)]
public class TileData : ScriptableObject
{
   public TileBase[] tiles;
   public int tillBroken = 2;
   
}
