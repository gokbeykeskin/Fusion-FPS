using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    bool isRespawnRequested = false;
    bool isGoToLobbyRequested = false;


    //Other components
    NetworkCharacterControllerCustom networkCharacterControllerPrototypeCustom;
    HPHandler hpHandler;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerCustom>();
        hpHandler = GetComponent<HPHandler>();
    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (isRespawnRequested)
            {
                Respawn();
                return;
            }
            if (isGoToLobbyRequested)
            {
                GoToLobby();
                return;
            }

        }


        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimForwardVector;

            //Cancel out rotation on X to avoid tilting.
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            //Move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump
            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();

            CheckFallRespawn();
            CheckPortal();
        }

    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasStateAuthority)
            {
                RequestGoToLobby();
            }

        }
    }

    void CheckPortal()
    {
        if(transform.position.x > 94 && transform.position.x <96 && transform.position.z > 95 && transform.position.z < 97)
        {
            if (Object.HasStateAuthority) RequestRespawn();
        }
    }

    public void RequestRespawn()
    {
        isRespawnRequested = true;
    }

    void Respawn()
    {
        networkCharacterControllerPrototypeCustom.TeleportToPosition(Utils.GetRandomSpawnPoint());

        hpHandler.OnRespawned();

        isRespawnRequested = false;
    }

    public void RequestGoToLobby()
    {
        isGoToLobbyRequested = true;
    }

    void GoToLobby()
    {
        networkCharacterControllerPrototypeCustom.TeleportToPosition(Utils.GetLobbySpawnPoint());
        hpHandler.OnDied();
        isGoToLobbyRequested = false;
        hpHandler.isDead = true;

    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }

}
