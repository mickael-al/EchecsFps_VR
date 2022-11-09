using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class King : Piece
    {
        private bool m_check;
        private static Dictionary<Team,King> m_listOfKing = new Dictionary<Team, King>();
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

        public override void calculePossibleMoves(Piece[,] field, bool check)
        {
            m_possibleMoves.Clear();            
            bool castles = true;            
            int a, b, c;

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

            if (!m_hasMoved)
            {
                for (int i = 0; i <= 7; i += 7)
                {
                    for (int j = 0; j <= 7; j += 7)
                    {
                        castles = true;
                        
                            if (field[i,j] != null && field[i,j].Team == m_team && field[i,j].Type == PieceType.ROOK && !field[i,j].HasMoved)
                            {                                
                                if (i == 0)
                                {
                                    a = 1;b = 2;c = 3;
                                }
                                else
                                {
                                    a = 5;b = 6;c = 6;
                                }
                                if (field[a,j] == null && field[b,j] == null && field[c,j] == null)
                                {
                                    for (int k = 0; k < 8; k++)
                                    {
                                        for (int l = 0; l < 8; l++)
                                        {
                                            if (field[k,l] != null)
                                            {
                                                if (field[k,l].Team != m_team)
                                                {                                                    
                                                    foreach (var v in field[k,l].PossibleMoves)
                                                    {
                                                        if (i == 0)
                                                        {
                                                            if ((v.Key.x == 4 && v.Key.y == j) || (v.Key.x == 2 && v.Key.y == j) || (v.Key.x == 3 && v.Key.y == j))
                                                            {
                                                                castles = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if ((v.Key.x == 5 && v.Key.y == j) || (v.Key.x == 6 && v.Key.y == j) || (v.Key.x == 4 && v.Key.y == j))
                                                            {
                                                                castles = false;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (castles)
                                    {                                        
                                        m_possibleMoves.Add(new Vector2Int(i,j),MoveType.CASTLE);                                    
                                    }
                                }
                            }                        
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