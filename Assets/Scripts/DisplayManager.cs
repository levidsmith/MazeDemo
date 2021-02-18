//2021 Levi D. Smith
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour {

    bool isCameraOrthographic = true;
    public Camera cameraOrthographic;
    public Camera cameraPerspective;


    public GameObject playButton;

    public MazeManager mazemanager;

    public GameObject PlayerPrefab;

    public Dropdown dropdownMazeAlgorithm;

    public Slider slider;

    public enum MazeAlgorithm { prims, kruskals };

    void Start() {
        
    }

    void Update() {
        
    }

    public void mazeReady() {
        playButton.SetActive(true);
    }



    public void setMaxDelay(Slider slider) {
        float fValue;
        fValue = slider.maxValue + slider.minValue - slider.value;

        mazemanager.setMaxDelay(fValue);

    }

    public void doRestart() {
        setMaxDelay(slider);
        mazemanager.mazealgorithm = getMazeAlgorithm();

        if (mazemanager.mazealgorithm == MazeAlgorithm.prims) {
            mazemanager.setupMazePrims();
        } else if (mazemanager.mazealgorithm == MazeAlgorithm.kruskals) {
            mazemanager.setupMazeKruskals();
        }


    }

    public void toggleCamera() {

        Player player = GameObject.FindObjectOfType<Player>();
        if (player != null) {
            Destroy(player.gameObject);
        }

        isCameraOrthographic = !isCameraOrthographic;
        if (isCameraOrthographic) {
            cameraOrthographic.gameObject.SetActive(true);
            cameraPerspective.gameObject.SetActive(false);
        } else {
            cameraOrthographic.gameObject.SetActive(false);
            cameraPerspective.gameObject.SetActive(true);

        }

    }

    public void doPlay() {
        cameraOrthographic.gameObject.SetActive(false);
        cameraPerspective.gameObject.SetActive(false);

        Instantiate(PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity);


    }

    public MazeAlgorithm getMazeAlgorithm() {
        MazeAlgorithm mazealgorithm;

        switch(dropdownMazeAlgorithm.value) {
            case 0:
                mazealgorithm = MazeAlgorithm.prims;
                break;
            case 1:
                mazealgorithm = MazeAlgorithm.kruskals;
                break;
            default:
                mazealgorithm = MazeAlgorithm.prims;
                break;
        }



        return mazealgorithm;
    }


}