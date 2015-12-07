using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    
    public Tile tile;
    public GameObject tileGrid;
    public GameObject wallParent;
    public Vector2 tileGridSize;

    public int powerUpCount;

    Tile[,] tiles;

    void Awake() {
        tiles = new Tile[(int)tileGridSize.x, (int)tileGridSize.y];

        GenerateGrid(); // make the grid of tiles
        GenerateWalls(); // make the walls surrounding the grid
    }    

    void GenerateGrid() {
        for (int i = 0; i < tileGridSize.y; i++) {
            for (int j = 0; j < tileGridSize.x; j++) {
                Vector3 pos = new Vector3(j, 0, i);
                tiles[j,i] = Instantiate(tile, pos, Quaternion.Euler(90, 0, 0)) as Tile;
                tiles[j,i].transform.parent = tileGrid.transform.GetChild(0).transform;
                
            }
        }
    }

    void GenerateWalls() {
        float cubeSize = 0.5f;
        GameObject westWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        westWall.name = "West wall";
        westWall.transform.parent = wallParent.transform;
        westWall.transform.localScale = new Vector3(cubeSize, cubeSize, tileGridSize.y);
        westWall.transform.position = new Vector3(-cubeSize - (cubeSize / 2), 0, tileGridSize.y / 2 - cubeSize);
        westWall.GetComponent<Renderer>().material.color = Color.grey;
        GameObject northWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        northWall.name = "North wall";
        northWall.transform.parent = wallParent.transform;
        northWall.transform.localScale = new Vector3(tileGridSize.x + (cubeSize * 2), cubeSize, cubeSize);
        northWall.transform.position = new Vector3(tileGridSize.x / 2 - cubeSize, 0, tileGridSize.y - (cubeSize / 2));
        northWall.GetComponent<Renderer>().material.color = Color.grey;
        GameObject eastWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        eastWall.name = "East wall";
        eastWall.transform.parent = wallParent.transform;
        eastWall.transform.localScale = new Vector3(cubeSize, cubeSize, tileGridSize.y);
        eastWall.transform.position = new Vector3(tileGridSize.x - cubeSize / 2, 0, tileGridSize.y / 2 - cubeSize);
        eastWall.GetComponent<Renderer>().material.color = Color.grey;
        GameObject southWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        southWall.name = "South wall";
        southWall.transform.parent = wallParent.transform;
        southWall.transform.localScale = new Vector3(tileGridSize.x + (cubeSize * 2), cubeSize, cubeSize);
        southWall.transform.position = new Vector3(tileGridSize.x / 2 - cubeSize, 0, -cubeSize - (cubeSize / 2));
        southWall.GetComponent<Renderer>().material.color = Color.grey;
    }

    public Tile GetTile(int x, int y) {
        return tiles[x, y];
    }

}
