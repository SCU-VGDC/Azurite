using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PipePuzzle : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private Sprite tileBackground;
    [SerializeField] private Sprite tileStart;
    [SerializeField] private Sprite tileEnd;
    [SerializeField] private Sprite tileStraight;
    [SerializeField] private Sprite tileTShape;
    [SerializeField] private Sprite tilePlusShape;
    [SerializeField] private Sprite tileLShape;
    [SerializeField] private Sprite tileStartWet;
    [SerializeField] private Sprite tileEndWet;
    [SerializeField] private Sprite tileStraightWet;
    [SerializeField] private Sprite tileTShapeWet;
    [SerializeField] private Sprite tilePlusShapeWet;
    [SerializeField] private Sprite tileLShapeWet;

    private Dictionary<int, Vector3Int> tileQuaternionToVector;
    private Dictionary<Sprite, PipeInfo> spriteToPipeInfo;
    private Dictionary<Sprite, Sprite> spriteToWetSprite;

    private HashSet<Vector3Int> visitedPipes;

    private Dictionary<Vector3Int, Tile> visitedPipesToReset;

    private class PipeInfo
    {
        public PipeInfo(List<Quaternion> openSides)
        {
            OpenSides = openSides;
        }

        public List<Quaternion> OpenSides { get; set; }
    }

    void Start()
    {
        tileQuaternionToVector = new Dictionary<int, Vector3Int>() {
            {0, new Vector3Int(1, 0, 0)},
            {90, new Vector3Int(0, 1, 0)},
            {180, new Vector3Int(-1, 0, 0)},
            {270, new Vector3Int(0, -1, 0)},
        };

        // both start and end pipe defaulted to all sides
        PipeInfo startPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 90f), Quaternion.Euler(0f, 0, 180f), Quaternion.Euler(0f, 0, 270f)});
        PipeInfo endPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 90f), Quaternion.Euler(0f, 0, 180f), Quaternion.Euler(0f, 0, 270f)});
        
        // set sides for all types of pipe
        PipeInfo straightPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 180f)});
        PipeInfo TShapePipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 90f), Quaternion.Euler(0f, 0, 180f)});
        PipeInfo PlusShapePipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 90f), Quaternion.Euler(0f, 0, 180f), Quaternion.Euler(0f, 0, 270f)});
        PipeInfo LShapePipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, 90f)});

        // initialize quick look up dicts
        spriteToPipeInfo = new Dictionary<Sprite, PipeInfo> {
            {tileStart, startPipeInfo},
            {tileEnd, endPipeInfo},
            {tileStraight, straightPipeInfo},
            {tileTShape, TShapePipeInfo},
            {tilePlusShape, PlusShapePipeInfo},
            {tileLShape, LShapePipeInfo},
        };

        spriteToWetSprite = new Dictionary<Sprite, Sprite> {
            {tileStart, tileStartWet},
            {tileEnd, tileEndWet},
            {tileStraight, tileStraightWet},
            {tileTShape, tileTShapeWet},
            {tilePlusShape, tilePlusShapeWet},
            {tileLShape, tileLShapeWet},
        };

        // for DPS
        visitedPipes = new HashSet<Vector3Int>();
        visitedPipesToReset = new Dictionary<Vector3Int, Tile>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePos = tileMap.WorldToCell(worldPoint);

            // try to get a tile from cell position
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            // if we clicked on a tile
            if (tileSprite)
            {
                // see if we should rotate the tile
                List<Sprite> tilesThatCanRotate = new List<Sprite>(){ tileStraight, tileTShape, tilePlusShape, tileLShape };

                if (tilesThatCanRotate.Contains(tileSprite)) 
                {
                    Quaternion currentRotation = tileMap.GetTransformMatrix(tilePos).rotation;

                    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, currentRotation * Quaternion.Euler(0f, 0f, -90f), Vector3.one);
                    tileMap.SetTransformMatrix(tilePos, matrix);
                }
            }
        }
    }

    public void RunGame()
    {
        // find source
        Vector3Int? startPos = null;

        foreach (Vector3Int tilePos in tileMap.cellBounds.allPositionsWithin)
        {
            // try to get a tile from cell position
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            if (tileSprite != null)
            {
                if (tileSprite == tileStart)
                {
                    startPos = tilePos;

                    break;
                }
            }
        }

        // propagate through all the pipes!
        if (startPos != null)
        {
            DFS((Vector3Int) startPos);
        }
    }

    public void ResetGame()
    {
        foreach (Vector3Int visitedPos in visitedPipes)
        {
            // if this tile needs to be reset
            if (visitedPipesToReset.ContainsKey(visitedPos))
            {
                Tile tileToReset = visitedPipesToReset[visitedPos];

                tileMap.SetTile(visitedPos, tileToReset);
                tileMap.RefreshTile(visitedPos);
            }
        }

        visitedPipes = new HashSet<Vector3Int>();
    }

    private void DFS(Vector3Int tilePos)
    {
        TileBase tile = tileMap.GetTile(tilePos);

        if (tile != null)
        {
            // make sure we don't repeat tiles
            visitedPipes.Add(tilePos);

            // get sprite to determine what kind of tile this is
            Sprite tileSprite = tileMap.GetSprite(tilePos);

            // check to see if won!
            if (tileSprite == tileEnd)
            {
                Debug.Log("Won!");
            }

            // get tile transform matrix for rotation
            Matrix4x4 transformMatrix = tileMap.GetTransformMatrix(tilePos);
            
            // save current tile (for resetting)
            Tile copyOfCurrentTile = ScriptableObject.CreateInstance<Tile>();
            copyOfCurrentTile.hideFlags = HideFlags.DontSave; // <-- NEED THIS SO UNITY DOESNT REPLACE TILE PAST PLAY SESSION
            copyOfCurrentTile.transform = transformMatrix;
            copyOfCurrentTile.sprite = tileSprite;
            
            visitedPipesToReset[tilePos] = copyOfCurrentTile;

            // make wet
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.hideFlags = HideFlags.DontSave; // <-- NEED THIS SO UNITY DOESNT REPLACE TILE PAST PLAY SESSION
            newTile.transform = transformMatrix;
            newTile.sprite = spriteToWetSprite[tileSprite];
            
            tileMap.SetTile(tilePos, newTile);
            tileMap.RefreshTile(tilePos);

            PipeInfo currentPipeInfo = spriteToPipeInfo[tileSprite]; 
            
            // for every entrance propagate!
            foreach (Quaternion openSide in currentPipeInfo.OpenSides)
            {
                Vector3Int nextDir = getDir(transformMatrix.rotation * openSide);

                Vector3Int nextPos = tilePos + nextDir;
                Sprite nextTileSprite = tileMap.GetSprite(nextPos);
                
                // only propagate if not visited and if the tile is a pipe
                if (!visitedPipes.Contains(nextPos) && nextTileSprite != null && spriteToPipeInfo.ContainsKey(nextTileSprite))
                {
                    foreach (Quaternion nextOpenSide in spriteToPipeInfo[nextTileSprite].OpenSides)
                    {
                        if (nextDir + getDir(tileMap.GetTransformMatrix(nextPos).rotation * nextOpenSide) == Vector3Int.zero)
                        {
                            DFS(nextPos);
                        }
                    }
                }
            }
        }
    }

    private Vector3Int getDir(Quaternion quaternion)
    {
        int roundToNearestTen = ((int)Math.Round(quaternion.eulerAngles.z / 10.0)) * 10;

        return tileQuaternionToVector[roundToNearestTen];
    }
}
