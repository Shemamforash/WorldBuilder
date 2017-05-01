using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MaxDistance = 3f;
    public float MinDistance = 1.3f;
    public float ZoomSpeed = 0.1f, PanSpeed = 8f;
    private float _currentZoom;

    private enum ActiveButton
    {
        RIGHT,
        LEFT,
        MIDDLE,
        NONE
    };

    private ActiveButton _currentButton = ActiveButton.NONE;
    private Vector2 _lastMousePosition;
    private bool _hasLastMousePosition = false;

    // Use this for initialization
    void Start()
    {
        _currentZoom = MaxDistance;
    }

    //Handle the mouse states
    private void GetButtonPress()
    {
        //If no button has been pressed, allow a new state to be activated
        if (_currentButton == ActiveButton.NONE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventController.GeneratePerlin();
                _currentButton = ActiveButton.LEFT;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _currentButton = ActiveButton.RIGHT;
            }
            else if (Input.GetMouseButtonDown(2))
            {
                _currentButton = ActiveButton.MIDDLE;
            }
        }
        else if ((_currentButton == ActiveButton.LEFT && Input.GetMouseButtonUp(0)) ||
                 _currentButton == ActiveButton.RIGHT && Input.GetMouseButtonUp(1) ||
                 _currentButton == ActiveButton.MIDDLE && Input.GetMouseButtonUp(2))
        {
            _currentButton = ActiveButton.NONE;
        }
    }

    private void UpdateRotation()
    {
        if (_currentButton == ActiveButton.MIDDLE)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            if (_hasLastMousePosition)
            {
                Vector2 mouseDifference = _lastMousePosition - currentMousePosition;
                float xDistance = mouseDifference.x / Screen.width;
                float yDistance = mouseDifference.y / Screen.width;
                Vector3 newCameraPosition = transform.localPosition;
                Vector3 xTransform = transform.right * xDistance * PanSpeed;
                newCameraPosition += xTransform;
                Vector3 yTransform = transform.up * yDistance * PanSpeed;
                newCameraPosition += yTransform;
                newCameraPosition.Normalize();
                newCameraPosition *= _currentZoom;
                if (newCameraPosition.x < 0.1f && newCameraPosition.x > 0f)
                {
                    newCameraPosition.x = 0.1f;
                }
                else if (newCameraPosition.x > -0.1f && newCameraPosition.x < 0f)
                {
                    newCameraPosition.x = -0.1f;
                }
                transform.localPosition = newCameraPosition;
                transform.LookAt(transform.parent.position);
            }
            _lastMousePosition = currentMousePosition;
            _hasLastMousePosition = true;
        }
        else
        {
            _hasLastMousePosition = false;
        }
    }

    private void UpdateZoom()
    {
        if (_currentButton == ActiveButton.NONE)
        {
            float zoomAmount = -Input.mouseScrollDelta.y * ZoomSpeed;
            if (zoomAmount != 0)
            {
                _currentZoom += zoomAmount;
                if (_currentZoom < MinDistance)
                {
                    _currentZoom = MinDistance;
                }
                else if (_currentZoom > MaxDistance)
                {
                    _currentZoom = MaxDistance;
                }
                Vector3 newCameraPosition = transform.localPosition;
                newCameraPosition.Normalize();
                newCameraPosition *= _currentZoom;
                transform.localPosition = newCameraPosition;
            }
        }
    }

    void Update()
    {
        GetButtonPress();
        UpdateRotation();
        UpdateZoom();
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity))
        {
            if (_currentButton == ActiveButton.LEFT)
            {

                GameObject collidedObject = hit.transform.gameObject;
                if (collidedObject.tag == "TileObject")
                {
                    EventController.UseTool(collidedObject.GetComponent<TileObject>());
                }
            }
            else
            {
                EventController.ResetSelectedTile();
            }
        }

    }
}