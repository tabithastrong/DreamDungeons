using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoomGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    public DungeonTileMap nightmareStyle;
    public DungeonTileMap[] stylesToPick;

    public Transform teleportPrefab;
    public Transform clownPrefab;
    public Transform dogPrefab;
    public Transform dinoPrefab;
    public Transform spiderPrefab;
    public Transform shadowPrefab;
    public Transform childPrefab;
    public Transform coinPrefab;

    ChildGenerator childGenerator;

    public Transform musicPlayerPrefab;

    public Vector2Int rangeWidth = new Vector2Int(5, 15);
    public Vector2Int rangeHeight = new Vector2Int(5, 15);
    public List<Room> rooms;

    public PlayerInputSystem player;
    public AudioSource playerMusicSource;

    DreamToken childToken1;
    DreamToken childToken2;
    bool wentBackToMainMenuYet = false;

    public enum DoorType {
        NONE,
        NORMAL,
        BOSS
    }
    public struct Room
    {
        public List<Enemy> enemies;
        public Vector2Int position;
        public Vector2Int size;
        public Bounds bounds;

        public DungeonTileMap incompleteStyle;
        public DungeonTileMap completedStyle;

        public bool completed;

        public DoorType doorTop;
        public DoorType doorBottom;
        public DoorType doorLeft;
        public DoorType doorRight;

        public Vector2Int doorTopGoesTo;
        public Vector2Int doorBottomGoesTo;
        public Vector2Int doorLeftGoesTo;
        public Vector2Int doorRightGoesTo;
    }

    Vector2Int GetOpposite(Vector2Int dir)
    {
        return dir *= -1;
    }

    void Start()
    {
        Vector2Int position = Vector2Int.zero;
        List<Vector2Int> directions = new List<Vector2Int>() {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        rooms = new List<Room>();

        Vector2Int pos = Vector2Int.zero;
        Vector2Int lastDir = Vector2Int.zero;

        Dictionary<Vector2Int, bool> occupiedRooms = new Dictionary<Vector2Int, bool>();
        Vector2Int gridPos = Vector2Int.zero;
        DungeonTileMap style = stylesToPick[Random.Range(0, stylesToPick.Length)];

        int amountOfRooms = Random.Range(4, 12);

        for (int i = 0; i < amountOfRooms; i++)
        {
            bool isBossRoom = i == amountOfRooms - 1;

            Vector2Int dir = directions[Random.Range(0, 4)];
            Vector2Int size = new Vector2Int(Random.Range(rangeWidth.x, rangeWidth.y) + 2, Random.Range(rangeHeight.x, rangeHeight.y) + 2);

            int tries = 0;
            while (occupiedRooms.ContainsKey(gridPos + dir) && occupiedRooms[gridPos + dir] == true)
            {
                if (tries == 10)
                {
                    break;
                }

                dir = directions[Random.Range(0, 4)];
                tries++;
            }

            if (tries == 10)
            {
                break;
            }

            if (dir == GetOpposite(lastDir))
            {
                dir = lastDir;
            }

            Room room = new Room();
            room.position = pos;
            room.size = size;
            room.completed = false;
            room.incompleteStyle = nightmareStyle;
            room.completedStyle = style;

            if(rooms.Count > 0) {
                int dirDoor = directions.IndexOf(lastDir);
                Room lastRoom = rooms[rooms.Count - 1];

                if(dirDoor == 0) {
                    lastRoom.doorTop = isBossRoom ? DoorType.BOSS : DoorType.NORMAL;
                    lastRoom.doorTopGoesTo = new Vector2Int(room.position.x + room.size.x / 2, room.position.y + 2);
                } else if(dirDoor == 1) {
                    lastRoom.doorRight = isBossRoom ? DoorType.BOSS : DoorType.NORMAL;
                    lastRoom.doorRightGoesTo = new Vector2Int(room.position.x + 2, room.position.y + room.size.y / 2);
                } else if(dirDoor == 2) {
                    lastRoom.doorBottom = isBossRoom ? DoorType.BOSS : DoorType.NORMAL;
                    lastRoom.doorBottomGoesTo = new Vector2Int(room.position.x + room.size.x / 2, room.position.y + room.size.y - 2);
                } else if(dirDoor ==3) {
                    lastRoom.doorLeft = isBossRoom ? DoorType.BOSS : DoorType.NORMAL;
                    lastRoom.doorLeftGoesTo = new Vector2Int(room.position.x + room.size.x - 2, room.position.y + room.size.y / 2);
                }

                rooms[rooms.Count - 1] = lastRoom;

                int lastDirDoor = directions.IndexOf(GetOpposite(lastDir));

                if(lastDirDoor == 0) {
                    room.doorTop = DoorType.NORMAL;
                    room.doorTopGoesTo = new Vector2Int(lastRoom.position.x + lastRoom.size.x / 2, lastRoom.position.y + 2);
                } else if(lastDirDoor == 1) {
                    room.doorRight = DoorType.NORMAL;
                    room.doorRightGoesTo = new Vector2Int(lastRoom.position.x + 2, lastRoom.position.y + lastRoom.size.y / 2);
                } else if(lastDirDoor == 2) {
                    room.doorBottom = DoorType.NORMAL;
                    room.doorBottomGoesTo = new Vector2Int(lastRoom.position.x + lastRoom.size.x / 2, lastRoom.position.y + lastRoom.size.y - 2);
                } else if(lastDirDoor ==3) {
                    room.doorLeft = DoorType.NORMAL;
                    room.doorLeftGoesTo = new Vector2Int(lastRoom.position.x + lastRoom.size.x - 2, lastRoom.position.y + lastRoom.size.y / 2);
                }
            }

            float area = size.x * size.y;
            int amountOfEnemies = isBossRoom ? 1 : Mathf.CeilToInt(area / 25);
            List<Enemy> enemies = new List<Enemy>();

            for(int j = 0; j < amountOfEnemies; j++) {
                Transform enemyType = clownPrefab;
                float rng = Random.Range(0f, 1f);

                if(rng < 0.25f) {
                    enemyType = clownPrefab;
                } else if(rng < 0.5f) {
                    enemyType = dogPrefab;
                } else if(rng < 0.7f) {
                    enemyType = dinoPrefab;
                } else if(rng < 0.9f) {
                    enemyType = spiderPrefab;
                } else {
                    enemyType = shadowPrefab;
                }

                GameObject enemy = Instantiate(enemyType, new Vector3(room.position.x + 2 + Random.Range(0, room.size.x-4), room.position.y + 2 + Random.Range(0, room.size.y-4)), Quaternion.identity).gameObject;
                Enemy enemyComponent = enemy.GetComponent<Enemy>();

                if(isBossRoom) {
                    enemyComponent.isBoss = true;
                }

                enemies.Add(enemyComponent);
            }

            room.enemies = enemies;

            rooms.Add(room);

            if(isBossRoom) {
                Transform transform = Instantiate(childPrefab, new Vector3(room.position.x + room.size.x/2, room.position.y + room.size.y / 2), Quaternion.identity);
                childGenerator = transform.GetComponent<ChildGenerator>();
            }

            pos += (dir * size * 3) + dir;
            lastDir = dir;
            gridPos += dir;
        }

        foreach (Room room in rooms)
        {
            GenerateRoomOnTilemap(room);
        }
    }

    void GenerateRoomOnTilemap(Room room) {
        DungeonTileMap style = room.completed ? room.completedStyle : room.incompleteStyle;

        for (int y = 0; y < room.size.y; y++)
            {
                for (int x = 0; x < room.size.x; x++)
                {
                    Tile tile = style.GetFloorTile();

                    // If bottom row
                    if (y == 0)
                    {
                        //if left corner
                        if (x == 0)
                        {
                            tile = style.GetWallCorner(1, false);
                        }
                        else if(x == room.size.x / 2 && room.doorBottom != DoorType.NONE) {
                            tile = style.GetDoorTile(2, room.doorBottom == DoorType.BOSS);

                            GameObject go = Instantiate(teleportPrefab, new Vector3(room.position.x + x, room.position.y + y) + (new Vector3(0.5f, 0.5f)), Quaternion.identity).gameObject;
                            go.GetComponent<DungeonDoorTeleporter>().teleportTo = room.doorBottomGoesTo;
                        }
                        else if (x == room.size.x - 1)
                        {
                            // if right corner
                            tile = style.GetWallCorner(2, false);
                        }
                        else
                        {
                            // if middle bit
                            tile = style.GetWallTile(2, Random.Range(0, 100) < 15);
                        }
                    }
                    //if top row
                    else if (y == room.size.y - 1)
                    {
                        //if left corner
                        if (x == 0)
                        {
                            tile = style.GetWallCorner(0, false);
                        }
                        else if(x == room.size.x / 2 && room.doorTop != DoorType.NONE) {
                            tile = style.GetDoorTile(0, room.doorTop == DoorType.BOSS);

                            GameObject go = Instantiate(teleportPrefab, new Vector3(room.position.x + x, room.position.y + y) + (new Vector3(0.5f, 0.5f)), Quaternion.identity).gameObject;
                            go.GetComponent<DungeonDoorTeleporter>().teleportTo = room.doorTopGoesTo;
                        }
                        else if (x == room.size.x - 1)
                        {
                            // if right corner
                            tile = style.GetWallCorner(3, false);
                        }
                        else
                        {
                            // if middle bit
                            tile = style.GetWallTile(0, Random.Range(0, 100) < 15);
                        }
                    }
                    // if anywhere else
                    else
                    {
                        //if first wall
                        if (x == 0)
                        {
                            if(y == room.size.y / 2 && room.doorLeft != DoorType.NONE) {
                                tile = style.GetDoorTile(1, room.doorLeft == DoorType.BOSS);
                            GameObject go = Instantiate(teleportPrefab, new Vector3(room.position.x + x, room.position.y + y) + (new Vector3(0.5f, 0.5f)), Quaternion.identity).gameObject;
                            go.GetComponent<DungeonDoorTeleporter>().teleportTo = room.doorLeftGoesTo;
                            } else {
                                tile = style.GetWallTile(1, Random.Range(0, 100) < 15);
                            }
                        }
                        // if last wall
                        else if (x == room.size.x - 1)
                        {
                            if(y == room.size.y / 2 && room.doorRight != DoorType.NONE) {
                                tile = style.GetDoorTile(3, room.doorRight == DoorType.BOSS);
                            GameObject go = Instantiate(teleportPrefab, new Vector3(room.position.x + x, room.position.y + y) + (new Vector3(0.5f, 0.5f)), Quaternion.identity).gameObject;
                            go.GetComponent<DungeonDoorTeleporter>().teleportTo = room.doorRightGoesTo;
                            } else {
                                tile = style.GetWallTile(3, Random.Range(0, 100) < 15);
                            }
                        }
                    }

                    tilemap.SetTile(new Vector3Int(room.position.x + x, room.position.y + y), tile);
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        Room playerCurrentlyIn = rooms[0];
        Vector2 playerPos = player.transform.position;

        bool allRoomsComplete = true;

        for(int i = 0; i < rooms.Count; i++) {
            Room room = rooms[i];
            bool enemyAlive = false;

            if(playerPos.x > room.position.x && playerPos.x < room.position.x + room.size.x && playerPos.y > room.position.y && playerPos.y < room.position.y + room.size.y) {
                playerCurrentlyIn = room;
                Camera.main.backgroundColor = room.completed ? room.completedStyle.skyboxColor : room.incompleteStyle.skyboxColor;
            } 

            foreach(Enemy e in room.enemies) {
                if(e.health > 0) {
                    enemyAlive = true;
                    break;
                }
            }

            if(!enemyAlive && !room.completed) {
                room.completed = true;
                rooms[i] = room;
                GenerateRoomOnTilemap(room);
            }

            if(!room.completed) {
                allRoomsComplete = false;
            }
        }

        if(allRoomsComplete && !childGenerator.isHappy) {
            childGenerator.isHappy = true;

            childToken1 = Instantiate(coinPrefab, childGenerator.transform.position + new Vector3(-0.6f, -1f), Quaternion.identity).GetComponent<DreamToken>();
            childToken2 = Instantiate(coinPrefab, childGenerator.transform.position + new Vector3(0.6f, -1f), Quaternion.identity).GetComponent<DreamToken>();
        }

        if(allRoomsComplete && childToken1.pickedUp && childToken2.pickedUp && !wentBackToMainMenuYet) {
            wentBackToMainMenuYet = true;
            StartCoroutine(player.BackToMainMenu(1f, 0));
        }

        AudioClip roomClip = playerCurrentlyIn.completed ? playerCurrentlyIn.completedStyle.musicToPlayWhenCompleted : playerCurrentlyIn.completedStyle.musicToPlayWhenNotCompleted;
        
        if(playerMusicSource.clip != roomClip) {
            float time = playerMusicSource.time;

            playerMusicSource.Stop();
            playerMusicSource.clip = roomClip;
            playerMusicSource.Play();
            playerMusicSource.time = time;
        }
    }
}
