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
            take = true;
        }

        public void StopTakeButton(InputAction.CallbackContext ctx)
        {
            take = false;
            piece.transform.parent = parentPiece;
            GameEchecs.Instance.ClearPossibleMove();
            GameEchecs.Instance.ApplyPlayerSimulate(currentP);
            piece = null;            
            currentP = null;
            currentPiceMove = null;
            parentPiece = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Piece" && take && piece == null)
            {
                piece = other.gameObject;
                parentPiece = piece.transform.parent;
                piece.transform.parent = transform;
                currentP = GameEchecs.Instance.DrawPossibleMove(piece,out currentPiceMove);
            }
        }
    }
}