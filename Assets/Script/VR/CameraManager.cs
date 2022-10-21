using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Chess;

namespace ChessVR
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject cameraObject = null;
        void Update()
        {
            cameraObject.transform.position = InputManager.Vr.XRI_Head.Position.ReadValue<Vector3>();
            cameraObject.transform.rotation = InputManager.Vr.XRI_Head.Rotation.ReadValue<Quaternion>();
        }
    }
}
