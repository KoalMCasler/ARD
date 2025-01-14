using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ARPortal : MonoBehaviour
{
    [Header("Managers")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    [Header("Input Properties")]
    public bool portalSpawned;
    public GameObject uIObject;
    public TrackableType trackableType;
    public GameObject portalPrefab;
    public Text debugText;
    public GameObject mainCam;
    [Header("Interaction Properties")]
    public GameObject ghost;
    public Animator ghostAnim;
    public AudioSource ghostSound;

    // Start is called before the first frame update
    void Start()
    {
        uIObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(portalSpawned)
        {
            uIObject.SetActive(false);
        }
        this.transform.position = mainCam.transform.position;
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

    void OnTriggerEnter(Collider other)
    {
        ghost = GameObject.FindWithTag("Ghost");
        ghostAnim = ghost.GetComponent<Animator>();
        //debugText.text = other.name + " entered";
        if(other.name == "Cheese (1)")
        {
            TriggerAnimation("Hide");
        }
    }

    void OnTriggerExit(Collider other)
    {
        //debugText.text = other.name + " Exited";
        if(other.name == "Cheese (1)")
        {
            TriggerAnimation("Return");
        }
    }

    void TriggerAnimation(string anim)
    {
        if(anim == "Hide")
        {
            ghostAnim.SetTrigger("Hide");
            ghostSound.PlayOneShot(ghostSound.clip);
        }
        else if(anim == "Return")
        {
            ghostAnim.SetTrigger("Return");
        }
    }

}
