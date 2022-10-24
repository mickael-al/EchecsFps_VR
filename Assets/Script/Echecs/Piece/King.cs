using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class King : Piece
    {
        private bool m_check;
        King(Team team, Vector2Int pos) : base(team,PieceType.KING,pos){}

        public bool Check
        {
            get
            {
                return m_check;
            }
        }

        public override void calculePossibleMoves(Piece[,] field, bool check)
        {

        }

        public void setCheck(Piece[,] field, Vector2Int p)
        {
            bool check = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (field[i,j] != null)
                    {
                        if (field[i,j].getTeam() != m_team)
                        {
                            if (field[i,j].getType() == PieceType.KING)
                            {
                                if (Mathf.Abs(field[i,j].getPos().x - p.x) <= 1 && Mathf.Abs(field[i,j].getPos().y - p.y) <= 1)
                                {
                                    check = true;
                                }

                            }
                            else if (field[i,j].getType() == PieceType.PAWN)
                            {
                                int dy_pawn;
                                if (field[i,j].getTeam() == Team.WHITE)
                                {
                                    dy_pawn = 1;
                                }
                                else
                                {
                                    dy_pawn = -1;
                                }
                                if ((p.x == field[i,j].getPos().x + 1 || p.x == field[i,j].getPos().x - 1) && p.y == field[i,j].getPos().y + dy_pawn)
                                {
                                    check = true;
                                }
                            }
                            else
                            {
                                field[i,j].calculePossibleMoves(field, false);
                                Dictionary<Vector2Int,MoveType> np = field[i,j].PossibleMoves;
                                foreach(var v in np)
                                {
                                    if(v.Key.x == p.x && v.Key.y == p.y)
                                    {
                                        check = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            m_check = check;
        }
    }
}