using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class Queen : Piece
    {
        public Queen(Team team, Vector2Int pos,GameEchecs ge) : base(team,PieceType.QUEEN,pos,ge){}

        public override float fps_LifeStat { get{return 350.0f;}}
	    public override float fps_SpeedStat { get{return 10.0f;}}
	    public override float fps_rateProjectileStat { get{return 0.1f;}}

        public override void calculePossibleMoves(Piece[,] field, bool check)
        {
            m_possibleMoves.Clear();
            int dxcp,dycp,dx,dy;
            for (dx = -1; dx <= 1; dx++)
            {
                for (dy = -1; dy <= 1; dy++)
                {
                    dxcp = dx;
                    dycp = dy;
                    while ((m_pos.x + dxcp >= 0 && m_pos.x + dxcp <= 7 && m_pos.y + dycp >= 0 && m_pos.y + dycp <= 7) && field[m_pos.x + dxcp,m_pos.y + dycp] == null)
                    {
                        m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dxcp, m_pos.y + dycp), MoveType.NORMAL,OwnKing,field,check);
                        if (dxcp < 0) {dxcp -= 1;} else if (dxcp > 0){dxcp += 1;}
                        if (dycp < 0) {dycp -= 1;} else if (dycp > 0){dycp += 1;}
                    }
                    if ((m_pos.x + dxcp >= 0 && m_pos.x + dxcp <= 7 && m_pos.y + dycp >= 0 && m_pos.y + dycp <= 7) && field[m_pos.x + dxcp,m_pos.y + dycp] != null)
                    {
                        if (field[m_pos.x + dxcp,m_pos.y + dycp].Team != m_team)
                        {
                            m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + dxcp, m_pos.y + dycp), MoveType.NORMAL,OwnKing,field,check);
                        }
                    }
                }
            }
        }
    }
}