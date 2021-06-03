using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GroundDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    Tilemap tilemap;
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
            Debug.Log("pPos:" + pPos);        
            StartCoroutine(DestroyBlock(pPos));
        }
    }
    IEnumerator DestroyBlock(Vector3Int Pos){
        yield return new WaitForSeconds(0.5f);
        tilemap.SetTile(new Vector3Int(Pos.x, Pos.y-1, Pos.z), null);
    }
}
