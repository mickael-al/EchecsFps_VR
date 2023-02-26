using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ChessVR;

namespace Echecs
{
    public class GameEchecs : MonoBehaviour
    {
        [SerializeField] private Transform boardObject = null;
        [SerializeField] private GameObject prefabPossibleMove = null;
        [SerializeField] private GameObject prefabEnnemi = null;
        [SerializeField] private GameObject mapObject = null;
        [SerializeField] private GameObject playerSpawnPoint = null;
        [SerializeField] private GameObject ennemieSpawnPoint = null;
        [SerializeField] private GameObject playerBaseChessPoint = null;
        [SerializeField] private GameObject botSpawnPoint = null;
        [SerializeField] private GameObject tipsText = null;
        [SerializeField] private List<Light> pointsLight = null;
        [SerializeField] private List<GameObject> winGameObjectText;
        [SerializeField] private AudioSource audioAccept = null;
        [SerializeField] private AudioSource audioChess = null;
        private static GameEchecs instance = null;
        private GameState gameState = new GameState();
        private Dictionary<PieceType,GameObject> prefabObjectPiece = new Dictionary<PieceType, GameObject>();
        private Dictionary<Team,Material> teamsMaterial = new Dictionary<Team,Material>();
        private Team botTeam = Team.BLACK;
        private List<GameObject> possibleMove = new List<GameObject>();

        private bool movePiece = false;
        private bool inFight = true;

        #region Getter Setter

        public static GameEchecs Instance { get{ return instance;}}
        public Dictionary<PieceType,GameObject> PrefabObjectPiece { get{ return prefabObjectPiece;}}
        public Dictionary<Team,Material> TeamsMaterial { get{ return teamsMaterial;}}
        public Transform BoardObject { get{ return boardObject;}}
        public GameObject pieceObjEnemy = null;
        private bool PlayerWinFpsGame = false;
        private bool inMove = false;
        private bool inMoveCor = false;

        public bool EndOfGame {get {return gameState.endGame;}}

        public void EndFpsGame(bool playerDead)
        {
            PlayerWinFpsGame = !playerDead;
            ScaleMapFight(false,null,null);
        }

        public void ScaleMapFight(bool value,Piece eobj,Piece pobj)
        {                
                GameObject playerFindWithTag = GameObject.FindGameObjectWithTag("Player");
                if(value)
                {
                    mapObject.transform.localScale = new Vector3(6,6,6);
                    for(int i = 0; i < pointsLight.Count;i++)
                    {
                        pointsLight[i].intensity = 10*8;
                        pointsLight[i].range = 30*8;
                    }
                    playerFindWithTag.transform.position = playerSpawnPoint.transform.position; 
                    playerFindWithTag.transform.eulerAngles = playerSpawnPoint.transform.eulerAngles; 
                    pieceObjEnemy = Instantiate(prefabEnnemi,ennemieSpawnPoint.transform.position,Quaternion.identity);
                    GameObject obj = Instantiate(eobj.Obj);
                    Ennemis en = pieceObjEnemy.transform.GetChild(0).GetComponent<Ennemis>();
                    en.Setup(playerFindWithTag,eobj.fps_SpeedStat,eobj.fps_rateProjectileStat,eobj.fps_LifeStat,EndFpsGame);
                    playerFindWithTag.GetComponent<Player>().FpsSetup(pobj.fps_LifeStat,EndFpsGame);                    
                    obj.transform.parent = en.transform;                    
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one*15;
                }
                else
                {
                    mapObject.transform.localScale = Vector3.one;
                    for(int i = 0; i < pointsLight.Count;i++)
                    {
                        pointsLight[i].intensity = 10;
                        pointsLight[i].range = 30;                        
                    }
                    playerFindWithTag.transform.position = playerBaseChessPoint.transform.position; 
                    playerFindWithTag.transform.eulerAngles = playerBaseChessPoint.transform.eulerAngles; 
                    Destroy(pieceObjEnemy);                        
                }
                playerFindWithTag.GetComponent<Player>().SetGun(value);
                playerFindWithTag.GetComponent<CharacterController>().enabled = value;
                inFight = value;
            
        }

        #endregion

        public static Vector3 PosToBoard(Vector2Int pos)
        {
            return new Vector3(0.075f*pos.x,0.0f,0.075f*pos.y);
        }
    
        public void Awake()
        {                       
            instance = this;
            prefabObjectPiece[PieceType.PAWN] = Resources.Load("pion") as GameObject;
            prefabObjectPiece[PieceType.BISHOP] = Resources.Load("fou") as GameObject;
            prefabObjectPiece[PieceType.KING] = Resources.Load("king") as GameObject;
            prefabObjectPiece[PieceType.QUEEN] = Resources.Load("queen") as GameObject;
            prefabObjectPiece[PieceType.KNIGHT] = Resources.Load("knight") as GameObject;
            prefabObjectPiece[PieceType.ROOK] = Resources.Load("tour") as GameObject;

            teamsMaterial[Team.BLACK] = Resources.Load("n") as Material;
            teamsMaterial[Team.WHITE] = Resources.Load("b") as Material;
            InitGameState(gameState);
        }      

        public void InitGameState(GameState gs)
        {
            Clear(gs);
            List<Piece> w = new List<Piece>();
            List<Piece> b = new List<Piece>();

            gs.field[0,6] = new Pawn(Team.BLACK,new Vector2Int(0,6),this);
            gs.field[1,6] = new Pawn(Team.BLACK,new Vector2Int(1,6),this);
            gs.field[2,6] = new Pawn(Team.BLACK,new Vector2Int(2,6),this);
            gs.field[3,6] = new Pawn(Team.BLACK,new Vector2Int(3,6),this);
            gs.field[4,6] = new Pawn(Team.BLACK,new Vector2Int(4,6),this);
            gs.field[5,6] = new Pawn(Team.BLACK,new Vector2Int(5,6),this);
            gs.field[6,6] = new Pawn(Team.BLACK,new Vector2Int(6,6),this);
            gs.field[7,6] = new Pawn(Team.BLACK,new Vector2Int(7,6),this);

            gs.field[0,1] = new Pawn(Team.WHITE,new Vector2Int(0,1),this);
            gs.field[1,1] = new Pawn(Team.WHITE,new Vector2Int(1,1),this);
            gs.field[2,1] = new Pawn(Team.WHITE,new Vector2Int(2,1),this);
            gs.field[3,1] = new Pawn(Team.WHITE,new Vector2Int(3,1),this);
            gs.field[4,1] = new Pawn(Team.WHITE,new Vector2Int(4,1),this);
            gs.field[5,1] = new Pawn(Team.WHITE,new Vector2Int(5,1),this);
            gs.field[6,1] = new Pawn(Team.WHITE,new Vector2Int(6,1),this);
            gs.field[7,1] = new Pawn(Team.WHITE,new Vector2Int(7,1),this);

            gs.field[0,7] = new Rook(Team.BLACK,new Vector2Int(0,7),this);
            gs.field[7,7] = new Rook(Team.BLACK,new Vector2Int(7,7),this);

            gs.field[0,0] = new Rook(Team.WHITE,new Vector2Int(0,0),this);
            gs.field[7,0] = new Rook(Team.WHITE,new Vector2Int(7,0),this);

            gs.field[1,7] = new Knight(Team.BLACK,new Vector2Int(1,7),this);
            gs.field[6,7] = new Knight(Team.BLACK,new Vector2Int(6,7),this);

            gs.field[1,0] = new Knight(Team.WHITE,new Vector2Int(1,0),this);
            gs.field[6,0] = new Knight(Team.WHITE,new Vector2Int(6,0),this);

            gs.field[2,7] = new Bishop(Team.BLACK,new Vector2Int(2,7),this);
            gs.field[5,7] = new Bishop(Team.BLACK,new Vector2Int(5,7),this);

            gs.field[2,0] = new Bishop(Team.WHITE,new Vector2Int(2,0),this);
            gs.field[5,0] = new Bishop(Team.WHITE,new Vector2Int(5,0),this);

            gs.field[3,7] = new King(Team.BLACK,new Vector2Int(3,7),this);
            gs.field[3,0] = new King(Team.WHITE,new Vector2Int(3,0),this);

            gs.field[4,7] = new Queen(Team.BLACK,new Vector2Int(4,7),this);
            gs.field[4,0] = new Queen(Team.WHITE,new Vector2Int(4,0),this);

            calculeAllMoves(gs);
        }  


        public void calculeAllMoves(GameState gs)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.field[i,j] != null)
                    {
                        gs.field[i,j].calculePossibleMoves(gs.field, true);
                    }
                }
            }
        }

        private Move RandomMove(GameState gs)
        {
            List<Piece> canmove = new List<Piece>();
            for(int i = 0; i < 8;i++)
            {
                for(int j = 0; j < 8;j++)
                {
                    if(gs.field[i,j] != null && gs.field[i,j].Team == gs.turn && gs.field[i,j].PossibleMoves.Count > 0)
                    {
                        canmove.Add(gs.field[i,j]);
                    }
                }
            }
            if(canmove.Count > 0)
            {
                Piece rdp = canmove[UnityEngine.Random.Range(0,canmove.Count)];
                int v = UnityEngine.Random.Range(0,rdp.PossibleMoves.Keys.Count);
                return new Move(gs.turn,rdp,rdp.PossibleMoves.Keys.ElementAt(v),rdp.PossibleMoves.Values.ElementAt(v));
            }
            return null;
        }
        private float updateSimulate = 0.0f;
        private void Update()
        {
            updateSimulate += Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Simulate(gameState);
                updateSimulate = 0.0f;
            }            
        }

        public Piece DrawPossibleMove(GameObject obj,out Dictionary<Vector2Int,MoveType> listMove)
        {
            ClearPossibleMove();
            Piece p = null;
            listMove = new Dictionary<Vector2Int,MoveType>();
            for(int i = 0; i < 8 && p == null; i++)
            {
                for(int j = 0; j < 8 && p == null; j++)
                {                    
                    if(gameState.field[i,j] != null && gameState.field[i,j].Obj == obj)
                    {
                        p = gameState.field[i,j];
                    }
                }
            }
            if(p != null)
            {
                DrawPossibleMove(p);
                listMove = p.PossibleMoves; 
                return p;
            }            
            return null;
        }

        public void DrawPossibleMove(Piece p)
        {
            int v = 0;
            foreach(KeyValuePair<Vector2Int,MoveType> m in p.PossibleMoves)
            {
                if(possibleMove.Count > v)
                {
                    possibleMove[v].transform.position = boardObject.position + PosToBoard(m.Key);
                    possibleMove[v].SetActive(true);
                }
                else
                {
                    GameObject obj = Instantiate(prefabPossibleMove,Vector3.zero,Quaternion.Euler(90,0,0),boardObject);
                    possibleMove.Add(obj);
                    possibleMove[v].transform.position = boardObject.position + PosToBoard(m.Key);
                }
                v++;
            }
        }

        public void ClearPossibleMove()
        {
            foreach(GameObject go in possibleMove)
            {
                go.SetActive(false);
            }
        }

        /*public void Simulate(GameState gs)
        {
            if(gs.endGame)
            {
                return;
            }
            if(gs.move != null)
            {
                move(gs.move,gs);
            }
            gs.move=null;
            gs.move = RandomMove(gs);
            if(gs.move != null)
            {
                if(!gs.simulation)
                {
                    ClearPossibleMove();
                    DrawPossibleMove(gs.move.targetPiece);
                }
            }
        }*/

        public IEnumerator ApplyPlayerSimulate(Piece p,Action applyPlayerSimulate)
        {
            if(gameState.endGame || p == null || movePiece)
            {
                applyPlayerSimulate?.Invoke();
                yield break;
            }            
            float minDistance = 0.05f;
            float valCalc = 0.0f;
            int v = -1;

            for(int i = 0; i < p.PossibleMoves.Count;i++)            
            {
                valCalc = Vector3.Distance((boardObject.position + PosToBoard(p.PossibleMoves.Keys.ElementAt(i))),p.Obj.transform.position);
                if(valCalc < minDistance)
                {
                    minDistance = valCalc;
                    v = i;
                }
            }
            if(v == -1)
            {
                p.Pos = p.Pos;
                applyPlayerSimulate?.Invoke();
                yield break;
            }
            gameState.move = new Move(gameState.turn,p,p.PossibleMoves.Keys.ElementAt(v),p.PossibleMoves.Values.ElementAt(v));
            if(gameState.move != null)
            {
                inMoveCor = true;
                StartCoroutine(move(gameState.move,gameState));
                audioChess.Play();
                while(inMove)
                {
                    yield return null;
                }
                gameState.move = null;
                gameState.move = RandomMove(gameState);
                StartCoroutine(pieceMoveEffect(gameState.move.targetPiece.Obj , boardObject.position + PosToBoard(gameState.move.targetPiece.Pos), boardObject.position + PosToBoard(gameState.move.move)));

            }         
            yield return null;  
            applyPlayerSimulate?.Invoke();
        }

        IEnumerator pieceMoveEffect(GameObject obj , Vector3 src, Vector3 dst)
        {
            movePiece = true;            
            float time = 0.0f;
            while(time < 1.0f)
            {
                time += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(src,dst,time);
                yield return null;
            }
            yield return null;      
            inMoveCor = true;                  
            StartCoroutine(move(gameState.move,gameState));
            while(inMoveCor)
            {
                yield return null;
            }
            movePiece = false;
        }

        public void Clear(GameState gs)
        {
            for(int i = 0; i < 8;i++)
            {
                for(int j = 0; j < 8;j++)
                {
                    gs.field[i,j] = null;
                }
            }
        }

        private IEnumerator move(Move move,GameState gs)
        {
            if (gs.checkEnPassant)
            {
                disableEnPassant(gs);
            }
            else
            {
                gs.checkEnPassant = true;
            }
            //Debug.Log(move.move+" "+move.moveType+" "+move.targetPiece);

            switch (move.moveType)
            {
                case MoveType.NORMAL:
                    inMove = true;
                    StartCoroutine(normal(move.targetPiece.Pos, move.move,gs));
                    break;
                case MoveType.CASTLE:
                    castles(move.targetPiece.Pos,  move.move,gs);
                    break;
                case MoveType.ENPASSANT:
                    enPassant(move.targetPiece.Pos, move.move,gs);
                    break;
                case MoveType.NEWPIECE:
                    exchange(move.targetPiece.Pos,  move.move,gs);
                    break;
                default:
                    break;
            }

            while(inMove)
            {
                yield return null;
            }
                            GameObject playerFindWithTag = GameObject.FindGameObjectWithTag("Player");
                playerFindWithTag.transform.position = playerBaseChessPoint.transform.position; 
                playerFindWithTag.transform.eulerAngles = playerBaseChessPoint.transform.eulerAngles; 
            WinOrLoose(gs);
            yield return null;
            inMoveCor = false;
        }

        private void WinOrLoose(GameState gs)
        {
            bool lost = true;
            King king = King.GetKingByTeam(gs.turn == Team.BLACK ? Team.WHITE : Team.BLACK);

            king.setCheck(gs.field,King.GetKingByTeam(Team.WHITE).Pos);
            bool moveOnlyKing = true;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.field[i,j] != null)
                    {
                        if (gs.field[i,j].Team != gs.turn)
                        {
                            gs.field[i,j].calculePossibleMoves(gs.field, true);
                            if (gs.field[i,j].PossibleMoves.Count > 0)
                            {
                                lost = false;
                                if(gs.field[i,j].Type != PieceType.KING)
                                {
                                    moveOnlyKing = false;
                                }
                            }
                        }
                    }
                }
            }

            if (king.Check && lost)
            {
                Debug.Log(gs.turn == Team.BLACK ? "Black wins" : "White wins");    
                winGameObjectText[gs.turn == Team.BLACK ? 0 : 1].SetActive(true);            
                gs.endGame = true;
            }
            else if (lost)
            {
                Debug.Log("Equal"+(gs.turn == Team.BLACK ? "B" : "W")); 
                winGameObjectText[2].SetActive(true);
                gs.endGame = true;
            }
            if(moveOnlyKing)
            {
                king.TurnMoveOnly++;
                if(king.TurnMoveOnly > 15)
                {
                    Debug.Log("TurnMoveOnly");
                    Debug.Log(gs.turn == Team.BLACK ? "Black wins" : "White wins");
                    winGameObjectText[gs.turn == Team.BLACK ? 0 : 1].SetActive(true);
                    gs.endGame = true;
                }
            }
            else
            {                
                king.TurnMoveOnly = 0;
            }
            gs.turn = gs.turn == Team.WHITE ? Team.BLACK : Team.WHITE;
        }

        public void AcceptCommand()
        {
            if(tipsText.activeSelf)
            {
                tipsText.SetActive(false);
                audioAccept.Play();
            }
            if(gameState.endGame)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (gameState.field[i,j] != null)
                        {
                            Destroy(gameState.field[i,j].Obj);
                        }
                    }
                }
                tipsText.SetActive(false);
                winGameObjectText[0].SetActive(false);
                winGameObjectText[1].SetActive(false);
                winGameObjectText[2].SetActive(false);
                possibleMove = new List<GameObject>();
                movePiece = false;
                inFight = true;
                King.ClearKing();
                gameState = new GameState();
                InitGameState(gameState);
                audioAccept.Play();
            }
        }


        private IEnumerator normal(Vector2Int start, Vector2Int end,GameState gs)
        {            
            bool MovePlayerTeam = gs.field[start.x,start.y].Team == Team.WHITE;
            if(gs.field[end.x,end.y] != null)
            {
                tipsText.SetActive(true);
                while(tipsText.activeSelf)
                {
                    yield return null;
                }
                if(gs.field[end.x,end.y].Team == Team.BLACK)
                {
                    ScaleMapFight(true,gs.field[end.x,end.y],gs.field[start.x,start.y]);
                }
                else
                {
                    ScaleMapFight(true,gs.field[start.x,start.y],gs.field[end.x,end.y]);
                }
                while(inFight)
                {
                    yield return null;
                }
                GameObject playerFindWithTag = GameObject.FindGameObjectWithTag("Player");
                playerFindWithTag.transform.position = playerBaseChessPoint.transform.position; 
                playerFindWithTag.transform.eulerAngles = playerBaseChessPoint.transform.eulerAngles; 
                if((MovePlayerTeam && !PlayerWinFpsGame) || (!MovePlayerTeam && PlayerWinFpsGame))
                {
                    inMove = false;
                    gs.field[start.x,start.y].Obj.transform.position = boardObject.position + PosToBoard(gs.field[start.x,start.y].Pos);
                    yield break;
                }
                gs.field[end.x,end.y].Dead = true;
            }
            gs.field[end.x,end.y] = gs.field[start.x,start.y];
            gs.field[end.x,end.y].HasMoved = true;
            gs.field[start.x,start.y] = null;
            gs.field[end.x,end.y].Pos = end;

            if (gs.field[end.x,end.y].Type == PieceType.PAWN)
            {
                if (Mathf.Abs(end.y - start.y) == 2)
                {
                    if (end.x - 1 >= 0)
                    {
                        if (gs.field[end.x - 1,end.y] != null)
                        {
                            if (gs.field[end.x - 1,end.y].Type == PieceType.PAWN)
                            {
                                Pawn pawn = (Pawn)(gs.field[end.x - 1,end.y]);
                                pawn.EnPassant = new KeyValuePair<bool, int>(true, -1);
                                gs.checkEnPassant = false;
                            }
                        }
                    }

                    if (end.x + 1 <= 7)
                    {
                        if (gs.field[end.x + 1,end.y] != null)
                        {
                            if (gs.field[end.x + 1,end.y].Type == PieceType.PAWN)
                            {
                                Pawn pawn = (Pawn)(gs.field[end.x + 1,end.y]);
                                pawn.EnPassant = new KeyValuePair<bool, int>(true, 1);
                                gs.checkEnPassant = false;
                            }
                        }
                    }
                }
            }
            inMove = false;
            yield return null;
        }


        private void exchange(Vector2Int start, Vector2Int end,GameState gs)
        {
            int y_draw = 0;
            Team team = Team.WHITE;

            if (gs.field[start.x,start.y].Team == Team.BLACK)
            {
                team = Team.BLACK;
            }

            bool quit = false;
            int x = -1;
            int y = -1;

            Piece clickedOn = new Queen(team,end,this);
            //Create new Piece between Rook,Knight,bishop,Queen

            if(gs.field[end.x,end.y] != null)
            {
                gs.field[end.x,end.y].Dead = true;                
            }
            gs.field[end.x,end.y] = clickedOn;
            gs.field[start.x,start.y].Dead = true;
            gs.field[start.x,start.y] = null;

        }

        private void enPassant(Vector2Int start, Vector2Int end,GameState gs)
        {
            if(gs.field[end.x,end.y] != null)
            {
                gs.field[end.x,end.y].Dead = true;                
            }
            Pawn pawn = (Pawn)(gs.field[start.x,start.y]);
            gs.field[end.x,end.y - pawn.Direction] = null;
            gs.field[end.x,end.y] = gs.field[start.x,start.y];
            gs.field[end.x,end.y].HasMoved = true;
            gs.field[start.x,start.y] = null;
            gs.field[end.x,end.y].Pos = end;
        }

        private void castles(Vector2Int start, Vector2Int end,GameState gs)
        {
            if (end.x == 0)
            {
                gs.field[1,end.y] = gs.field[3,end.y];
                gs.field[2,end.y] = gs.field[0,end.y];
                gs.field[1,end.y].HasMoved = true;
                gs.field[2,end.y].HasMoved = true;
                gs.field[1,end.y].Pos = new Vector2Int(1, end.y);
                gs.field[2,end.y].Pos = new Vector2Int(2, end.y);
                gs.field[3,end.y] = null;
                gs.field[0,end.y] = null;
            }
            else
            {
                gs.field[6,end.y] = gs.field[3,end.y];
                gs.field[5,end.y] = gs.field[7,end.y];
                gs.field[6,end.y].HasMoved = true;
                gs.field[5,end.y].HasMoved = true;
                gs.field[6,end.y].Pos = new Vector2Int(6, end.y);
                gs.field[5,end.y].Pos = new Vector2Int(5, end.y);
                gs.field[3,end.y] = null;
                gs.field[7,end.y] = null;
            }
        }

        private void disableEnPassant(GameState gs)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.field[i,j] != null)
                    {
                        if (gs.field[i,j].Type == PieceType.PAWN)
                        {
                            Pawn p = (Pawn)(gs.field[i,j]);
                            p.EnPassant = new KeyValuePair<bool, int>(false,0);
                        }
                    }
                }
            }
        }

        #region Getter Setter

        #endregion
    }
}