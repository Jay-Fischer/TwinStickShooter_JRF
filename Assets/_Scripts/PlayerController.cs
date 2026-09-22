using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    TS_Inputs _inputs;
    CharacterController cc;

    [Header("Player Controls")]
    public float moveSpeed = 4f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;
    private float speedyTimer;

    [Header("GamepadSpeakerOutputType Setup")]
    PlayerInput pInput;
    [SerializeField] float controllerDeadZone = 0.1f;
    [SerializeField] float controllerRotateSmooth = 1000;

    public bool isDead;
    public int health;
    public float hitTime = 0;
    public bool isGamePad;

    private void Awake()
    {
        health = 5;
        _inputs = new TS_Inputs();
        cc = GetComponent<CharacterController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hitTime <= 0.001)
        {
            if (other.gameObject.tag == "Enemy")
            {
                hitTime = 0.5f;
                health--;
                if (health <= 0)
                {
                    isDead = true;
                }
            }
        }
        
        
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }
    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        speedyTimer -= Time.deltaTime;
        if (speedyTimer <= 0)
            moveSpeed = 4.0f;
        handleInputs();
        if (!isDead)
        {            
            handleMovement();
            handleRotation();
            if (hitTime > 0)
            {
                hitTime -= Time.deltaTime;
            }
        }        
    }
    public void OnDeviceChange(PlayerInput ctrl)
    {
        isGamePad = ctrl.currentControlScheme.Equals("Gamepad") ? true : false;
    }


    void handleInputs()
    {
        moveInput = _inputs.Player.Movement.ReadValue<Vector2>();
        aimInput = _inputs.Player.Aiming.ReadValue<Vector2>();
    }

    void handleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        cc.Move(move * moveSpeed * Time.deltaTime);
    }

    void handleRotation()
    {
        if (isGamePad)
        {
            if(Mathf.Abs(aimInput.x) > controllerDeadZone || Mathf.Abs(aimInput.y)  > controllerDeadZone)
            {
                Vector3 playerDirection = Vector3.right * aimInput.x
                    + Vector3.forward * aimInput.y;
                if(playerDirection.sqrMagnitude > 0)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        newRotation, controllerRotateSmooth * Time.deltaTime);
                }
            }
        }

        else
        {

            Ray ray = Camera.main.ScreenPointToRay(aimInput);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            float rayDistance; // from mouse position to our ground plane.

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, 
            transform.position.y, 
            lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }


    public void HealthPowerUp()
    {
        health += 3;
        if (health > 5)
            health = 5;        
    }

    public void SpeedPowerUp()
    {
        if (speedyTimer > 0)
        {
            speedyTimer += 15.0f;
        }
        else
        {
            speedyTimer = 15.0f;
            moveSpeed = 6.5f;
        }
    }
}
