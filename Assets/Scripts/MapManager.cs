using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tilemap tilemap;
    
    [SerializeField] List<TileData> tileDatas;
    [SerializeField] Tile tiletoswap;   
    class TileTrigger{
        public int tillBroken;
        public bool canBreak = true;
        public TileTrigger(int i, bool b){
            tillBroken = i;
            canBreak = b;
        }
        
    }
    private Dictionary<Vector3Int,TileTrigger> posTiletoData;
    private Dictionary<TileBase, TileData> datafromTiles;
    private void Awake() {
        posTiletoData = new Dictionary<Vector3Int, TileTrigger>();
        datafromTiles = new Dictionary<TileBase, TileData>();
        foreach(var tileData in tileDatas){
            
            foreach(var tile in tileData.tiles){
                datafromTiles.Add(tile,tileData);
            }
        }
    }
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionStay2D(Collision2D other) {
        
        if (other.gameObject.tag == "Player")
        {        
            var pPos = tilemap.WorldToCell(other.rigidbody.position);
            
            pPos = new Vector3Int(pPos.x, pPos.y - Mathf.RoundToInt(other.transform.GetComponent<BoxCollider2D>().size.y), pPos.z);
            TileBase tilewalked = tilemap.GetTile(pPos);
            if(tilewalked){
                if(  !posTiletoData.ContainsKey(pPos)){
                Debug.Log(datafromTiles[tilewalked].tillBroken);
                posTiletoData.Add(pPos, new TileTrigger(datafromTiles[tilewalked].tillBroken, true));
            }
            else{
                StartCoroutine(BreakBlock(pPos, posTiletoData[pPos]));  
                Debug.Log(posTiletoData[pPos].tillBroken); 
            }
            }
        }
    }

    IEnumerator BreakBlock(Vector3Int Pos, TileTrigger t){
        
        if(t.canBreak){
            t.canBreak = false;
            t.tillBroken -= 1;
            if(t.tillBroken>0){
                tilemap.SetTile(Pos, tiletoswap);
            }
            
            yield return new WaitForSeconds(0.35f);
            if(t.tillBroken <= 0){
                tilemap.SetTile(Pos, null);
            }
            t.canBreak = true;

        }
        yield return null;
        
    }
}
