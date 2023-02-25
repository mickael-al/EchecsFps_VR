using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class Knight : Piece
    {
        public override float fps_LifeStat { get{return 100.0f;}}
	    public override float fps_SpeedStat { get{return 8.0f;}}
	    public override float fps_rateProjectileStat { get{return 0.4f;}}
        public Knight(Team team, Vector2Int pos,GameEchecs ge) : base(team,PieceType.KNIGHT,pos,ge){}

        public override void calculePossibleMoves(Piece[,] field, bool check)
        {
            m_possibleMoves.Clear();
            int dx,dy;
	        for (dx = -2; dx <= 2 ; dx += 4)
	        {
		        for (dy = -1; dy <= 1; dy += 2)
		        {
			        if (m_pos.x + dx >= 0 && m_pos.x + dx <= 7 && m_pos.y + dy >= 0 && m_pos.y + dy <= 7)
			        {
				        if (field[m_pos.x + dx,m_pos.y + dy] == null)
				        {
					        m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
				        }
				        else if (field[m_pos.x + dx,m_pos.y + dy] != null)
				        {
					        if (field[m_pos.x + dx,m_pos.y + dy].Team != m_team)
					        {
						        m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
					        }
				        }
			        }
		        }
	        }

            for (dy = -2; dy <= 2; dy += 4)
            {
                for (dx = -1; dx <= 1; dx += 2)
                {
                    if (m_pos.x + dx >= 0 && m_pos.x + dx <= 7 && m_pos.y + dy >= 0 && m_pos.y + dy <= 7)
                    {
                        if (field[m_pos.x + dx,m_pos.y + dy] == null)
                        {
                            m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
                        }
                        else if (field[m_pos.x + dx,m_pos.y + dy] != null)
                        {
                            if (field[m_pos.x + dx,m_pos.y + dy].Team != m_team)
                            {
                                m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dx, m_pos.y + dy), MoveType.NORMAL,OwnKing,field,check);
                            }
                        }
                    }
                }
            }
        }
    }
}