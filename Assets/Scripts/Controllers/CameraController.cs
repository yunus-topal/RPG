using System;
using UnityEngine;

namespace Controllers {
    public class CameraController : MonoBehaviour {
        [SerializeField] private GameObject _followTarget;
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _offsetRotation;

        private void Update() {
            if (_followTarget != null) {
                var cameraTransform = transform;
                cameraTransform.position = _followTarget.transform.position + _offsetPosition;
                cameraTransform.eulerAngles = _followTarget.transform.eulerAngles + _offsetRotation;
            }
        }
    }
}


