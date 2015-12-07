using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //public GameController gameController;
    public BoardGenerator board;
    public Transform turningPoint;
    
    Tile currentTile;
    Tile targetTile;

    bool isMoving;
    
    void Start() {
        transform.position = board.CoordToPosition(board.mapCenter.x, board.mapCenter.y) + Vector3.up * 0.5f;
        currentTile = board.tiles[board.mapCenter.x, board.mapCenter.y];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetButtonDown("Horizontal") && !isMoving) {

            int direction = (int)Input.GetAxisRaw("Horizontal");    

            // Check if we can move
            if (currentTile.posX + direction >= 0 && currentTile.posX + direction < board.mapSize.x)
                targetTile = board.tiles[currentTile.posX + direction, currentTile.posY];
            
            // Moving horizontally
            if (targetTile != null && targetTile.passable) {
                turningPoint.position = new Vector3(transform.position.x + direction * transform.lossyScale.x / 2, 0, transform.position.z);
                StartCoroutine(RollCube(-direction, Vector3.forward));
            } 
        } else if (Input.GetButtonDown("Vertical") && !isMoving) {
            int direction = (int)Input.GetAxisRaw("Vertical");

            // Check if we can move            
            if (currentTile.posY + direction >= 0 && currentTile.posY + direction < board.mapSize.y)
                targetTile = board.tiles[currentTile.posX, currentTile.posY + direction];

            // Moving vertically
            if (targetTile != null && targetTile.passable) {
                turningPoint.position = new Vector3(transform.position.x, 0, transform.position.z + direction * transform.lossyScale.z / 2);
                StartCoroutine(RollCube(direction, Vector3.right));                
            }          
        }        
    }

    IEnumerator RollCube(float direction, Vector3 movingDirection) {
        isMoving = true;
        currentTile = targetTile;
        targetTile = null;
        for (int i = 0; i < 30; i++) {
            transform.RotateAround(turningPoint.position, movingDirection, direction * 3);
            yield return new WaitForSeconds(0.01f);
        }        
        turningPoint.rotation = Quaternion.identity;
        isMoving = false;
    }    

}
