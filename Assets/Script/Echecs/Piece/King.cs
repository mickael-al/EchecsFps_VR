using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class King : Piece
    {
        private bool m_check;
        private static Dictionary<Team,King> m_listOfKing = new Dictionary<Team, King>();
        private int turnMoveOnlyThis = 0;
        public King(Team team, Vector2Int pos,GameEchecs ge) : base(team,PieceType.KING,pos,ge)
        {
            if(!m_listOfKing.ContainsKey(team))
            {
                m_listOfKing.Add(team,this);
            }
            else
            {
                Debug.LogError("Only one King per Team");
            }
        }

        public static King GetKingByTeam(Team t)
        {
            return m_listOfKing[t];
        }

        public bool Check
        {
            get
            {
                return m_check;
            }
        }

        public int TurnMoveOnly
        {
            get{ return turnMoveOnlyThis;}
            set{ turnMoveOnlyThis = value;}
        }

        public override void calculePossibleMoves(Piece[,] field, bool check)
        {
            m_possibleMoves.Clear();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (m_pos.x + dx >= 0 && m_pos.x + dx <= 7 && m_pos.y + dy >= 0 && m_pos.y + dy <= 7)
                    {
                        if (field[m_pos.x + dx,m_pos.y + dy] != null)
                        {
                            if (field[m_pos.x + dx,m_pos.y + dy].Team != m_team)
                            {
                                m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
                            }
                        }
                        else
                        {
                            m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
                        }
                    }
                }
            }
            if(!m_hasMoved)
            {
                if(m_pos.y == 0 || m_pos.y == 7)
                {
                    if(field[7,m_pos.y] != null && !field[7,m_pos.y].HasMoved && field[4,m_pos.y] == null && field[5,m_pos.y] == null && field[6,m_pos.y] == null)
                    {
                        m_possibleMoves.Add(new Vector2Int(7,m_pos.y),MoveType.CASTLE);   
                    }
                    if(field[0,m_pos.y] != null && !field[0,m_pos.y].HasMoved && field[1,m_pos.y] == null && field[2,m_pos.y] == null)
                    {
                        m_possibleMoves.Add(new Vector2Int(0,m_pos.y),MoveType.CASTLE);   
                    }
                }
            }
        }

        public void setCheck(Piece[,] field, Vector2Int p)
        {
            m_check = false;
            int i,j,dy_pawn;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {                    
                        if (field[i,j] != null && field[i,j].Team != m_team)
                        {
                            if (field[i,j].Type == PieceType.KING)
                            {
                                if (Mathf.Abs(field[i,j].Pos.x - p.x) <= 1 && Mathf.Abs(field[i,j].Pos.y - p.y) <= 1)
                                {
                                    m_check = true;
                                }

                            }
                            else if (field[i,j].Type == PieceType.PAWN)
                            {
                                dy_pawn = field[i,j].Team == Team.WHITE ? 1 : -1;
                                if ((p.x == field[i,j].Pos.x + 1 || p.x == field[i,j].Pos.x - 1) && p.y == field[i,j].Pos.y + dy_pawn)
                                {
                                    m_check = true;
                                }
                            }
                            else
                            {
                                field[i,j].calculePossibleMoves(field, false);                                
                                foreach(var v in field[i,j].PossibleMoves)
                                {
                                    if(v.Key.x == p.x && v.Key.y == p.y)
                                    {
                                        m_check = true;
                                    }
                                }
                            }
                        }                    
                }
            }        
        }
    }
}