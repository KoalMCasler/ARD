using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;

public class ARPortal : MonoBehaviour
{
    [Header("Managers")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    [Header("Everything else")]
    public List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    public bool portalSpawned;
    public GameObject uIObject;
    public InputActionAsset inputs;
    public InputAction touchAction;
    public TrackableType trackableType;
    public GameObject portalPrefab;
    // Start is called before the first frame update
    void Start()
    {
        uIObject.SetActive(true);
        touchAction = inputs.FindAction("Touch", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(portalSpawned)
        {
            uIObject.SetActive(false);
        }
    }

    void OnTouch(InputValue touchValue)
    {
        if(!portalSpawned)
        {
            Vector2 touchLocation = touchValue.Get<Vector2>();
            if(raycastManager.Raycast(touchLocation,hitList,trackableType))
            {
                GameObject targetPlane = FindObjectOfType<ARPlane>().gameObject;
                Instantiate(portalPrefab,targetPlane.transform.position,targetPlane.transform.rotation);
                portalSpawned = true;
                planeManager.SetTrackablesActive(false);
                planeManager.enabled = false;
            }
        }
    }
}
