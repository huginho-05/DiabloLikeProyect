using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerNavMovementSystem : MonoBehaviour
{
    private PlayerInput input;
    private NavMeshAgent agent;
    private Camera cam;
    private Interactuable currentInteractuable;
    private Vector3 currentDestination;
    private Animator playerAnimator;
    
    private void Awake()
    {
        cam = Camera.main;
        input = GetComponent<PlayerInput>();
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable() //Al hacer click
    { 
        input.actions["Click"].started += OnClicked;
    }
    
    private void OnDisable() //Al dejar de hacer click
    {
        input.actions["Click"].started -= OnClicked;
    }

    private void OnClicked(InputAction.CallbackContext obj) //Se ejecuta al hacer click
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        
        //He clickado en un interactuable
        if (hit.transform.TryGetComponent(out Interactuable interactuable))
        {
            //Almaceno la informacion del interactuable clickado
            agent.stoppingDistance = interactuable.InteractionDistance;
            currentInteractuable = interactuable;
        }
        else
        {
            //He clickado en suelo
            currentInteractuable = null;
            agent.stoppingDistance = 0f;
            currentDestination = hit.point;
            agent.SetDestination(currentDestination);
        }
    }

    private void Update()
    {
        if (currentInteractuable != null)
        {
            //Este es mi nuevo destino
            currentDestination = currentInteractuable.transform.position;
            agent.SetDestination(currentDestination);

            if (ReachedDestination())
            {
                currentInteractuable.Interact(this.gameObject);
                currentInteractuable = null;
            }
        }
        
        playerAnimator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }

    private bool ReachedDestination()
    {
        //Devuelvo true si tengo un objetivo claro y mi distancia remanente está por debajo de distancia de parada
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
    
}
