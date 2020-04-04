using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;
    public Transform Obstruction;
    float zoomSpeed = 2f;

     void Start()
    {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CamControl();
        ViewObstructed();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY  = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(Target);

        /*the camera can be rotated indivudually by holding the shift key - I wanted this feature since
         I was using it a lot back in the days when I was playing DayZ Standalone :P and it's a nice thing to have
         */
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
        else
        { 
        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
    void ViewObstructed()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,Target.position - transform.position, out hit, 4.5f))
        {
            //checking if the game object blocking the camera is not the player
            if(hit.collider.gameObject.tag!="Player")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5)
                    {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
                    
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if(Vector3.Distance(transform.position, Target.position)<4.5f)
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }

        }
    }
}
