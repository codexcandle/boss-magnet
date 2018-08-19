using UnityEngine;
using System.Collections;

namespace Codebycandle.NCSoftDemo
{
    public class MinimapCameraController:MonoBehaviour
    {
        public GameObject player;
        public float smoothing = 5f;

        private Vector3 offset;

        void Start()
        {
            offset = transform.position - player.transform.position;
        }
        
        // TODO - confirm which method to use below?
        /*
        void LateUpdate ()
        {
            transform.position = player.transform.position + offset;
        }
        */
        void FixedUpdate()
        {
            Vector3 targetCamPos = player.transform.position + offset;

			// just use x, z!
			targetCamPos = new Vector3(targetCamPos.x, transform.position.y, targetCamPos.z);


            // TODO - confirm "Time.deltaTime" is needed for "fixedUpdate?"
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}