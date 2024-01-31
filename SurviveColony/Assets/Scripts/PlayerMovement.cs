using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform aimTarget;

    [Header("Movement Properties")]
    private bool running;
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float jumpSpeed = 4;
    public float rotationSpeed;
    public float targetRotationSpeed;
    public float speedSmoothTime = .1f;
    private float currentSpeed;
    private float speedSmoothVelocity;

    [Space(10), Header("Stamina")]
    [SerializeField] private float stamina = 100f;
    public float staminaUsagePerTime = 40f;
    public float stamineUseLimit = 20f;
    public float stamineRefillDelay = .7f;
    private float stamineRefillElapsedTime = 0f;

    [Space(10), Header("Jump")]
    public float jumpHeight = 1;
    private float velocityY;
    [Range(0, 1)]
    public float airControlPercent;

    [Space(10), Header("EnvironmentalFactors")]
    public float gravity = -12;

    //Input
    private Vector2 rawInput;

    private PlayerAnimationController playerAnimationController;
    private CharacterController characterController;
    private Enemy targetEnemy;

    private void Awake()
    {
        playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        stamineRefillElapsedTime = stamineRefillDelay;
        GameplayUI.instance.StaminaSlider(stamina);
    }

    public void UpdateInput(Vector3 newInput)
    {
        rawInput = new Vector2(newInput.x, newInput.z);
    }

    void Update()
    {
        Vector2 inputDir = rawInput.normalized;

        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > stamineUseLimit)
            {
                Run();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        if (running)
        {
            stamina -= staminaUsagePerTime * Time.deltaTime;
            if (stamina <= 0)
            {
                stamineRefillElapsedTime = 0f;
                running = false;
            }
        }
        else
        {
            stamineRefillElapsedTime += Time.deltaTime;
            stamineRefillElapsedTime = Mathf.Clamp(stamineRefillElapsedTime, 0, stamineRefillDelay);
            if (stamineRefillElapsedTime >= stamineRefillDelay)
            {
                stamina += staminaUsagePerTime * Time.deltaTime;
            }
        }
        stamina = Mathf.Clamp(stamina, 0, 100);
        GameplayUI.instance.StaminaSlider(stamina);

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);

        Vector2 animationVelocity;
        Vector3 lookAtDirection;
        if (GameManager.instance.inputType == InputType.Mobile)
        {
            if (targetEnemy != null)
            {
                lookAtDirection = targetEnemy.transform.position - transform.position;
                Vector3 dott = new Vector3(rawInput.x, 0, rawInput.y);
                float dotForward = Vector3.Dot(transform.forward, dott);
                float dotRight = Vector3.Dot(transform.right, dott);
                animationVelocity = new Vector2(animationSpeedPercent * dotRight, animationSpeedPercent * dotForward);
            }
            else
            {
                lookAtDirection = new Vector3(rawInput.x, 0, rawInput.y);
                animationVelocity = new Vector2(0, rawInput.magnitude * animationSpeedPercent);
            }
        }
        else
        {
            lookAtDirection = (aimTarget.transform.position - transform.position).normalized;
            Vector3 dott = new Vector3(rawInput.x, 0, rawInput.y);
            float dotForward = Vector3.Dot(transform.forward, dott);
            float dotRight = Vector3.Dot(transform.right, dott);
            animationVelocity = new Vector2(animationSpeedPercent * dotRight, animationSpeedPercent * dotForward);
        }


        LookAtDirection(lookAtDirection);
        playerAnimationController.SetSpeed(animationVelocity, speedSmoothTime, Time.deltaTime);
    }

    private void Move(Vector2 inputDir, bool running)
    {
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        Vector3 velocityDirection = new Vector3(inputDir.x, 0, inputDir.y);
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = velocityDirection * currentSpeed + Vector3.up * velocityY;
        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
            playerAnimationController.Jump();
        }
    }
    public void Run()
    {
        if (stamina > stamineUseLimit)
        {
            running = true;
        }
    }
    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }

    public void LookAtDirection(Vector3 direction)
    {
        if (direction.magnitude == 0) return;
        var lookAtSpeed = targetEnemy == null ? targetRotationSpeed : rotationSpeed;
        var dir = direction;
        dir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), lookAtSpeed);
    }

    public void SetIndicate(Enemy newEnemy)
    {
        targetEnemy = newEnemy;
    }
    public void RunStart()
    {
        running = true;
    }
    public void RunEnd()
    {
        running = false;
    }
}