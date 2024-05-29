using QCHT.Interactions.Core;
using QCHT.Interactions.Hands;
using Qualcomm.Snapdragon.Spaces.Samples;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using static QCHT.Interactions.Core.XRHandTrackingSubsystem;


public class Test : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor rayInteractor;
    [SerializeField]
    private InputActionReference selectAction;
    [SerializeField]
    private LayerMask selectableLayers;
    [SerializeField]
    private TextMeshProUGUI debugText;
    [SerializeField]
    private InputActionReference toggleAnchors;
    [SerializeField]
    private AnchorSampleController anchorManager;

    private GameObject currentObject;
    private int currentLayer;
    private Quaternion initObjectRotation;
    private Quaternion initHandRotation;

    private int debugCount = 0;

    private void Start()
    {
        selectAction.action.Enable();
        toggleAnchors.action.Enable();
        selectAction.action.started += onPressed;
        selectAction.action.canceled += onReleased;
        toggleAnchors.action.started += onTriggered;
    }
    
    private void Update()
    {
        if (currentObject == null)
            return;
        var handSubsystem = XRHandTrackingSubsystem.GetSubsystemInManager();
        currentObject.transform.position = rayInteractor.rayOriginTransform.position;
        currentObject.transform.rotation = (handSubsystem.RightHand.Root.rotation * Quaternion.Inverse(initHandRotation)) * initObjectRotation;
    }

    public void PlacePrefab(GameObject prefab)
    {
        selectObject(Instantiate(prefab));
    }

    private void onPressed(InputAction.CallbackContext context)
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        if (hit.transform?.gameObject is GameObject obj && selectableLayers == (selectableLayers | (1 << obj.layer)))
            selectObject(obj);
    }

    private void onReleased(InputAction.CallbackContext context)
    {
        if (currentObject != null)
            deselectObject();
    }

    private void onTriggered(InputAction.CallbackContext context)
    {
        if (anchorManager.enabled)
        {
            anchorManager.enabled = false;
        }
        else
        {
            anchorManager.enabled = true;
        }
    }

    private void selectObject(GameObject obj)
    {
        currentObject = obj;
        initObjectRotation = obj.transform.rotation;
        var handSubsystem = XRHandTrackingSubsystem.GetSubsystemInManager();
        initHandRotation = handSubsystem.RightHand.Root.rotation;
        currentLayer = currentObject.layer;
        

        foreach (var collider in currentObject.GetComponentsInChildren<Collider>())
            collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        foreach (var rb in currentObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = false;
        }
    }

    private void deselectObject()
    {
        // debugCount++;
        // debugText.text = "DESELECT" + debugCount;

        foreach (var collider in currentObject.GetComponentsInChildren<Collider>())
            collider.gameObject.layer = currentLayer;

        foreach (var rb in currentObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
        }

        currentObject = null;
    }
}
