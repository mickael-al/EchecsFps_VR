using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echecs
{
    public class GameState
    {
        public Piece[,] field = new Piece[8,8];
        public Move move = null; 
        public Team turn = Team.WHITE;
        public bool simulation = false;
        public bool endGame = false;
        public bool checkEnPassant;
    }
}
