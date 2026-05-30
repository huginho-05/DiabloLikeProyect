using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")] 
        [SerializeField] private float movementSpeed;
        [SerializeField] private float movementSmoothFactor = 0.3f;
        [SerializeField] private float rotationSmoothFactor = 0.3f;

        private CharacterController controller;
        private Animator anim;
        private Camera cam;
        
        private Vector2 inputVector;
        private Vector3 verticalMovement;
        
        private float currentSpeed;
        private float speedVelocity;
        private float rotationVelocity;
        private bool isAiming;

        public PlayerInput PlayerInput { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            PlayerInput = GetComponent<PlayerInput>();
            anim = GetComponentInChildren<Animator>();
            cam = Camera.main;
        }

        private void OnEnable()
        {
            PlayerInput.actions["Move"].canceled += UpdateMovement;
            PlayerInput.actions["Move"].performed += UpdateMovement;
            PlayerInput.actions["Click"].performed += StartAiming;
            PlayerInput.actions["Click"].canceled += StopAiming;
        }

        private void UpdateMovement(InputAction.CallbackContext ctx)
        {
            inputVector = ctx.ReadValue<Vector2>();
        }

        void Update()
        {
            MoveAndRotate();
        }

        private void MoveAndRotate()
        {
            // Calcular velocidad objetivo (respeta magnitud del joystick)
            float targetSpeed = movementSpeed * inputVector.magnitude;
            
            // Suavizar velocidad actual
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, movementSmoothFactor);
            
            // Calcular y aplicar movimiento
            Vector3 movement = Vector3.zero;
            
            if (inputVector.magnitude > 0)
            {
                // Calcular ángulo de rotación
                float angle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                
                // Rotar suavemente
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref rotationVelocity, rotationSmoothFactor);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
                
                // Mover hacia adelante
                movement = transform.forward * currentSpeed;
            }
            
            // Aplicar movimiento
            controller.Move((movement + verticalMovement) * Time.deltaTime);
            
            // Actualizar animación
            anim.SetFloat("Speed", currentSpeed / movementSpeed);
        }
        
        private void StartAiming(InputAction.CallbackContext ctx)
        {
            isAiming = true;
            anim.SetBool("IsAiming", true);
        }

        private void StopAiming(InputAction.CallbackContext ctx)
        {
            isAiming = false;
            anim.SetBool("IsAiming", false);
        }
}
