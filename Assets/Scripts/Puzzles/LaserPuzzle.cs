using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserPuzzle : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private GameObject laserObject;

    private const char tileBackground = '0';
    private const char tileLaserSource = '1';
    private const char tileOneMirror= '2';
    private const char tileTwoMirror = '3';
    private const char tileSplitter = '4';
    private const char tileLaserEnd = '5';

    private Dictionary<int, Vector3Int> tileQuaternionToVector;

    private Quaternion rotateByMirror1 = Quaternion.Euler(0f, 0, -180f);
    private Quaternion rotateByMirror2 = Quaternion.Euler(0f, 0, -270f);
    private Quaternion rotateByMirror3 = Quaternion.Euler(0f, 0, 0);
    private Quaternion rotateByMirror4 = Quaternion.Euler(0f, 0, -90f);
    private Vector3Int mirrorDir1, mirrorDir2, mirrorDir3, mirrorDir4;

    private class LaserCursor
    {
        public LaserCursor(Vector3Int cursorPos, Vector3Int cursorDir)
        {
            CursorPos = cursorPos;
            CursorDir = cursorDir;
        }

        public Vector3Int CursorPos { get; set; }
        public Vector3Int CursorDir { get; set; }
    }

    void Start()
    {
        tileQuaternionToVector = new Dictionary<int, Vector3Int>() {
            {0, new Vector3Int(1, 0, 0)},
            {270, new Vector3Int(0, -1, 0)},
            {180, new Vector3Int(-1, 0, 0)},
            {90, new Vector3Int(0, 1, 0)},
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePos = tileMap.WorldToCell(worldPoint);

            // try to get a tile from cell position
            TileBase tile = tileMap.GetTile(tilePos);

            // if we clicked on a tile
            if (tile)
            {
                char tileID = tile.name[tile.name.Length - 1];

                // see if we should rotate the tile
                List<char> tilesThatCanRotate = new List<char>(){ tileOneMirror, tileTwoMirror, tileSplitter };

                if (tilesThatCanRotate.Contains(tileID)) 
                {
                    Quaternion currentRotation = tileMap.GetTransformMatrix(tilePos).rotation;

                    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, currentRotation * Quaternion.Euler(0f, 0f, -90f), Vector3.one);
                    tileMap.SetTransformMatrix(tilePos, matrix);
                }
            }
        }
    }

    // run to see if player won!
    public void RunGame()
    {
        // find laser source
        TileBase sourceTile = null;
        Vector3Int? sourcePos = null;
        Vector3Int? sourceDir = null;

        foreach (Vector3Int tilePos in tileMap.cellBounds.allPositionsWithin)
        {
            // try to get a tile from cell position
            TileBase tile = tileMap.GetTile(tilePos);

            if (tile)
            {
                char tileID = tile.name[tile.name.Length - 1];

                if (tileID == tileLaserSource)
                {
                    sourceTile = tile;
                    sourcePos = tilePos;
                    
                    // rotated source to the left 90 degrees because the art display the laser exiting to the left
                    Quaternion rotateBy = Quaternion.Euler(0f, 0, -180f);
                    sourceDir = getDir(tileMap.GetTransformMatrix(tilePos).rotation * rotateBy);

                    break;
                }
            }
        }

        // draw the laser! its laser time
        Queue<LaserCursor> laserQueue = new Queue<LaserCursor>(); 
        // add OG cursor
        laserQueue.Enqueue(new LaserCursor((Vector3Int) sourcePos + (Vector3Int) sourceDir, (Vector3Int) sourceDir));

        // double looped just to make it easier to read the cursorPos/cursorDir
        while (laserQueue.Peek() != null) 
        {
            // check next tile
            TileBase tile = tileMap.GetTile(laserQueue.Peek().CursorPos);
            if (tile == null)
            {
                laserQueue.Dequeue();
            }
            Debug.Log(laserQueue.Peek().CursorPos);

            // draw laser
            Instantiate(laserObject, laserQueue.Peek().CursorPos + new Vector3(0.5f, 0.5f, 1f), Quaternion.identity);


            char tileID = tile.name[tile.name.Length - 1];
            switch (tileID) 
            {
                case tileBackground:
                    // continue in same direction
                    laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;

                    break;

                case tileLaserSource:
                    laserQueue.Dequeue();
                    
                    break;

                case tileOneMirror:
                    // check to see if laser hits the correct sides of the mirror
                    mirrorDir1 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror1);
                    mirrorDir2 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror2);
                    
                    if (laserQueue.Peek().CursorDir + mirrorDir1 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir2;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else if (laserQueue.Peek().CursorDir + mirrorDir2 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir1;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else 
                    {
                        laserQueue.Dequeue();
                    }

                    break;

                case tileTwoMirror:
                    // check to see if laser hits the correct sides of the mirror                    
                    mirrorDir1 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror1);
                    mirrorDir2 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror2);
                    
                    mirrorDir3 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror3);
                    mirrorDir4 = getDir(tileMap.GetTransformMatrix(laserQueue.Peek().CursorPos).rotation * rotateByMirror4);
                    
                    if (laserQueue.Peek().CursorDir + mirrorDir1 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir2;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else if (laserQueue.Peek().CursorDir + mirrorDir2 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir1;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else if (laserQueue.Peek().CursorDir + mirrorDir3 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir4;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else if (laserQueue.Peek().CursorDir + mirrorDir4 == Vector3Int.zero)
                    {
                        laserQueue.Peek().CursorDir = mirrorDir3;

                        laserQueue.Peek().CursorPos += laserQueue.Peek().CursorDir;
                    }
                    else 
                    {
                        laserQueue.Dequeue();
                    }
                    
                    break;

                case tileSplitter:
                
                    break;

                case tileLaserEnd:
                    // you won!
                    
                    laserQueue.Dequeue();

                    break;
            }
        }
    }

    private Vector3Int getDir(Quaternion quaternion)
    {
        int roundToNearestTen = ((int)Math.Round(quaternion.eulerAngles.z / 10.0)) * 10;

        return tileQuaternionToVector[roundToNearestTen];
    }
}
