using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerInteraction : MonoBehaviour
{
    public PlayerInput playerInput;

    void Awake()
    {
        playerInput.actions["Interact"].performed += context => Interact(5f);
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public IInteractable Interact(float interactionRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionRadius, gameObject.transform.forward, 
            Mathf.Infinity, Gears.gears.interactionLayer);

        Dictionary<IInteractable, GameObject> interactable = new Dictionary<IInteractable, GameObject>();
        
        List<IInteractable> interact = new List<IInteractable>();
        
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out IInteractable interaction))
            {
                interactable.Add(interaction, hits[i].collider.gameObject);
                interact.Add(interaction);
            }
        }

        IInteractable interactionF = null;
        float distance = Mathf.Infinity;
        
        for (int i = 0; i < interactable.Count; i++)
        {
            if (Vector3.Distance(transform.position, interactable[interact[i]].transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, interactable[interact[i]].transform.position);
                interactionF = interact[i];
            }
        }
        
        interactionF?.Interact();

        return interactionF;
    }
}
