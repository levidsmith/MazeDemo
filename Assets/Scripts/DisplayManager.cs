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
        mazemanager.setupMazePrims();
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


}