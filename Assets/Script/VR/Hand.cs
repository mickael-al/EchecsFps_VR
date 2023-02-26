using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Echecs;

namespace ChessVR
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private HandType type;
        [SerializeField] private GameObject baseObject = null;
        [SerializeField] private GameObject Gun = null;
        [SerializeField] private GameObject PivotTP = null;
        [SerializeField] private GameObject projectile = null;
        [SerializeField] private LayerMask lm = default;
        private bool take = false;
        private GameObject piece = null;
        private Piece currentP;
        private Transform parentPiece = null;
        private Dictionary<Vector2Int,MoveType> currentPiceMove;
        private List<GameObject> listPiece = new List<GameObject>();
        private static List<Hand> listOfHand = new List<Hand>();
        private Vector2 stickValue;
        private RaycastHit hit;

        private bool takeState = false;
        private bool applyPlayerSimulate = false;
        private bool swapGun = false;
        private Vector3 lastDestination = Vector3.zero;
        private bool TpPoint = false;

        public bool SwapGun
        {
            get{return swapGun;}
            set{
                swapGun = value;
                Gun.SetActive(value);
                baseObject.SetActive(!value);
            }
        }
        void Start()
        {
            if(type == HandType.Left)
            {
                InputManager.Vr.XRI_HandLeft.TakeButton.started += StartTakeButton;
                InputManager.Vr.XRI_HandLeft.TakeButton.canceled += StopTakeButton;
                InputManager.Vr.XRI_HandLeft.Accept.performed += Accept;
            }
            else
            {
                InputManager.Vr.XRI_HandRight.TakeButton.started += StartTakeButton;
                InputManager.Vr.XRI_HandRight.TakeButton.canceled += StopTakeButton;
                InputManager.Vr.XRI_HandRight.Accept.performed += Accept;
            }
            listOfHand.Add(this);
        }

        public void Accept(InputAction.CallbackContext ctx)
        {
            GameEchecs.Instance.AcceptCommand();
        }

        public void StartTakeButton(InputAction.CallbackContext ctx)
        {
            if(swapGun)
            {
                Destroy(Instantiate(projectile,Gun.transform.GetChild(0).position,Gun.transform.GetChild(0).rotation),10.0f);
            }
            else
            {
                if(!takeState && !GameEchecs.Instance.EndOfGame)
                {
                    StartTB();
                    takeState = true;
                }
            }
        }

        public void StartTB()
        {
            GameObject obj = null;
            float minValue = float.MaxValue;
            float calculeVal = 0.0f;
            for(int i = 0; i < listPiece.Count;i++)
            {
                if(listPiece[i] != null)
                {
                    calculeVal = Vector3.Distance(listPiece[i].transform.position,transform.position);
                    if(calculeVal < minValue)
                    {
                        minValue = calculeVal;  
                        obj = listPiece[i];       
                    }
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
                        takeState = false;
                    }
                }
                if(piece != null)
                {
                    parentPiece = piece.transform.parent;
                    piece.transform.parent = transform;
                }
        }

        public void StopTakeButton(InputAction.CallbackContext ctx)
        {
            if(takeState)
            {
                StartCoroutine(StopTB());
            }
        }

        public void StopApplySimulate()
        {
            applyPlayerSimulate = false;
        }

        public IEnumerator StopTB()
        {
            if(piece == null)
            {
                takeState = false;
                yield break;
            }
            piece.transform.parent = parentPiece;  
            applyPlayerSimulate = true;          
            StartCoroutine(GameEchecs.Instance.ApplyPlayerSimulate(currentP,StopApplySimulate));
            while(applyPlayerSimulate)
            {
                yield return null;
            }
            GameEchecs.Instance.ClearPossibleMove();
            piece = null;            
            currentP = null;
            currentPiceMove = null;
            parentPiece = null;
            listPiece.Clear();
            takeState = false;
        }

        public void Update()
        {

            if(Input.GetKeyDown(KeyCode.Z))
            {                
                GameEchecs.Instance.AcceptCommand();
            }
            if(Input.GetKeyDown(KeyCode.Space) && !takeState)
            {                
                takeState = true;
                StartTB();
            }
            if(Input.GetKeyDown(KeyCode.A) && takeState)
            {
                StartCoroutine(StopTB());
            }
            if(type == HandType.Left)
            {
                stickValue = InputManager.Vr.XRI_HandLeft.stick.ReadValue<Vector2>();
            }
            else
            {
                stickValue = InputManager.Vr.XRI_HandRight.stick.ReadValue<Vector2>();                
            }
            if((stickValue.y < -0.9f || Input.GetKey(KeyCode.Q)) && swapGun)
            {
                if(Physics.Raycast(PivotTP.transform.position,PivotTP.transform.TransformDirection(Vector3.forward),out hit,Mathf.Infinity,lm))
                {
                    PivotTP.transform.GetChild(0).localScale = new Vector3(0.01f,hit.distance,0.01f);
                    PivotTP.transform.GetChild(0).localPosition = new Vector3(0.0f,0.0f,hit.distance);
                    lastDestination = hit.point;
                    TpPoint = true;
                }
            }
            else if(stickValue.y > -0.1f)
            {
                PivotTP.transform.GetChild(0).localScale = new Vector3(0.01f,0.0f,0.01f);
                PivotTP.transform.GetChild(0).localPosition = new Vector3(0.0f,0.0f,0.0f);
                if(TpPoint)
                {
                    TpPoint = false;
                    GameObject.FindGameObjectWithTag("Player").transform.position = lastDestination;
                }
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