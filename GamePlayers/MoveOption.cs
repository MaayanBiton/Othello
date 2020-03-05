using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GamePlayers
{
    public struct MoveOption
    {
        public int[]            m_WhereIsTheDamageOfTheMove;
        private Point           m_PossibleMoveInGameBoard;
        private int[]           m_DamageOfMove;

        public MoveOption(Point i_optionalMove)
        {
            m_WhereIsTheDamageOfTheMove = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };        
            m_PossibleMoveInGameBoard = new Point(i_optionalMove.X, i_optionalMove.Y);
            m_DamageOfMove = new int[1] { 0 };
        }

        public int DamageOfMove
        {
            get { return m_DamageOfMove[0]; }
            set { m_DamageOfMove[0] = value; }
        }

        public Point PossibleMove
        {
            get { return m_PossibleMoveInGameBoard; }
            set { m_PossibleMoveInGameBoard = value; }
        }
    }
}