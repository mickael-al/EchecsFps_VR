using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
	public class Pawn : Piece
	{
		protected int m_dy;
		protected KeyValuePair<bool,int> m_enPassant = new KeyValuePair<bool,int>(false,0);
		Pawn(Team team, Vector2Int pos) : base(team,PieceType.PAWN,pos)
		{
			m_dy = team == Team.BLACK ? -1 : 1;
		}

		#region Getter Setter

		public KeyValuePair<bool,int> EnPassant
		{
			get{return m_enPassant;}
			set{m_enPassant = value;}
		}

		#endregion

		public override void calculePossibleMoves(Piece[,] field, bool check)
		{			
			m_possibleMoves.Clear();
			if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
			{
				if (field[m_pos.x,m_pos.y + m_dy] == null)
				{			
					m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x,m_pos.y + m_dy),MoveType.NEWPIECE,OwnKing,field,check);
				}
			}
			else
			{
				if (field[m_pos.x,m_pos.y + m_dy] == null)
				{
					m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x,m_pos.y + m_dy),MoveType.NORMAL,OwnKing,field,check);
				}
			}

			if ((m_pos.y + 2 * m_dy >= 0) && (m_pos.y + 2 * m_dy <= 7))
			{
				if (field[m_pos.x,m_pos.y + 2 * m_dy] == null && !m_hasMoved)
				{
					m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x,  m_pos.y + 2 * m_dy),MoveType.NORMAL,OwnKing,field,check);	
				}
			}

			if (m_pos.x + 1 <= 7)
			{
				if (field[m_pos.x + 1,m_pos.y + m_dy] != null)
				{
					if (field[m_pos.x + 1,m_pos.y + m_dy].Team != m_team)
					{
						if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
						{
							m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + 1, m_pos.y + m_dy),MoveType.NEWPIECE,OwnKing,field,check);
						}
						else
						{
							m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + 1, m_pos.y + m_dy),MoveType.NORMAL,OwnKing,field,check);
						}
					}
				}
			}
			if (m_pos.x - 1 >= 0)
			{
				if (field[m_pos.x - 1,m_pos.y + m_dy] != null)
				{
					if (field[m_pos.x - 1,m_pos.y + m_dy].Team != m_team)
					{
						if (m_pos.y + m_dy == 0 || m_pos.y + m_dy == 7)
						{
							m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x - 1, m_pos.y + m_dy),MoveType.NEWPIECE,OwnKing,field,check);
						}
						else
						{
							m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x - 1, m_pos.y + m_dy),MoveType.NORMAL,OwnKing,field,check);
						}
					}
				}
			}
			if (m_enPassant.Key && m_enPassant.Value == -1)
			{
				m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x + 1, m_pos.y + m_dy),MoveType.ENPASSANT,OwnKing,field,check);
			}
			if (m_enPassant.Key && m_enPassant.Value == 1)
			{
				m_possibleMoves = pushMove(m_possibleMoves,new Vector2Int(m_pos.x - 1, m_pos.y + m_dy),MoveType.ENPASSANT,OwnKing,field,check);
			}
		}
	}
}