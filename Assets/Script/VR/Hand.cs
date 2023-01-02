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
        private static List<Hand> listOfHand = new List<Hand>();

        void Start()
        {
            if(type == HandType.Left)
            {
                InputManager.Vr.XRI_HandLeft.TakeButton.started += StartTakeButton;
                InputManager.Vr.XRI_HandLeft.TakeButton.canceled += StopTakeButton;
            }
            listOfHand.Add(this);
        }

        public void StartTakeButton(InputAction.CallbackContext ctx)
        {
            StartTB();
        }

        public void StartTB()
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
           StopTB();
        }

        public void StopTB()
        {
            if(piece == null)
            {
                return;
            }
            piece.transform.parent = parentPiece;            
            GameEchecs.Instance.ApplyPlayerSimulate(currentP);
            GameEchecs.Instance.ClearPossibleMove();
            piece = null;            
            currentP = null;
            currentPiceMove = null;
            parentPiece = null;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartTB();
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                StopTB();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Piece")
            {
                foreach(Hand h in listOfHand)
                {
                    if(h != this)
                    {
                        if(h.listPiece.Contains(other.gameObject))
                        {
                            return;
                        }
                    }
                }
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