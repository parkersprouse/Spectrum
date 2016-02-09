/*
 //* Camera Follow Simple
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public PlayerController target;
    Vector3 velocity = Vector3.zero;

    bool cameraMoving;

    void LateUpdate() {
        if (!cameraMoving) {
            transform.position = new Vector3(target.transform.position.x, 1 + (target.transform.position.y * .1f), -10);
        }
        else {
            Vector3 targetPosition = new Vector3(target.transform.position.x, 1 + (target.transform.position.y * .1f), -10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.2f);

            if (Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f) {
                cameraMoving = false;
                Debug.Log("done");
            }
        }
    }

    public void ChangeTarget(PlayerController target) {
        this.target = target;
        cameraMoving = true;
    }

}
*/


using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public PlayerController target;
    public Vector2 focusAreaSize;
    public float verticalOffset, lookAheadDistanceX, lookSmoothTimeX, verticalSmoothTime;

    FocusArea focusArea;
    float currentLookAheadX, targetLookAheadX, lookAheadDirectionX, smoothLookVelocityX, smoothVelocityY;
    bool lookAheadStopped;

    bool cameraMoving = false;
    Vector3 velocity = Vector3.zero;

    struct FocusArea {
        public Vector2 center, velocity;
        float left, right, top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = Vector2.zero;
        }

        public void Update(Bounds targetBounds) {
            float shiftX = 0;
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    void Start() {
        focusArea = new FocusArea(target.boxCollider.bounds, focusAreaSize);
    }

    void LateUpdate() {
        focusArea.Update(target.boxCollider.bounds);
        Vector2 focusPos = focusArea.center + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0) {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);

            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
                targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
                lookAheadStopped = false;
            }
            else {
                if (!lookAheadStopped) {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4f;
                    lookAheadStopped = true;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPos.y = Mathf.SmoothDamp(transform.position.y, focusPos.y, ref smoothVelocityY, verticalSmoothTime);
        focusPos += Vector2.right * currentLookAheadX;


        //if (!cameraMoving) {
            transform.position = (Vector3)focusPos + Vector3.forward * -10;
        //}
        /*else {
            Vector3 targetPosition = new Vector3(focusPos.x, focusPos.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.1f);

            if (Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f) {
                cameraMoving = false;
                Debug.Log("done");
            }
        }*/
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    public void ChangeTarget(PlayerController target) {
        this.target = target;
        cameraMoving = true;
    }
}
