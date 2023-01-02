using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Echecs;

namespace ChessVR
{
    public class Hand : MonoBehaviour
    {
        private HandType type;
        private bool take = false;
        private GameObject piece = null;
        private Piece currentP;
        private Transform parentPiece = null;
        private Dictionary<Vector2Int,MoveType> currentPiceMove;
        private List<GameObject> listPiece = new List<GameObject>();

        void Start()
        {
            if(type == HandType.Left)
            {
                InputManager.Vr.XRI_HandLeft.TakeButton.started += StartTakeButton;
                InputManager.Vr.XRI_HandLeft.TakeButton.canceled += StopTakeButton;
            }
        }

        public void StartTakeButton(InputAction.CallbackContext ctx)
        {
            GameObject obj = null;
            float minValue = float.MaxValue;
            float calculeVal = 0.0f;
            for(int i = 0; i < listPiece.Count;i++)
            {
                calculeVal = Vector3.Distance(listPiece[i].transform.position,transform.position);
                if(calculeVal < minValue)
                {
                    minValue = calculeVal;  
                    obj = listPiece[i];       
                }
            }
            if(obj == null)
            {
                return;
            }

                piece = obj;
                currentP = GameEchecs.Instance.DrawPossibleMove(piece,out currentPiceMove);
                if(currentP != null)
                {
                    if(currentP.Team == Team.BLACK)
                    {
                        piece = null;
                        parentPiece = null;
                    }
                }
                parentPiece = piece.transform.parent;
                piece.transform.parent = transform;
        }

        public void StopTakeButton(InputAction.CallbackContext ctx)
        {
            piece.transform.parent = parentPiece;
            GameEchecs.Instance.ClearPossibleMove();
            GameEchecs.Instance.ApplyPlayerSimulate(currentP);
            piece = null;            
            currentP = null;
            currentPiceMove = null;
            parentPiece = null;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartTakeButton(default);
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                StopTakeButton(default);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Piece")
            {
                listPiece.Add(other.gameObject);
            }          
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.transform.tag == "Piece")
            {
                listPiece.Remove(other.gameObject);
            }          
        }
    }
}