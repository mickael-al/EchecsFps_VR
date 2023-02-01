using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Chess;

namespace ChessVR
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject cameraObject = null;
        [SerializeField] private CharacterController characterController = null;
        [SerializeField] private GameObject[] Hand = null;
        [SerializeField] private float size = 1.0f;

        public void SetGun(bool state)
        {
            for(int i = 0; i < Hand.Length;i++)
            {
                Hand[i].GetComponent<Hand>().SwapGun = state;
            }
        }

        void Update()
        {
            cameraObject.transform.localPosition = InputManager.Vr.XRI_Head.Position.ReadValue<Vector3>() * size;
            cameraObject.transform.rotation = InputManager.Vr.XRI_Head.Rotation.ReadValue<Quaternion>();
            Hand[0].transform.localPosition = InputManager.Vr.XRI_HandLeft.Position.ReadValue<Vector3>() * size;
            Hand[0].transform.rotation = InputManager.Vr.XRI_HandLeft.Rotation.ReadValue<Quaternion>();
            Hand[1].transform.localPosition = InputManager.Vr.XRI_HandRight.Position.ReadValue<Vector3>() * size;
            Hand[1].transform.rotation = InputManager.Vr.XRI_HandRight.Rotation.ReadValue<Quaternion>();
        }
    }
}
