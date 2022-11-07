using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed=false;
    bool isFireButtonPressed = false;

    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;
    // Start is called before the first frame update
    void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponentInChildren<CharacterMovementHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (!characterMovementHandler.Object.HasInputAuthority) return;

        //Rotate input
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;
        //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
        //Jump Input
        if (Input.GetButtonDown("Jump"))
            isJumpButtonPressed = true;
        //Fire Input
        if (Input.GetButtonDown("Fire1"))
        {
            isFireButtonPressed = true;
        }

        localCameraHandler.SetViewInputVector(viewInputVector);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpPressed = isJumpButtonPressed;

        networkInputData.isFireButtonPressed = isFireButtonPressed;
        isJumpButtonPressed = false;
        isFireButtonPressed = false;

        return networkInputData;
    }
}
