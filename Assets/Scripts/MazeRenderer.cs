﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 100)]
    private int width = 10;

    [SerializeField]
    [Range(1, 100)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private GameObject obstaclePrefab = null; // New obstacle prefab

    protected Position startPosition;
    protected Position endPosition;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height, out startPosition, out endPosition);
        Draw(maze);
        InstantiatePlayer();
        PlaceObstacles(maze);
    }

    private void Draw(WallState[,] maze)
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 0.01f, height);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }
    }

    private void InstantiatePlayer()
    {
        float playerHeight = 0.1f; // Adjust this value to match the height of your player and floor level
        Vector3 startPos = new Vector3(-width / 2 + startPosition.X, playerHeight, -height / 2 + startPosition.Y);
        Instantiate(playerPrefab, startPos, Quaternion.identity);
    }

    private void PlaceObstacles(WallState[,] maze)
    {
        // Place obstacles randomly within the maze
        int numObstacles = (width * height) / 10; // Adjust number of obstacles as needed
        System.Random rand = new System.Random();

        for (int i = 0; i < numObstacles; ++i)
        {
            int x = rand.Next(1, width - 1);
            int y = rand.Next(1, height - 1);

            // Ensure obstacles are not placed at the start or end positions
            if ((x == startPosition.X && y == startPosition.Y) || (x == endPosition.X && y == endPosition.Y))
                continue;

            Vector3 position = new Vector3(-width / 2 + x, 0.5f, -height / 2 + y); // Adjust height as needed
            Instantiate(obstaclePrefab, position, Quaternion.identity);
        }
    }

    void Update()
    {
    }
}
