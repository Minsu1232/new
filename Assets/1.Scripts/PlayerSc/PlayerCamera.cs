using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public class PlayerCamera : MonoBehaviour
    {
        
        public Player player;
        public Camera cameraObj;
        

        Vector3 cameraVelocity;
    float cameraSmoothSpeed = 1f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void CameraAction()
        {if(player != null)
            {

            }

        }
       public void FollowTarget()
        {
            Vector3 targetCameraPositon = Vector3.SmoothDamp(transform.position,player.transform.position,ref cameraVelocity,cameraSmoothSpeed*Time.deltaTime);
            transform.position = targetCameraPositon;
        }
    }

