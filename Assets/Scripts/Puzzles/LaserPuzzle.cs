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
                    Quaternion rotateBy = Quaternion.Euler(0f, 0f, -90f);
                    sourceDir = Vector3Int.FloorToInt((tileMap.GetTransformMatrix(tilePos).rotation * rotateBy).eulerAngles);

                    break;
                }
            }
        }

        // draw the laser! its laser time
        Vector3Int cursorPos = (Vector3Int) sourcePos + (Vector3Int) sourceDir;
        Vector3Int cursorDir = (Vector3Int) sourceDir;
        bool stopLaser = false;
        while (tileMap.GetTile(cursorPos) != null && !stopLaser)
        {
            Debug.Log(cursorPos);
            
            // draw laser
            Instantiate(laserObject, cursorPos, Quaternion.identity);

            // check next tile
            TileBase tile = tileMap.GetTile(cursorPos);
            char tileID = tile.name[tile.name.Length - 1];

            switch (tileID) 
            {
                case tileBackground:
                    // continue in same direction
                    cursorPos += cursorDir;

                    break;

                case tileLaserSource:
                    stopLaser = true;
                    break;

                case tileOneMirror:
                
                    break;

                case tileTwoMirror:
                
                    break;

                case tileSplitter:
                
                    break;

                case tileLaserEnd:
                    // you won!
                    
                    stopLaser = true;

                    break;
            }


        }
    }
}
