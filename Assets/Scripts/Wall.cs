//2021 Levi D. Smith
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public List<Cell> connectedCells;
    public GameObject model;

    void Start() {
        
    }

    void Update() {
        
    }

    public void setHighlighted(bool in_highlighted) {
        if (in_highlighted) {
            model.GetComponent<Renderer>().material.color = new Color(0.2553f, 0.6878f, 0.865f);
        }
    }
}