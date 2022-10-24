using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
	public class Pawn : Piece
	{
		protected int m_dy;
		Pawn(Team team, Vector2Int pos) : base(team,PieceType.PAWN,pos)
		{
			m_dy = team == Team.BLACK ? -1 : 1;
		}

		public override void calculePossibleMoves(Piece[,] field, bool check)
		{
			Dictionary<Vector2Int, MoveType> moves = new Dictionary<Vector2Int, MoveType>();

			if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
			{
				if (field[m_pos.x,m_pos.y + m_dy] == null)
				{			
					moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x,m_pos.y + m_dy),MoveType.NEWPIECE),getOwnKing(field),field,check);
				}
			}
			else
			{
				if (field[m_pos.x,m_pos.y + m_dy] == null)
				{
					moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x,m_pos.y + m_dy),MoveType.NORMAL),getOwnKing(field),field,check);
				}
			}

			if ((m_pos.y + 2 * m_dy >= 0) && (m_pos.y + 2 * m_dy <= 7))
			{
				if (field[m_pos.x,m_pos.y + 2 * m_dy] == null && !m_hasMoved)
				{
					moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x,  m_pos.y + 2 * m_dy),MoveType.NORMAL),getOwnKing(field),field,check);	
				}
			}

			if (m_pos.x + 1 <= 7)
			{
				if (field[m_pos.x + 1,m_pos.y + m_dy] != null)
				{
					if (field[m_pos.x + 1,m_pos.y + m_dy].getTeam() != m_team)
					{
						if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
						{
							moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x + 1, m_pos.y + m_dy),MoveType.NEWPIECE),getOwnKing(field),field,check);
						}
						else
						{
							moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x + 1, m_pos.y + m_dy),MoveType.NORMAL),getOwnKing(field),field,check);
						}
					}
				}
			}
			if (m_pos.x - 1 >= 0)
			{
				if (field[m_pos.x - 1,m_pos.y + m_dy] != null)
				{
					if (field[m_pos.x - 1,m_pos.y + m_dy].getTeam() != m_team)
					{
						if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
						{
							moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x - 1, m_pos.y + m_dy),MoveType.NEWPIECE),getOwnKing(field),field,check);
						}
						else
						{
							moves = pushMove(moves,new KeyValuePair<Vector2Int,MoveType>(new Vector2Int(m_pos.x - 1, m_pos.y + m_dy),MoveType.NORMAL),getOwnKing(field),field,check);
						}
					}
				}
			}
		}
	}
}