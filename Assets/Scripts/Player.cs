//2021 Levi D. Smith
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        handleInput();   
    }

    private void handleInput() {
        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up, fHorizontal * 90f * Time.deltaTime);

        Vector3 vectMove = transform.forward * fVertical;
        float fMoveSpeed = 10f;
        //        transform.GetComponent<Rigidbody>().MovePosition(transform.position + vectMove.normalized * fMoveSpeed * Time.deltaTime);
        CharacterController charactercontroller = GetComponent<CharacterController>();
        charactercontroller.Move(vectMove * fMoveSpeed * Time.deltaTime);

        

    }
}