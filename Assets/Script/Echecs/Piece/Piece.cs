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
		protected GameObject m_obj = null;
		protected bool m_dead = false;
		private GameEchecs m_gameEchecs = null;

		public Piece(Team team,PieceType type,Vector2Int pos,GameEchecs ge)
		{
			m_pos = pos;
			m_team = team;
			m_type = type;	
			m_gameEchecs = ge;
			m_obj = GameObject.Instantiate(ge.PrefabObjectPiece[m_type],Vector3.zero,Quaternion.identity,ge.BoardObject);
			if(m_obj != null)
			{
				m_obj.GetComponent<Renderer>().material = ge.TeamsMaterial[m_team];
				UpdatOBJPosition();
			}
		}

		public void UpdatOBJPosition()
		{
			m_obj.transform.position = m_gameEchecs.BoardObject.position+ GameEchecs.PosToBoard(m_pos);
			m_obj.transform.rotation = Quaternion.Euler(0,m_team == Team.BLACK ? 180.0f : 0.0f,0);
		}

		public abstract void calculePossibleMoves(Piece[,] field, bool check);

		#region Getter Setter

		public Dictionary<Vector2Int,MoveType> PossibleMoves { get{return m_possibleMoves;} }
		public bool HasMoved { get{return m_hasMoved;} set{ m_hasMoved = value;}}
		public Team Team { get{return m_team;} }
		public PieceType Type { get{return m_type;} }
		public Vector2Int Pos { get{return m_pos;} set{m_pos = value;UpdatOBJPosition();} }
		public King OwnKing { get{return King.GetKingByTeam(m_team);} }
		public GameObject Obj { get{return m_obj;}}

		public virtual float fps_LifeStat { get{return 0.0f;}}
		public virtual float fps_SpeedStat { get{return 0.0f;}}
		public virtual float fps_rateProjectileStat { get{return 0.0f;}}

		public bool Dead 
		{ 
			get{return m_dead;}
			set
			{
				m_dead = value;
				GameObject.Destroy(m_obj);
			} 
		}

		#endregion

		public static void Swap(ref Piece p1,ref Piece p2)
		{
			Piece np = p1;
			p1 = p2;
			p2 = np;
		}

		public static bool isInBoard(Vector2Int p)
		{
			return p.x >= 0 && p.x <= 7 && p.y >= 0 && p.y <= 7;
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
					moveList[moveP] = moveT;
				}
				king.setCheck(field, king.Pos);
			}
			return moveList;
		}
	}
}