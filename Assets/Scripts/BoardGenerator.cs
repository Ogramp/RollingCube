using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;
    [Range(0, 1)]
    public float obstaclePercent;

    public float tileSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    
    public int seed;
    [HideInInspector]
    public Coord mapCenter;
    public Tile[,] tiles;

    void Start() {
        seed = Random.Range(0, 10);
        GenerateBoard();        
    }
    
    public void GenerateBoard() {
        tiles = new Tile[(int)mapSize.x, (int)mapSize.y];

        allTileCoords = new List<Coord>();
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);

        string holderName = "Generated Board";
        if (transform.FindChild(holderName))
            DestroyImmediate(transform.FindChild(holderName).gameObject);

        Transform boardHolder = new GameObject(holderName).transform;
        boardHolder.parent = transform;

        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;                
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = boardHolder;
                tiles[x, y] = newTile.GetComponent<Tile>();
                tiles[x, y].posX = x;
                tiles[x, y].posY = y;
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstaclecCount = 0;
        for(int i = 0; i < obstacleCount; i++) {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstaclecCount++;

            if (randomCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObstaclecCount)) {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                tiles[randomCoord.x, randomCoord.y].passable = false;

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = boardHolder;
                newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
            } else {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstaclecCount--;
            }
        }
                
        GenerateWalls();

    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;

        int accessibleTileCount = 1;

        while(queue.Count > 0) {
            Coord tile = queue.Dequeue();

            for(int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++) {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if(x == 0 || y == 0) {
                        if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
                            if(!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY]) {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    public Vector3 CoordToPosition(int x, int y) {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y) * tileSize;
    }

    public Coord GetRandomCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord {
        public int x;
        public int y;

        public Coord(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }
    }

    // Bad Code!!!
    // TODO: Refactor
    void GenerateWalls() {
        string wallHolderName = "Generated walls";
        if (transform.FindChild(wallHolderName))
            DestroyImmediate(transform.FindChild(wallHolderName).gameObject);
        Transform wallHolder = new GameObject(wallHolderName).transform;
        wallHolder.parent = transform;

        float cubeSize = 0.5f;
        GameObject westWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        westWall.name = "West wall";
        westWall.transform.parent = wallHolder.transform;
        westWall.transform.localScale = new Vector3(cubeSize, cubeSize, mapSize.y);
        westWall.transform.position = new Vector3(-mapSize.x /2 - (cubeSize / 2), 0, 0);
        westWall.GetComponent<Renderer>().sharedMaterial.color = Color.grey;
        GameObject northWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        northWall.name = "North wall";
        northWall.transform.parent = wallHolder.transform;
        northWall.transform.localScale = new Vector3(mapSize.x + (cubeSize * 2), cubeSize, cubeSize);
        northWall.transform.position = new Vector3(0, 0, mapSize.y / 2 + (cubeSize / 2));
        northWall.GetComponent<Renderer>().sharedMaterial.color = Color.grey;
        GameObject eastWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        eastWall.name = "East wall";
        eastWall.transform.parent = wallHolder.transform;
        eastWall.transform.localScale = new Vector3(cubeSize, cubeSize, mapSize.y);
        eastWall.transform.position = new Vector3(mapSize.x / 2 + (cubeSize / 2), 0, 0);
        eastWall.GetComponent<Renderer>().sharedMaterial.color = Color.grey;
        GameObject southWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        southWall.name = "South wall";
        southWall.transform.parent = wallHolder.transform;
        southWall.transform.localScale = new Vector3(mapSize.x + (cubeSize * 2), cubeSize, cubeSize);
        southWall.transform.position = new Vector3(0, 0, -mapSize.y / 2 - (cubeSize / 2));
        southWall.GetComponent<Renderer>().sharedMaterial.color = Color.grey;
    }

}
