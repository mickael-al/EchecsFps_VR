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
        private float life = 100.0f;
        private bool swapGun = false;
        public void SetGun(bool state)
        {
            for(int i = 0; i < Hand.Length;i++)
            {
                Hand[i].GetComponent<Hand>().SwapGun = state;                
            }
            swapGun = state;
        }

        void Update()
        {
            cameraObject.transform.localPosition = InputManager.Vr.XRI_Head.Position.ReadValue<Vector3>();
            cameraObject.transform.localRotation = InputManager.Vr.XRI_Head.Rotation.ReadValue<Quaternion>();
            Hand[0].transform.localPosition = InputManager.Vr.XRI_HandLeft.Position.ReadValue<Vector3>();
            Hand[0].transform.localRotation = InputManager.Vr.XRI_HandLeft.Rotation.ReadValue<Quaternion>();
            Hand[1].transform.localPosition = InputManager.Vr.XRI_HandRight.Position.ReadValue<Vector3>();
            Hand[1].transform.localRotation = InputManager.Vr.XRI_HandRight.Rotation.ReadValue<Quaternion>();
            if(swapGun && Mathf.Abs(InputManager.Vr.XRI_HandRight.stick.ReadValue<Vector2>().y+InputManager.Vr.XRI_HandLeft.stick.ReadValue<Vector2>().y) < 0.8f)
            {
                transform.Rotate(Vector3.up * Mathf.Clamp(InputManager.Vr.XRI_HandRight.stick.ReadValue<Vector2>().x+InputManager.Vr.XRI_HandLeft.stick.ReadValue<Vector2>().x,-1.0f,1.0f) * Time.deltaTime * 55.0f);
            }
        }

        public void FpsSetup(float lifeP)
        {
            life = lifeP;
        }

        public void addDegat(float degat)
        {
            life -= degat;
        }
    }
    
}
