using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InsightPuzzle : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private List<Vector3Int> solutionTiles;

    [SerializeField] private Sprite tileOffToggleable;
    [SerializeField] private Sprite tileOnToggleable;
    [SerializeField] private Sprite tileOffUntoggleable;
    [SerializeField] private Sprite tileOnUntoggleable;

    private Dictionary<Sprite, Sprite> toggledTilesMap;

    // runs before start!
    void Awake()
    {
        // setup:
        toggledTilesMap = new Dictionary<Sprite, Sprite>() {
            {tileOffToggleable, tileOnToggleable},
            {tileOnToggleable, tileOffToggleable},
        };
        solutionTiles = new List<Vector3Int>();

        // record all solution tiles and reset them!
        foreach (Vector3Int tilePos in tileMap.cellBounds.allPositionsWithin)
        {
            // try to get a tile from cell position
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            if (tileSprite != null)
            {
                if (tileSprite == tileOnToggleable)
                {
                    solutionTiles.Add(tilePos);

                    // set each on tile to off!
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.hideFlags = HideFlags.DontSave; // <-- NEED THIS SO UNITY DOESNT REPLACE TILE PAST PLAY SESSION
                    newTile.sprite = tileOffToggleable;

                    // update the tilemap UI to the player
                    tileMap.SetTile(tilePos, newTile);
                    tileMap.RefreshTile(tilePos);
                }
            }
        }
    }

    void Update()
    {
        // toggle a tile if toggleable!
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePos = tileMap.WorldToCell(worldPoint);

            // try to get a tile from cell position
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            // if we clicked on a tile
            if (tileSprite && toggledTilesMap.ContainsKey(tileSprite))
            {
                // set each on tile to off!
                Tile newTile = ScriptableObject.CreateInstance<Tile>();
                newTile.hideFlags = HideFlags.DontSave; // <-- NEED THIS SO UNITY DOESNT REPLACE TILE PAST PLAY SESSION
                newTile.sprite = toggledTilesMap[tileSprite];

                // update the tilemap UI to the player
                tileMap.SetTile(tilePos, newTile);
                tileMap.RefreshTile(tilePos);
            }
        }
    }

    public void CheckIfWon()
    {
        List<Vector3Int> solutionTilesCopy = new List<Vector3Int>(solutionTiles);

        foreach (Vector3Int tilePos in tileMap.cellBounds.allPositionsWithin)
        {
            // try to get a tile from cell position
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            if (tileSprite != null && tileSprite == tileOnToggleable)
            {
                if (solutionTilesCopy.Contains(tilePos))
                {
                    solutionTilesCopy.Remove(tilePos);
                }
                else
                {
                    break;
                }
            }
        }

        if (solutionTilesCopy.Count == 0)
        {
            WonGame();
        }
    }

    public void WonGame()
    {
        Debug.Log("wooo you won!");

        StartCoroutine(GameManager.inst.Sleep(1.0f, GameManager.inst.EndCurrentPuzzle));
    }
}
