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
        public Move(Team t)
        {
            team = t;
        }
    }
}
