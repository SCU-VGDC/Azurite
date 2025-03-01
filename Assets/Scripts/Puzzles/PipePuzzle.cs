using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PipePuzzle : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private Sprite tileBackground;
    [SerializeField] private Sprite tileStart;
    [SerializeField] private Sprite tileEnd;
    [SerializeField] private Sprite tileStraight;
    [SerializeField] private Sprite  tileTShape;
    [SerializeField] private Sprite  tilePlusShape;
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
        InitializeDictionaries();
    }

    void InitializeDictionaries()
    {
        tileQuaternionToVector = new Dictionary<int, Vector3Int>() {
            {0, new Vector3Int(1, 0, 0)},
            {270, new Vector3Int(0, -1, 0)},
            {180, new Vector3Int(-1, 0, 0)},
            {90, new Vector3Int(0, 1, 0)},
        };

        // Initialize pipe connections (adjust these based on your actual pipe connections)
        spriteToPipeInfo = new Dictionary<Sprite, PipeInfo>();
        spriteToPipeInfo.Add(tileStart, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 90),   // Up
            Quaternion.Euler(0, 0, 180),  // Left
            Quaternion.Euler(0, 0, 270)  // Down
        }));
        
        spriteToPipeInfo.Add(tileEnd, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 90),   // Up
            Quaternion.Euler(0, 0, 180),  // Left
            Quaternion.Euler(0, 0, 270)  // Down
        }));

        spriteToPipeInfo.Add(tileStraight, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 180)   // Left
        }));

        spriteToPipeInfo.Add(tileTShape, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 90),   // Up
            Quaternion.Euler(0, 0, 180)   // Left
        }));

        spriteToPipeInfo.Add(tilePlusShape, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 90),   // Up
            Quaternion.Euler(0, 0, 180),  // Left
            Quaternion.Euler(0, 0, 270)  // Down
        }));

        spriteToPipeInfo.Add(tileLShape, new PipeInfo(new List<Quaternion> {
            Quaternion.Euler(0, 0, 0),    // Right
            Quaternion.Euler(0, 0, 90),   // Up
        }));


        // Wet sprites mapping
        spriteToWetSprite = new Dictionary<Sprite, Sprite> {
            {tileStart, tileStartWet},
            {tileEnd, tileEndWet},
            {tileStraight, tileStraightWet},
            {tileTShape, tileTShapeWet},
            {tilePlusShape, tilePlusShapeWet},
            {tileLShape, tileLShapeWet}
        };
    }

    public void RunGame()
    {
        Vector3Int? startPos = FindStartPosition();
        if (!startPos.HasValue) return;

        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Stack<Vector3Int> stack = new Stack<Vector3Int>();
        stack.Push(startPos.Value);
        visited.Add(startPos.Value);

        bool reachedEnd = false;

        while (stack.Count > 0 && !reachedEnd)
        {
            Vector3Int current = stack.Pop();
            SetWetTile(current);

            if (IsEndTile(current))
            {
                reachedEnd = true;
                break;
            }

            foreach (Vector3Int neighbor in GetConnectedNeighbors(current))
            {
                if (!visited.Contains(neighbor) && IsValidConnection(current, neighbor))
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                }
            }
        }

        Debug.Log(reachedEnd ? "Puzzle Solved!" : "No Path to End");
    }

    Vector3Int? FindStartPosition()
    {
        foreach (Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            if (tileMap.GetSprite(pos) == tileStart)
                return pos;
        }
        return null;
    }

    bool IsEndTile(Vector3Int position)
    {
        return tileMap.GetSprite(position) == tileEnd;
    }

    IEnumerable<Vector3Int> GetConnectedNeighbors(Vector3Int position)
    {
        Sprite sprite = tileMap.GetSprite(position);
        if (!spriteToPipeInfo.TryGetValue(sprite, out PipeInfo pipeInfo)) yield break;

        Quaternion rotation = tileMap.GetTransformMatrix(position).rotation;

        foreach (Quaternion openSide in pipeInfo.OpenSides)
        {
            Quaternion combinedRotation = rotation * openSide;
            Vector3Int direction = GetDirection(combinedRotation);
            Vector3Int neighborPos = position + direction;

            if (tileMap.HasTile(neighborPos))
                yield return neighborPos;
        }
    }

    bool IsValidConnection(Vector3Int from, Vector3Int to)
    {
        Vector3Int direction = to - from;
        Vector3Int reverseDirection = -direction;

        // Check if end tile
        if (IsEndTile(to))
        {
            return CheckEndTileConnection(to, reverseDirection);
        }

        // Check regular tile connection
        Sprite neighborSprite = tileMap.GetSprite(to);
        if (!spriteToPipeInfo.TryGetValue(neighborSprite, out PipeInfo neighborInfo)) return false;

        Quaternion neighborRotation = tileMap.GetTransformMatrix(to).rotation;

        foreach (Quaternion openSide in neighborInfo.OpenSides)
        {
            Quaternion combined = neighborRotation * openSide;
            Vector3Int neighborDir = GetDirection(combined);
            if (neighborDir == reverseDirection)
                return true;
        }

        return false;
    }

    bool CheckEndTileConnection(Vector3Int endPos, Vector3Int requiredDirection)
    {
        if (!spriteToPipeInfo.TryGetValue(tileEnd, out PipeInfo endInfo)) return false;

        Quaternion endRotation = tileMap.GetTransformMatrix(endPos).rotation;

        foreach (Quaternion openSide in endInfo.OpenSides)
        {
            Quaternion combined = endRotation * openSide;
            Vector3Int dir = GetDirection(combined);
            if (dir == requiredDirection)
                return true;
        }

        return false;
    }

    Vector3Int GetDirection(Quaternion rotation)
    {
        float angle = rotation.eulerAngles.z;
        int roundedAngle = Mathf.RoundToInt(angle / 90) * 90;
        return tileQuaternionToVector[roundedAngle % 360];
    }

    void SetWetTile(Vector3Int position)
    {
        Sprite currentSprite = tileMap.GetSprite(position);
        if (spriteToWetSprite.TryGetValue(currentSprite, out Sprite wetSprite))
        {
            Tile wetTile = ScriptableObject.CreateInstance<Tile>();
            wetTile.sprite = wetSprite;
            Matrix4x4 matrix = tileMap.GetTransformMatrix(position);
            tileMap.SetTile(position, wetTile);
            tileMap.SetTransformMatrix(position, matrix);
        }
    }



    // void Start()
    // {
    //     tileQuaternionToVector = new Dictionary<int, Vector3Int>() {
    //         {0, new Vector3Int(1, 0, 0)},
    //         {270, new Vector3Int(0, -1, 0)},
    //         {180, new Vector3Int(-1, 0, 0)},
    //         {90, new Vector3Int(0, 1, 0)},
    //     };

    //     // both start and end pipe defaulted to all sides
    //     PipeInfo startPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, -90f), Quaternion.Euler(0f, 0, -180f), Quaternion.Euler(0f, 0, -270f)});
    //     PipeInfo endPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, 0f), Quaternion.Euler(0f, 0, -90f), Quaternion.Euler(0f, 0, -180f), Quaternion.Euler(0f, 0, -270f)});
        
    //     PipeInfo straightPipeInfo = new PipeInfo(new List<Quaternion> {Quaternion.Euler(0f, 0, -180f), Quaternion.Euler(0f, 0, -90f)});
    //     PipeInfo TShapePipeInfo = new PipeInfo(new List<Quaternion> {});
    //     PipeInfo PlusShapePipeInfo = new PipeInfo(new List<Quaternion> {});
    //     PipeInfo LShapePipeInfo = new PipeInfo(new List<Quaternion> {});
    // }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //         Vector3Int tilePos = tileMap.WorldToCell(worldPoint);

    //         // try to get a tile from cell position
    //         Sprite tileSprite = tileMap.GetSprite(tilePos);

    //         // if we clicked on a tile
    //         if (tileSprite)
    //         {
    //             // see if we should rotate the tile
    //             List<Sprite> tilesThatCanRotate = new List<Sprite>(){ tileStraight, tileTShape, tilePlusShape, tileLShape };

    //             if (tilesThatCanRotate.Contains(tileSprite)) 
    //             {
    //                 Quaternion currentRotation = tileMap.GetTransformMatrix(tilePos).rotation;

    //                 Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, currentRotation * Quaternion.Euler(0f, 0f, -90f), Vector3.one);
    //                 tileMap.SetTransformMatrix(tilePos, matrix);
    //             }
    //         }
    //     }
    // }

    // void RunGame()
    // {
    //     // find source
    //     Vector3Int? startPos = null;

    //     foreach (Vector3Int tilePos in tileMap.cellBounds.allPositionsWithin)
    //     {
    //         // try to get a tile from cell position
    //         Sprite tileSprite = tileMap.GetSprite(tilePos);

    //         if (tileSprite)
    //         {
    //             if (tileSprite == tileStart)
    //             {
    //                 startPos = tilePos;

    //                 break;
    //             }
    //         }
    //     }

    //     List<Vector3Int> visitedPipes = new List<Vector3Int>();
    //     // propagate through all the pipes!

    // }

    // private Vector3Int getDir(Quaternion quaternion)
    // {
    //     int roundToNearestTen = ((int)Math.Round(quaternion.eulerAngles.z / 10.0)) * 10;

    //     return tileQuaternionToVector[roundToNearestTen];
    // }
}
