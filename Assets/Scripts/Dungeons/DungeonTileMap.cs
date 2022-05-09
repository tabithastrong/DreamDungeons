using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "DreamJam/Dungeon Tile Map", fileName = "New Dungeon Tile Map.asset")]
public class DungeonTileMap : ScriptableObject
{
    public Sprite standardWall;
    public Sprite standardDoor;
    public Sprite bossDoor;
    public Sprite floor;
    public Sprite wallIn;
    public Sprite wallOut;
    public Sprite specialWall;

    public Color skyboxColor;

    public AudioClip musicToPlayWhenNotCompleted;
    public AudioClip musicToPlayWhenCompleted;

    public Transform enemyType;

    public Tile[] standardWallTiles;
    private Tile[] standardDoorTiles;
    private Tile[] bossDoorTiles;
    private Tile[] wallInTiles;
    private Tile[] wallOutTiles;
    private Tile[] specialWallTiles;
    private Tile floorTile;

    private void FillTileArray(Sprite s, ref Tile[] array, bool collider) {
        array = new Tile[4];

        for(int i = 0; i < 4; i++) {
            array[i] = Tile.CreateInstance<Tile>();
            array[i].sprite = s;
            array[i].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, i * 90f), Vector3.one);

            if(collider) {
                array[i].colliderType = Tile.ColliderType.Grid;
            }
        }
    }

    public void SetUpTiles() {
        FillTileArray(standardWall, ref standardWallTiles, true);
        FillTileArray(standardDoor, ref standardDoorTiles, true);
        FillTileArray(bossDoor, ref bossDoorTiles, true);
        FillTileArray(wallIn, ref wallInTiles, true);
        FillTileArray(wallOut, ref wallOutTiles, true);
        FillTileArray(specialWall, ref specialWallTiles, true);

        floorTile = Tile.CreateInstance<Tile>();
        floorTile.sprite = floor;
        floorTile.colliderType = Tile.ColliderType.None;
    }

    public Tile GetWallTile(int i, bool special) {
        if(floorTile == null) {
            SetUpTiles();
        }

        return special ? specialWallTiles[i] : standardWallTiles[i];
    }
    public Tile GetDoorTile(int i, bool boss) {
        if(floorTile == null) {
            SetUpTiles();
        }

        return boss ? bossDoorTiles[i] : standardDoorTiles[i];
    }

    public Tile GetWallCorner(int i, bool outWall) {
        if(floorTile == null) {
            SetUpTiles();
        }

        return outWall ? wallOutTiles[i] : wallInTiles[i];
    }

    public Tile GetFloorTile() {
        if(floorTile == null) {
            SetUpTiles();
        }

        return floorTile;
    }
    
}
