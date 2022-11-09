using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class GameEchecs : MonoBehaviour
    {
        [SerializeField] private Transform boardObject = null;
        private static GameEchecs instance = null;
        private GameState gameState = new GameState();
        private Dictionary<PieceType,GameObject> prefabObjectPiece = new Dictionary<PieceType, GameObject>();
        private Dictionary<Team,Material> teamsMaterial = new Dictionary<Team,Material>();
        private Team botTeam = Team.BLACK;

        #region Getter Setter

        public static GameEchecs Instance { get{ return instance;}}
        public Dictionary<PieceType,GameObject> PrefabObjectPiece { get{ return prefabObjectPiece;}}
        public Dictionary<Team,Material> TeamsMaterial { get{ return teamsMaterial;}}
        public Transform BoardObject { get{ return boardObject;}}

        #endregion
    
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

        private void Update()
        {

        }

        public void Simulate(GameState gs)
        {
            gs.move=null;
            
            if(botTeam == gs.turn)
            {
                
            }
            else
            {

            }
            if(gs.move != null)
            {
                gs.turn = gs.turn == Team.WHITE ? Team.BLACK : Team.WHITE;
            }
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

        #region Getter Setter

        #endregion
    }
}