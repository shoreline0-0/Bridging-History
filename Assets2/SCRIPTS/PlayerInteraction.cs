using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float playerReach = 3f;

    [Header("UI Elements")]
    public GameObject interactionPanel; // Assign in Inspector

    [Header("Player Control Scripts")]
    public MonoBehaviour movementScript;    // Assign your movement script (e.g., PlayerMovement)
    public MonoBehaviour cameraLookScript;  // Assign your camera script (e.g., MouseLook)

    private Interactable currentInteractable;

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();

            if (interactionPanel != null)
            {
                interactionPanel.SetActive(true);

                // Show cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                // Disable movement and camera look
                if (movementScript != null) movementScript.enabled = false;
                if (cameraLookScript != null) cameraLookScript.enabled = false;
            }
        }

        // Auto-lock cursor and re-enable control if panel is closed
        if (interactionPanel != null && !interactionPanel.activeSelf && Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (movementScript != null) movementScript.enabled = true;
            if (cameraLookScript != null) cameraLookScript.enabled = true;
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();

        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }

    // 🔘 Called by UI Button to close the panel
    public void CloseInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);

            // Hide cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Re-enable movement and camera look
            if (movementScript != null) movementScript.enabled = true;
            if (cameraLookScript != null) cameraLookScript.enabled = true;
        }
    }
}
