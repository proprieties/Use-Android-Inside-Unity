using UnityEngine;
using System.Collections;

namespace PickInput
{
    public class PinchZoom : MonoBehaviour
    {
        public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
        public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
        Camera mainCamera;
        public LocalImage image;

        public Vector3 delta = Vector3.zero;
        private Vector3 lastPos = Vector3.zero;

        void Start()
        {
            mainCamera = GetComponent<Camera>();
        }

        void Update()
        {
            float deltaMagnitudeDiff = 0;

            // If there are two touches on the device...
            deltaMagnitudeDiff = Input.GetAxis("Mouse ScrollWheel");
            if (deltaMagnitudeDiff != 0f)
            {
                orthoZoomSpeed = 0.5f;
            }
            else if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * -1f;
                orthoZoomSpeed = 0.1f;
            }

            if (deltaMagnitudeDiff != 0)
            {
                image.Zoom(deltaMagnitudeDiff * orthoZoomSpeed);

                // If the camera is orthographic...
                if (mainCamera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    mainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 0.1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 0.1f, 179.9f);
                }

                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                lastPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                delta = Input.mousePosition - lastPos;
                image.Move(delta.x, delta.y);
                lastPos = Input.mousePosition;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Get movement of the finger since last frame
                delta = Input.GetTouch(0).deltaPosition * -1f;
                image.Move(delta.x, delta.y);
            }

            Debug.Log("delta X : " + delta.x);
            Debug.Log("delta Y : " + delta.y);
        }
    }
}