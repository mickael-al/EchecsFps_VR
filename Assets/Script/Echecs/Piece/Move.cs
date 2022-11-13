using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class Move
    {
        private Team team;
        public Piece targetPiece;
        public Vector2Int move;
        public MoveType moveType;
        public Move(Team t,Piece tp,Vector2Int m,MoveType mt)
        {
            team = t;
            targetPiece = tp;
            move = m;
            moveType = mt;
        }
    }
}
