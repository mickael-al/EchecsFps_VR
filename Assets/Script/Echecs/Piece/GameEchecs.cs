using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class GameEchecs : MonoBehaviour
    {
        private GameState gameState = new GameState();

        public void Awake()
        {                       
            InitGameState(gameState);
        }      

        public void InitGameState(GameState gs)
        {
            Clear();
            gs.field[0,1] = new Pawn(Team.WHITE,new Vector2Int(0,1));
            gs.field[1,1] = new Pawn(Team.WHITE,new Vector2Int(1,1));
            gs.field[2,1] = new Pawn(Team.WHITE,new Vector2Int(2,1));
            gs.field[3,1] = new Pawn(Team.WHITE,new Vector2Int(3,1));
            gs.field[4,1] = new Pawn(Team.WHITE,new Vector2Int(4,1));
            gs.field[5,1] = new Pawn(Team.WHITE,new Vector2Int(5,1));
            gs.field[6,1] = new Pawn(Team.WHITE,new Vector2Int(6,1));
            gs.field[7,1] = new Pawn(Team.WHITE,new Vector2Int(7,1));

            gs.field[0,6] = new Pawn(Team.BLACK,new Vector2Int(0,6));
            gs.field[1,6] = new Pawn(Team.BLACK,new Vector2Int(1,6));
            gs.field[2,6] = new Pawn(Team.BLACK,new Vector2Int(2,6));
            gs.field[3,6] = new Pawn(Team.BLACK,new Vector2Int(3,6));
            gs.field[4,6] = new Pawn(Team.BLACK,new Vector2Int(4,6));
            gs.field[5,6] = new Pawn(Team.BLACK,new Vector2Int(5,6));
            gs.field[6,6] = new Pawn(Team.BLACK,new Vector2Int(6,6));
            gs.field[7,6] = new Pawn(Team.BLACK,new Vector2Int(7,6));

            
        }  

        public void Clear()
        {
            for(int i = 0; i < 8;i++)
            {
                for(int j = 0; j < 8;j++)
                {
                    gameState.field[i,j] = null;
                }
            }

        }

        #region Getter Setter

        #endregion
    }
}