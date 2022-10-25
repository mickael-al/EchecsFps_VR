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

		#region Getter Setter

		public Dictionary<Vector2Int,MoveType> PossibleMoves
		{
			get{return m_possibleMoves;}
		}

		public bool HasMoved
		{
			get{return m_hasMoved;}
		}

		public Team Team 
		{ 
			get{return m_team;}
		}

		public PieceType Type
		{ 
			get{return m_type;}
		}

		public Vector2Int Pos
		{
			get{return m_pos;}
			set{m_pos = value;}
		}

		public King OwnKing
		{
			get{return King.GetKingByTeam(m_team);}
		}

		#endregion

		public static void Swap(ref Piece p1,ref Piece p2)
		{
			Piece np = p1;
			p1 = p2;
			p2 = np;
		}

		public Dictionary<Vector2Int, MoveType> pushMove(Dictionary<Vector2Int, MoveType> moveList,Vector2Int moveP,MoveType moveT,King king,Piece[,] field,bool check)
		{
			if (!check)
			{
				moveList.Add(moveP,moveT);
			}
			else
			{
				bool enemyPlace = true;
				king.setCheck(field, king.Pos);
				Piece zwisch = field[moveP.x,moveP.y];
				enemyPlace = false;

				if (field[moveP.x,moveP.y] != null)
				{
					enemyPlace = true;
					field[moveP.x,moveP.y] = null;
				}

				Swap(ref field[moveP.x,moveP.y], ref field[m_pos.x,m_pos.y]);
				if (m_type == PieceType.KING)
				{
					king.setCheck(field, moveP);
				}
				else
				{
					king.setCheck(field, king.Pos);
				}
				Swap(ref field[moveP.x,moveP.y],ref field[m_pos.x,m_pos.y]);

				if (enemyPlace)
				{
					field[moveP.x,moveP.y] = zwisch;
				}
				if (!king.Check)
				{
					moveList.Add(moveP,moveT);
				}
				king.setCheck(field, king.Pos);
			}
			return moveList;
		}
	}
}