//2021 Levi D. Smith
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

 //   public List<Cell> cellNeighbors;
    List<Wall> walls;
    public int iRow;
    public int iCol;

    public GameObject WallPrefab;
    public MazeManager mazemanager;

    public bool isInMaze;  //for Prim's
    public bool isFrontier;  //for Prim's

    public int iKruskalID;  //for Kruskal's

    public GameObject model;

    void Start() {

    }

    void Update() {
        
    }

    public void setup(int in_row, int in_col, MazeManager in_mazemanager) {
        iRow = in_row;
        iCol = in_col;
        mazemanager = in_mazemanager;
        model.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

    }

    public List<Cell> getNeighbors() {
        List<Cell> cellNeighbors = mazemanager.getCellNeighbors(this);

        return cellNeighbors;

    }

    public Cell getNeighborInMaze() {
        Cell cellReturn = null;
        List<Cell> cellNeighborsInMaze = new List<Cell>();

        foreach (Cell cell in getNeighbors()) {
            if (cell.isInMaze) {
                cellNeighborsInMaze.Add(cell);
            }
        }

        if (cellNeighborsInMaze.Count > 0) {
            cellReturn = cellNeighborsInMaze[Random.Range(0, cellNeighborsInMaze.Count)];
        }

        return cellReturn;
    }

    public void markCellPrims() {
        isInMaze = true;
        isFrontier = false;

        model.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0f);
        
        foreach (Cell cell in getNeighbors()) {
            if (!cell.isInMaze) {
                cell.isFrontier = true;
                cell.model.GetComponent<Renderer>().material.color = new Color(1f, 0.5f, 0.5f);
            }
        }
    }

    public void setIDKruskals(int in_iKruskalID) {
        iKruskalID = in_iKruskalID;

        model.GetComponent<Renderer>().material.color = Color.HSVToRGB((float)iKruskalID / (float)(MazeManager.MAZE_ROWS * MazeManager.MAZE_COLS), 1f, 1f);

    }

    public override string ToString() {
        return "Cell[" + iRow + "," + iCol + "]";

    }
}