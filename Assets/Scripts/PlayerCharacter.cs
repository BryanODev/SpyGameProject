using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementComponent))]
public class PlayerCharacter : MonoBehaviour
{
    PlayerInputs PlayerInputsComponent;
    PlayerMovementComponent MovementComponent;
    PlayerCamera PlayerCameraComponent;

    public Transform Controller;
    bool sprinting;
    float controllerPitch;
    Vector3 LastInput;

    bool sneaking;
    IInteractable objectInteracting;

    private void Awake()
    {
        PlayerInputsComponent = new PlayerInputs();
        PlayerInputsComponent.Enable();
        Controller = transform;

        PlayerCameraComponent = GetComponentInChildren<PlayerCamera>();
        MovementComponent = GetComponent<PlayerMovementComponent>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayerInputs();
    }

    void SetupPlayerInputs()
    {
        if (PlayerInputsComponent != null)
        {
            //Input goes here

            //PlayerInputsComponent.Keyboard.Move.performed += ctx => AddMovementInput(ctx.ReadValue<Vector2>());
            PlayerInputsComponent.Keyboard.LookUp.performed += ctx => AddControllerPitch(ctx.ReadValue<float>());
            PlayerInputsComponent.Keyboard.LookRight.performed += ctx => AddControllerYaw(ctx.ReadValue<float>());

            PlayerInputsComponent.Keyboard.Jump.performed += ctx => Jump();

            PlayerInputsComponent.Keyboard.Sprint.performed += ctx => StartSprint();
            PlayerInputsComponent.Keyboard.Sprint.canceled += ctx => StopSprinting();

            PlayerInputsComponent.Keyboard.Crouch.performed += ctx => StartCrouch();
            PlayerInputsComponent.Keyboard.Crouch.canceled += ctx => StopCrouch();

            PlayerInputsComponent.Keyboard.Interact.performed += ctx => Interact();
            PlayerInputsComponent.Keyboard.Interact.canceled += ctx => Interact();
        }
    }

    private void Update()
    {
        Move(PlayerInputsComponent.Keyboard.Move.ReadValue<Vector2>());

        sneaking = MovementComponent.IsCrouching();
    }

    void StartSprint()
    {
        MovementComponent.SetIsSprinting(true);
    }

    void StopSprinting()
    {
        if (MovementComponent)
        {
            MovementComponent.SetIsSprinting(false);
        }
    }

    void StartCrouch()
    {
        if (MovementComponent)
        {
            MovementComponent.Crouch();
        }
    }

    void StopCrouch()
    {
        MovementComponent.UnCrouch();
    }

    void Move(Vector2 Direction)
    {
        if (Direction == Vector2.zero)
        {
            LastInput = Vector2.zero;
            return;
        }

        if (Direction != Vector2.zero)
        {
            Vector3 DesireDirection = new Vector3(Direction.x, 0, Direction.y);
            DesireDirection = transform.TransformDirection(DesireDirection);
            LastInput = DesireDirection;
            AddMovementInput(DesireDirection);
        }
    }

    void AddMovementInput(Vector3 Direction)
    {
        MovementComponent.ReceiveInput(Direction);
    }

    void AddControllerPitch(float Axis)
    {
        //PlayerCamera.Rotate(Vector3.right, Axis);

        controllerPitch += Axis;
        controllerPitch = Mathf.Clamp(controllerPitch, -90f, 90f);

        //Camera rotation only allowed if game us not paused
        if (PlayerCameraComponent != null)
        {
            PlayerCameraComponent.transform.rotation = Quaternion.Euler(controllerPitch, PlayerCameraComponent.transform.rotation.eulerAngles.y, PlayerCameraComponent.transform.rotation.eulerAngles.z);
        }
    }

    void Interact() 
    {
        if (PlayerCameraComponent != null)
        {
            RaycastHit hit;

            if (Physics.Raycast(PlayerCameraComponent.transform.position, PlayerCameraComponent.transform.forward, out hit, 2.0f))
            {
                objectInteracting = hit.transform.GetComponent<IInteractable>();

                if (objectInteracting != null)
                {
                    objectInteracting.OnInteract();
                }
            }
        }
    }

    void StopInteract() 
    {
        if (objectInteracting != null) 
        {
            objectInteracting.OnInteractStop();
        }
    }

    void AddControllerYaw(float Axis)
    {
        if (Controller)
        {
            Controller.Rotate(Vector3.up * Axis);
        }
    }

    void Jump() 
    {
        MovementComponent.PerformJump();
    }

    public PlayerCamera GetPlayerCameraComponent() 
    {
        return PlayerCameraComponent;
    }
}
