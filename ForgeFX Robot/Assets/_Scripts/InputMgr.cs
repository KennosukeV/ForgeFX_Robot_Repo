using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created on 12/10/21 by Ken Vernon.
// Description: This script defines and manages logic to allow an end-user to interact with real-time assets via mouse input.
// Updated on 12/10/21 as v0.1.0 by Ken Vernon.
// - Created Camera variable called 'cam' and initialized it in Start().
// - Created Ray variable called 'ray' and is initialized in Update().
// - Created RaycastHit variable called 'hit' and is initialized in Update().
// Updated on 12/11/21 as v0.2.0 by Ken Vernon.
// - Modified Update() to have unique logic if 'Keycode.Mouse0' is detected.
// - Created InteractiveController variable called 'interactScript' and is initialized in Update().
// - Created Vector3 variable called 'offset' and is initialized in Update().
// - Created Collider variable called 'curCollider' and is initialized in Update().
// - Created Vector3 variable called 'inputPos' and is initialized in Update().
// - Created float variable called 'distOffset' and initialized it at declaration.
// - Created Vector3 variable called 'snapPos' and is initialized in Update().
// - Created float variable called 'snapDist' and initialized it in Start().
// - Modified Update() to only perform raycast logic if 'Keycode.Mouse0' is not detected.
// - Modified Update() to call InteractiveController.SetStatus() when 'Keycode.Mouse0' is detected.
// Updated on 12/12/21 as v0.2.1 by Ken Vernon.
// - Modified Update() to call InteractiveController.Highlight().

public class InputMgr : MonoBehaviour
{
    [Tooltip("Camera in which the app is viewed through.  [Default = Main Camera]")]
    public Camera cam;
    [Tooltip("Controls pixel distance until dragged interactive object snaps back to its home position.")]
    public float snapDist;
    [Tooltip("If camera view is moveable during real time, set to 'true.'  If camera view is stationary, leave as 'false.'")]
    public bool motionCamera;

    private Ray ray;
    private RaycastHit hit;
    private InteractiveController interactScript;
    private Collider curCollider;
    private Vector3 inputPos, snapPos;
    private float distOffset = 0f;
    private bool snapping = false;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if(snapDist <= 0f)
        {
            snapDist = 10f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (interactScript != null)
            {
                if(distOffset == 0f)
                {
                    distOffset = Vector3.Distance(interactScript.transform.position, cam.transform.position);
                }

                inputPos = Input.mousePosition;
                inputPos.z = distOffset;

                if(motionCamera == false)
                {
                    if (interactScript.GetStatus() == true)
                    {
                        snapPos = interactScript.GetSnapPosition();
                    }
                }
                else
                {
                    snapPos = interactScript.GetSnapPosition();
                }

                if(Vector2.Distance(snapPos, inputPos) > snapDist)
                {
                    interactScript.SetPosition(cam.ScreenToWorldPoint(inputPos));

                    if(snapping == true)
                    {
                        snapping = false;
                    }
                }
                else
                {
                    interactScript.SetPosition(cam.ScreenToWorldPoint(snapPos));

                    if(snapping == false)
                    {
                        snapping = true;
                    }
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(interactScript != null)
            {
                interactScript.SetStatus(snapping);
            }
        }
        else
        {
            if(distOffset != 0f)
            {
                distOffset = 0f;
            }

            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.CompareTag("Interactive"))
                {
                    if (hit.collider != curCollider)
                    {
                        interactScript = hit.collider.attachedRigidbody.gameObject.GetComponent<InteractiveController>();
                        interactScript.Highlight(true);
                        return;
                    }
                }
            }
            else
            {
                if (interactScript != null)
                {
                    interactScript.Highlight(false);
                    interactScript = null;
                }
            }
        }
    }
}
