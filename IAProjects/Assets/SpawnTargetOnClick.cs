using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTargetOnClick : MonoBehaviour
{
    public Camera cam;
   public GameObject movingObject; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
                    movingObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;       // we want 2m away from the camera position
            movingObject.transform.position = cam.ScreenToWorldPoint(mousePos);
        }
    }

}
