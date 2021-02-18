//2021 Levi D. Smith
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour {

    public const int MAZE_ROWS = 16;
    public const int MAZE_COLS = 16;

    public GameObject CellPrefab;
    public GameObject WallPrefab;
    public GameObject BoundaryPrefab;

    List<Cell> cells;
    List<Wall> walls;

    List<Cell> mazeCells;
    List<Cell> frontierCells; //for Prim's

    List<Wall> kruskalsWalls;

    public GameObject CellParent;
    public GameObject WallParent;

    float fMaxDelay = 0.2f;
    float fDelay;

    public SoundEffects soundeffects;

    bool isFinished;


    public DisplayManager displaymanager;

    public DisplayManager.MazeAlgorithm mazealgorithm;


    void Start() {
//        setupMazePrims();
//        fDelay = fMaxDelay;
        
    }

    void Update() {

        if (!isFinished) {
            if (fDelay > 0f) {
                fDelay -= Time.deltaTime;
                if (fDelay <= 0f) {
                    //do something
                    if (mazealgorithm == DisplayManager.MazeAlgorithm.prims) {
                        createMazePrimsSelectFrontier();
                    } else if (mazealgorithm == DisplayManager.MazeAlgorithm.kruskals) {
                        createMazeKruskalsSelectWall();
                    }
                    fDelay += fMaxDelay;
                }
            }
        }
        
    }

    public void setupCellsAndWalls() {
        isFinished = false;

        clearMaze();

        cells = new List<Cell>();
        setupCells();

        walls = new List<Wall>();
        setupWalls();

        fDelay = fMaxDelay;

    }
    public void setupMazePrims() {
        setupCellsAndWalls();

        createMazePrims();
    }

    public void setupMazeKruskals() {
        setupCellsAndWalls();

        createMazeKruskals();

    }

    private void clearMaze() {
        int i;

        for (i = 0; i < CellParent.transform.childCount; i++) {
            Destroy(CellParent.transform.GetChild(i).gameObject);
        }

        for (i = 0; i < WallParent.transform.childCount; i++) {
            Destroy(WallParent.transform.GetChild(i).gameObject);
        }

    }

    private void setupCells() {
        int i, j;

        for (i = 0; i < MAZE_ROWS; i++) {
            for (j = 0; j < MAZE_COLS; j++) {
                Cell cell = Instantiate(CellPrefab, new Vector3(j, 0f, i), Quaternion.identity).GetComponent<Cell>();
                cell.setup(i, j, this);
                cell.transform.SetParent(CellParent.transform);

                cells.Add(cell);
            }
        }

    }

    private void setupWalls() {
        //create boundary
        int i;
        GameObject gobj;
        for (i = 0; i < MAZE_COLS; i++) {
            gobj = Instantiate(BoundaryPrefab, new Vector3(i, 0f, -0.5f), Quaternion.Euler(0f, 90f, 0f));
            gobj.transform.SetParent(WallParent.transform);
            gobj = Instantiate(BoundaryPrefab, new Vector3(i, 0f, MAZE_ROWS - 0.5f), Quaternion.Euler(0f, 90f, 0f));
            gobj.transform.SetParent(WallParent.transform);
        }

        for (i = 0; i < MAZE_ROWS; i++) {
            gobj = Instantiate(BoundaryPrefab, new Vector3(-0.5f, 0f, i), Quaternion.Euler(0f, 0f, 0f));
            gobj.transform.SetParent(WallParent.transform);
            gobj = Instantiate(BoundaryPrefab, new Vector3(MAZE_COLS - 0.5f, 0f, i), Quaternion.Euler(0f, 0f, 0f)); gobj.transform.SetParent(WallParent.transform);
        }


        //create walls
        foreach (Cell cell in cells) {
            foreach (Cell cell_neighbor in cell.getNeighbors()) {
                if (getNeighborWall(cell, cell_neighbor) == null) {
                    addWall(cell, cell_neighbor);
                }

            }
        }


    }

    private Wall getNeighborWall(Cell in_cell, Cell in_neighbor_cell) {
        Wall wallReturn = null;

        foreach (Wall wall in walls) {
            if (wall.connectedCells.Contains(in_cell) && wall.connectedCells.Contains(in_neighbor_cell)) {
                wallReturn = wall;
            }
        }

        return wallReturn;
    }

    private void addWall(Cell in_cell, Cell in_neighbor_cell) {
        float xOffset = (in_neighbor_cell.iCol - in_cell.iCol) * 0.5f;
        float zOffset = (in_neighbor_cell.iRow - in_cell.iRow) * 0.5f;

        float rot = 0f;
        if (in_cell.iRow != in_neighbor_cell.iRow) {
            rot = 90f;
        }

        Wall wall = Instantiate(WallPrefab, new Vector3(in_cell.iCol + xOffset, 0f, in_cell.iRow + zOffset), Quaternion.Euler(0f, rot, 0f)).GetComponent<Wall>();

        wall.connectedCells = new List<Cell>();
        wall.connectedCells.Add(in_cell);
        wall.connectedCells.Add(in_neighbor_cell);
        wall.transform.SetParent(WallParent.transform);
        walls.Add(wall);

    }


    public Cell getCell(int in_row, int in_col) {
        Cell cellReturn = null;
        foreach(Cell cell in cells) {
            if (cell.iRow == in_row && cell.iCol == in_col) {
                cellReturn = cell;
            }

        }


        return cellReturn;
    }
    public Cell getNeighbor(Cell in_cell, int in_row, int in_col) {
        Cell cellReturn = null;
        foreach (Cell cell in cells) {
            if (cell.iRow == in_cell.iRow + in_row && cell.iCol == in_cell.iCol + in_col) {
                cellReturn = cell;
            }

        }

        return cellReturn;
    }

    public List<Cell> getCellNeighbors(Cell in_cell) {
        List<Cell> listNeighbors = new List<Cell>();

        Cell cellNeighbor;

        cellNeighbor = getNeighbor(in_cell, 0, 1);
        if (cellNeighbor != null) {
            listNeighbors.Add(cellNeighbor);
        }

        cellNeighbor = getNeighbor(in_cell, 1, 0);
        if (cellNeighbor != null) {
            listNeighbors.Add(cellNeighbor);
        }

        cellNeighbor = getNeighbor(in_cell, 0, -1);
        if (cellNeighbor != null) {
            listNeighbors.Add(cellNeighbor);
        }

        cellNeighbor = getNeighbor(in_cell, -1, 0);
        if (cellNeighbor != null) {
            listNeighbors.Add(cellNeighbor);
        }


        return listNeighbors;
    }

    private List<Cell> getFrontierCells() {
        List<Cell> listCells = new List<Cell>();

        foreach (Cell cell in cells) {
            if (cell.isFrontier) {
                listCells.Add(cell);
            }
        }

        return listCells;
    }

    private void createMazePrims() {
        Cell randomCell = getCell(Random.Range(0, MAZE_ROWS), Random.Range(0, MAZE_COLS));
        mazeCells = new List<Cell>();
        frontierCells = new List<Cell>();

        randomCell.markCellPrims();
        mazeCells.Add(randomCell);


    }

    private void createMazePrimsSelectFrontier() {
        List<Cell> cellsFrontier = getFrontierCells();

        if (cellsFrontier.Count > 0) {
            //            Cell cellRandomFrontier = getFrontierCells()[Random.Range(0, getFrontierCells().Count)];
            Cell cellRandomFrontier = cellsFrontier[Random.Range(0, cellsFrontier.Count)];
            Cell cellNeighborInMaze = cellRandomFrontier.getNeighborInMaze();
            makePassagePrims(cellRandomFrontier, cellNeighborInMaze);
            cellRandomFrontier.markCellPrims();

            soundeffects.soundBeep.pitch = 0.5f * (1 + Random.Range(0, 4));
            soundeffects.soundBeep.Play();
        } else {
            isFinished = true;
            soundeffects.soundFinished.Play();
        }

    }

    private void makePassagePrims(Cell in_cell1, Cell in_cell2) {
        Wall wallToDelete = null;
        foreach (Wall wall in walls) {
            if (wall.connectedCells.Contains(in_cell1) && wall.connectedCells.Contains(in_cell2)) {
                wallToDelete = wall;
            }
        }

        if (wallToDelete != null) {
            Debug.Log("Destroy wall: " + wallToDelete.connectedCells[0] + ", " + wallToDelete.connectedCells[1]);
            walls.Remove(wallToDelete);
            Destroy(wallToDelete.gameObject);
        }
    }

    public void setMaxDelay(float fValue) {
        fMaxDelay = fValue;
        if (fDelay > fMaxDelay) {
            fDelay = fMaxDelay;
        }

    }



    private void createMazeKruskals() {
        kruskalsWalls = new List<Wall>();

        foreach (Wall wall in walls) {
            kruskalsWalls.Insert(Random.Range(0, kruskalsWalls.Count), wall);
        }

        int iID = 0;
        foreach (Cell cell in cells) {
            //cell.iKruskalID = iID;
            cell.setIDKruskals(iID);
            iID++;
        }

    }

    private void createMazeKruskalsSelectWall() {
        if (kruskalsWalls.Count > 0) {
            Wall selectedWall = kruskalsWalls[0];
            kruskalsWalls.RemoveAt(0);

            //I just used IDs to keep track of groups;  Could have used tree structures instead
            int iKruskalID1 = selectedWall.connectedCells[0].iKruskalID;
            int iKruskalID2 = selectedWall.connectedCells[1].iKruskalID;

            selectedWall.setHighlighted(true);

            bool isUsingSimpleImplementation = false;


            if (isUsingSimpleImplementation) {
                //simple implementation

                if (iKruskalID1 != iKruskalID2) {
                    foreach (Cell cell in cells) {

                        if (cell.iKruskalID == iKruskalID2) {
                            cell.setIDKruskals(iKruskalID1);
                        }
                    }

                    Destroy(selectedWall.gameObject);
                    soundeffects.soundBeep.pitch = 0.5f * (1 + Random.Range(0, 4));
                    soundeffects.soundBeep.Play();

                } else {
                    soundeffects.soundBloop.Play();

                }
            } else {
                //calculate the majority and minority of cells in joined areas
                //reduces amount of flashing between colors
                if (iKruskalID1 != iKruskalID2) {

                    int iKruskalID1Count = 0;
                    int iKruskalID2Count = 0;
                    foreach (Cell cell in cells) {
                        if (cell.iKruskalID == iKruskalID1) {
                            iKruskalID1Count++;
                        } else if (cell.iKruskalID == iKruskalID2) {
                            iKruskalID2Count++;
                        }
                    }

                    int iKruskalIDOld;
                    int iKruskalIDNew;

                    if (iKruskalID1Count > iKruskalID2Count) {
                        iKruskalIDOld = iKruskalID2;
                        iKruskalIDNew = iKruskalID1;
                    } else {
                        iKruskalIDOld = iKruskalID1;
                        iKruskalIDNew = iKruskalID2;
                    }


                    foreach (Cell cell in cells) {
                        if (cell.iKruskalID == iKruskalIDOld) {
                            cell.setIDKruskals(iKruskalIDNew);
                        }
                    }

                    Destroy(selectedWall.gameObject);
                    soundeffects.soundBeep.pitch = 0.5f * (1 + Random.Range(0, 4));
                    soundeffects.soundBeep.Play();


                } else {
                    soundeffects.soundBloop.Play();

                }



            }

        } else {
            isFinished = true;
            soundeffects.soundFinished.Play();
        }


    }



}