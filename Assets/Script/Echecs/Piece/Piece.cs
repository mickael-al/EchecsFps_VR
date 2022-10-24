using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
	public abstract class Piece
	{
		protected Vector2Int m_pos;
		protected Team m_team;
		protected PieceType m_type;
		protected Dictionary<Vector2Int,MoveType> m_possibleMoves = new Dictionary<Vector2Int,MoveType>();
		protected bool m_hasMoved = false;

		public Piece(Team team,PieceType type,Vector2Int pos)
		{
			m_pos = pos;
			m_team = team;
			m_type = type;
		}

		public abstract void calculePossibleMoves(Piece[,] field, bool check);

		public Dictionary<Vector2Int,MoveType> PossibleMoves
		{
			get
			{
				return m_possibleMoves;
			}
		}

		public bool HasMoved
		{
			get
			{
				return m_hasMoved;
			}
		}

		public Team getTeam() 
		{ 
			return m_team;
		}

		public PieceType getType() 
		{ 
			return m_type;
		}

		public void setPosition(Vector2Int newPos) 
		{ 
			m_pos = newPos;
		}

		public Vector2Int getPos() 
		{ 
			return m_pos;
		}

		public King getOwnKing(Piece[,] field)//TODO refaire
		{
			for  (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (field[i,j] != null)
					{
						if (field[i,j].getTeam() == m_team && field[i,j].getType() == PieceType.KING)
						{
							return (King)(field[i,j]);
						}
					}
				}
			}
			return null;
		}

		public static void Swap(ref Piece p1,ref Piece p2)
		{
			Piece np = p1;
			p1 = p2;
			p2 = np;
		}

		public Dictionary<Vector2Int, MoveType> pushMove(Dictionary<Vector2Int, MoveType> moveList,KeyValuePair<Vector2Int,MoveType> move,King king,Piece[,] field,bool check)
		{
			if (!check)
			{
				moveList.Add(move.Key,move.Value);
			}
			else
			{
				bool enemyPlace = true;
				king.setCheck(field, king.getPos());
				Piece zwisch = field[move.Key.x,move.Key.y];
				enemyPlace = false;

				if (field[move.Key.x,move.Key.y] != null)
				{
					enemyPlace = true;
					field[move.Key.x,move.Key.y] = null;
				}

				Swap(ref field[move.Key.x,move.Key.y], ref field[m_pos.x,m_pos.y]);
				if (m_type == PieceType.KING)
				{
					king.setCheck(field, move.Key);
				}
				else
				{
					king.setCheck(field, king.getPos());
				}
				Swap(ref field[move.Key.x,move.Key.y],ref field[m_pos.x,m_pos.y]);

				if (enemyPlace)
				{
					field[move.Key.x,move.Key.y] = zwisch;
				}
				if (!king.Check)
				{
					moveList.Add(move.Key,move.Value);
				}
				king.setCheck(field, king.getPos());
			}
			return moveList;
		}
	}
}