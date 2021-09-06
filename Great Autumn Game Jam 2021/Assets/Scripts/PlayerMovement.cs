using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

	public float movementSpeed = 5;

	void Start()
    {
        ///get the rididbody component that is attached to our player
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorz = Input.GetAxisRaw("Horizontal");
        float inputVert = Input.GetAxisRaw("Vertical");

        playerRigidBody.velocity = new Vector2(inputHorz * movementSpeed, inputVert * movementSpeed);
    }
}
