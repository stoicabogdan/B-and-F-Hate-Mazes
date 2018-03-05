﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaze : MonoBehaviour {

    // Initialize all private Variables
    private class centreOfCells
    {
        public float cellXCoordinate = 0.0f;
        public float cellZCoordinate = 0.0f;
        public float cellYCoordinate = 0.0f;


        public centreOfCells(float cellX, float cellY, float cellZ)
        {
            cellXCoordinate = cellX;
            cellYCoordinate = cellY;
            cellZCoordinate = cellZ;
        }


    }
    private BinaryTree createnewMaze;
    private int Columns;
    private int Rows;
    private centreOfCells[,] arrayOfCellsCentre;


    // Initialize all public Variables
    [SerializeField]
    public enum MazeType { Binary, Prim, Random };
    public MazeType myMaze;   
    public GameObject xWallPrefab;
    public GameObject zWallPrefab;
    public GameObject betweenWallsPrefab;
    public GameObject PlayerPrefab;
    public GameObject LightDropPrefab;
    public GameObject FloorPrefab;
    public int MinNumRows = 4;
    public int MaxNumRows = 8;
    public int MinNumColumns = 4;
    public int MaxNumColumns = 8;
    public float distanceBetweenObjects = 0.1f;
    public float xStartingPoint = 0.0f;
    public float zStartingPoint = 0.0f;
    public float yStartingPoint = 0.0f;
    [SerializeField]
    private Transform pos;
    

   


    // Use this for initialization
    void Start() {
        //GameObject Floor = Instantiate(Resources.Load("Floor", typeof(GameObject))) as GameObject;

        Rows = Random.Range(MinNumColumns,MaxNumRows);
        Columns = Random.Range(MinNumColumns,MaxNumColumns);

        CreateMazeWalls();

        LightDrop lightDrop = new LightDrop(PlayerPrefab, LightDropPrefab);
        lightDrop.setNumberLightDrops(Rows,Columns);




    }

    public void placeFloor(float distanceBetweenObjects,float wallPrefabLength, float betweenPrefabLength, float wallPrefabHeight)
    {
        float xFloor,zFloor,yFloor;
        yFloor = yStartingPoint - wallPrefabHeight/2;
        xFloor = (xStartingPoint + (2 * distanceBetweenObjects + wallPrefabHeight + betweenPrefabLength) * Rows + betweenPrefabLength)/-2;
        zFloor = (zStartingPoint + (2 * distanceBetweenObjects + wallPrefabHeight + betweenPrefabLength) * Columns + betweenPrefabLength)/ -2;
        Vector3 floorPosition = new Vector3(xFloor, yFloor, zFloor);
        Quaternion floorRotation = new Quaternion(0, 0, 0, 0);
        Instantiate(FloorPrefab, floorPosition, floorRotation);
    }

    public void placePlayer()
    {
        int columnOrRow = Random.Range(0, 11);

        int posX = Random.Range(0,Rows-1);
        int posZ = Random.Range(0,Columns-1);
        Vector3 playerPosition = new Vector3(arrayOfCellsCentre[posX,posZ].cellXCoordinate, arrayOfCellsCentre[posX, posZ].cellYCoordinate,arrayOfCellsCentre[posX,posZ].cellZCoordinate);
        Quaternion playerRotation = new Quaternion(0,0,0,0);
        Instantiate(PlayerPrefab,playerPosition,playerRotation);
    }

    public void placeExit()
    { }

    public void CreateMazeWalls()
    {
        createnewMaze = new BinaryTree(Rows, Columns);
        CreateCell.Cell[,] MazeArray = createnewMaze.returnBinaryArray();
        float wallPrefabLength = xWallPrefab.GetComponent<Renderer>().bounds.size.x;
        float wallPrefabHeight = xWallPrefab.GetComponent<Renderer>().bounds.size.y;
        float betweenPrefabLength = betweenWallsPrefab.GetComponent<Renderer>().bounds.size.x;
        float betweenPrefabHeight = betweenWallsPrefab.GetComponent<Renderer>().bounds.size.y;
        arrayOfCellsCentre = new centreOfCells[Rows,Columns];
        float wallIsHigher = 0.0f;
        float betweenIsHigher = 0.0f;
        if(betweenPrefabHeight > wallPrefabHeight)
        {
            betweenIsHigher = (betweenPrefabHeight - wallPrefabHeight)/2;
        }
        else
        {
            wallIsHigher = (wallPrefabHeight - betweenPrefabHeight)/2;
        }


        for (int i = 0; i <= Rows; i++)
        {
            for (int j = 0; j <= Columns; j++)
            {
                
                Vector3 pos = new Vector3((xStartingPoint + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) * i) * -1, yStartingPoint + betweenIsHigher, (zStartingPoint + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) * j) * -1);
                GameObject aux = Instantiate(betweenWallsPrefab, pos, Quaternion.identity);
                aux.transform.parent = transform;
                if (i < Rows && j<Columns)
                { 
                    arrayOfCellsCentre[i, j] = new centreOfCells((xStartingPoint + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) * i + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) / 2) * -1, yStartingPoint + betweenIsHigher, (zStartingPoint + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) * j + (2 * distanceBetweenObjects + wallPrefabLength + betweenPrefabLength) / 2) * -1);
                }
            }
        }


        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {

                if (i == 0)
                {
                    Vector3 pos = new Vector3((xStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * i) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * j) * -1);
                    GameObject aux = Instantiate(zWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;
                }
                if (j == 0)
                {                  
                    Vector3 pos = new Vector3((xStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * i) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * j) * -1);
                    GameObject aux = Instantiate(xWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;

                }
                if (j == Columns - 1)
                {
                    Vector3 pos = new Vector3((xStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * i) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * (j + 1)) * -1);

                    GameObject aux = Instantiate(xWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;
                }
                if (i == Rows - 1)
                {
                    Vector3 pos = new Vector3((xStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * (i + 1)) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * j) * -1);
                    GameObject aux = Instantiate(zWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;
                }

            }
        }


        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                GameObject aux = null;
                if (MazeArray[i, j].getNeighbourEast() != null && MazeArray[i, j].isLinked(MazeArray[i, j].getNeighbourEast()) == false)
                {
                    int nEastRow = MazeArray[i, j].getNeighbourEast().getCellRow();
                    int nEastColumn = MazeArray[i, j].getNeighbourEast().getCellColumn();
                    Vector3 pos = new Vector3((xStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * nEastRow) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * nEastColumn) * -1);
                    aux = Instantiate(xWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;
                }

                if (MazeArray[i, j].getNeighbourSouth() != null && MazeArray[i, j].isLinked(MazeArray[i, j].getNeighbourSouth()) == false)
                {
                    int nSouthRow = MazeArray[i, j].getNeighbourSouth().getCellRow();
                    int nSouthColumn = MazeArray[i, j].getNeighbourSouth().getCellColumn();
                    Vector3 pos = new Vector3((xStartingPoint + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * nSouthRow) * -1, yStartingPoint + wallIsHigher, (zStartingPoint + (wallPrefabLength + betweenPrefabLength + distanceBetweenObjects) / 2 + (2 * distanceBetweenObjects + betweenPrefabLength + wallPrefabLength) * nSouthColumn) * -1);
                    aux = Instantiate(zWallPrefab, pos, Quaternion.identity);
                    aux.transform.parent = transform;
                }
                   
            }
        }


        placePlayer();
        placeFloor(distanceBetweenObjects, wallPrefabLength, betweenPrefabLength,wallPrefabHeight);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
