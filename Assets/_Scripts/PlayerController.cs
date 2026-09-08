using UnityEngine;

public class PlayerController : MonoBehaviour
{
    TS_Inputs _inputs;
    CharacterController cc;

    [Header("Player Controls")]
    public float moveSpeed = 4f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    public bool isDead;

    private void Awake()
    {
        _inputs = new TS_Inputs();
        cc = GetComponent<CharacterController>();
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
        handleInputs();
        if (!isDead)
        {
            handleMovement();
            handleRotation();
        }        
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
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        float rayDistance; // from mouse position to our ground plane.

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            LookAt(point);
        }
    }

    void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, 
            transform.position.y, 
            lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
