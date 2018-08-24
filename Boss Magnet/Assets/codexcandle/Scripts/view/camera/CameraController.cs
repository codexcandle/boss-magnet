using UnityEngine;
using System.Collections;

namespace Codebycandle.BossMagnet
{
    public class CameraController:MonoBehaviour
    {
        [SerializeField]
        private GameObject playerRoot;
        private GameObject player;

        public float smoothing = 1f;

        private Vector3 offset;

        private bool moving;
        private bool focused;

        private GameObject focusTarget;

        private Quaternion originalRotation;

        public void ZoomTo(Transform target)
        {
            StopFocus();
            
            if(!moving)
            {
                StopAllCoroutines();

                StartCoroutine(MoveObject(transform, 
                                            transform.position, 
                                            target.transform.position, 
                                            transform.rotation, 
                                            target.transform.rotation, 
                                            4.0F));
            }
        }

        public void FocusOn(GameObject target, float zoomDuration = 4.0F)
        {
            if(moving) 
                return;

            focusTarget = target;

            // offset = transform.position - target.transform.position;

            StopAllCoroutines();

            StartCoroutine(MoveObject(transform, 
                                        transform.position, 
                                        target.transform.position + offset, 
                                        transform.rotation, 
                                        originalRotation, 
                                        zoomDuration));

            focused = true;
        }

        public void StopFocus()
        {
            focused = false;
        }

        void Start()
        {
            offset = transform.position - playerRoot.transform.position;

            originalRotation = transform.rotation;

            player = GameObject.FindWithTag(GameTag.TAG_PLAYER);
        }

        void FixedUpdate()
        {
            if(focused && !moving)
            {
                Vector3 targetPos = focusTarget.transform.position + offset;

                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }

        IEnumerator MoveObject(Transform thisTransform,
                                Vector3 startPos,
                                Vector3 endPos,
                                Quaternion startRot,
                                Quaternion endRot,
                                float time)
        {
            moving = true;

            float i = 0;
            float rate = 1 / time;
            while (i < 1)
            {
                i += Time.deltaTime * rate;

                thisTransform.position = Vector3.Lerp(startPos, endPos, i);
                thisTransform.rotation = Quaternion.Slerp(startRot, endRot, i);

                yield return 0;
            }

            moving = false;
        }
    }
}