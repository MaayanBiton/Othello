using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GamePlayers;

namespace GameLogic
{
    public class BoardState
    {
        public static char[,]               m_PlayerPositionsInDemiBoard;
        public static LinkedList<Point>     m_BlackPositionList = new LinkedList<Point>();
        public static LinkedList<Point>     m_WhitePositionList = new LinkedList<Point>();

        public enum eBoardSizeOption
        {
            Six = 6,
            Eight = 8,
            Ten = 10,
            Twelve = 12
        }

        public enum ePossibleDirections
        {
            Up,
            Down,
            Left,
            Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }

        private enum eDirections
        {
            Up = -1,
            Down = 1,
            Left = -1,
            Right = 1,
            None = 0
        }

        public static void InitDemiBoard(int i_BoardSize)
        {
            int firstPosition = (i_BoardSize / 2) - 1;

            m_PlayerPositionsInDemiBoard = new char[i_BoardSize, i_BoardSize];
            m_PlayerPositionsInDemiBoard[firstPosition, firstPosition] = 'O';
            m_WhitePositionList.AddFirst(new Point(firstPosition, firstPosition));
            m_PlayerPositionsInDemiBoard[firstPosition + 1, firstPosition + 1] = 'O';
            m_WhitePositionList.AddFirst(new Point(firstPosition + 1, firstPosition + 1));
            m_PlayerPositionsInDemiBoard[firstPosition, firstPosition + 1] = 'X';
            m_BlackPositionList.AddFirst(new Point(firstPosition, firstPosition + 1));
            m_PlayerPositionsInDemiBoard[firstPosition + 1, firstPosition] = 'X';
            m_BlackPositionList.AddFirst(new Point(firstPosition + 1, firstPosition));
        }

        public static void CheckOptionalMoves(LinkedList<MoveOption> i_BlackMoveOptions, LinkedList<MoveOption> i_WhiteMoveOptions)
        {
            LookAroundForOpponentCoin(m_BlackPositionList, i_BlackMoveOptions, 'O');
            LookAroundForOpponentCoin(m_WhitePositionList, i_WhiteMoveOptions, 'X');
        }

        public static bool AreThereAnyMovesLeft(LinkedList<MoveOption> i_PossibleMoveList)
        {
            return i_PossibleMoveList.Count == 0;
        }

        private static void LookAroundForOpponentCoin(LinkedList<Point> i_PlayerListMove, LinkedList<MoveOption> i_MoveOptions, char i_OpponentCoin)
        {
            foreach (Point currentPoint in i_PlayerListMove)
            {
                if ((currentPoint.X > 0) && (m_PlayerPositionsInDemiBoard[currentPoint.X - 1, currentPoint.Y] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Up, eDirections.None, ePossibleDirections.Up, i_OpponentCoin);
                }

                if ((currentPoint.X < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && (m_PlayerPositionsInDemiBoard[currentPoint.X + 1, currentPoint.Y] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Down, eDirections.None, ePossibleDirections.Down, i_OpponentCoin);
                }

                if ((currentPoint.Y < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && (m_PlayerPositionsInDemiBoard[currentPoint.X, currentPoint.Y + 1] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.None, eDirections.Right, ePossibleDirections.Right, i_OpponentCoin);
                }

                if ((currentPoint.Y > 0) && (m_PlayerPositionsInDemiBoard[currentPoint.X, currentPoint.Y - 1] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.None, eDirections.Left, ePossibleDirections.Left, i_OpponentCoin);
                }

                if ((currentPoint.X > 0) && (currentPoint.Y > 0) && (m_PlayerPositionsInDemiBoard[currentPoint.X - 1, currentPoint.Y - 1] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Up, eDirections.Left, ePossibleDirections.UpLeft, i_OpponentCoin);
                }

                if ((currentPoint.X > 0) && (currentPoint.Y < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && (m_PlayerPositionsInDemiBoard[currentPoint.X - 1, currentPoint.Y + 1] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Up, eDirections.Right, ePossibleDirections.UpRight, i_OpponentCoin);
                }

                if ((currentPoint.X < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && (currentPoint.Y > 0) && (m_PlayerPositionsInDemiBoard[currentPoint.X + 1, currentPoint.Y - 1] == i_OpponentCoin))
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Down, eDirections.Left, ePossibleDirections.DownLeft, i_OpponentCoin);
                }

                if ((currentPoint.X < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && (currentPoint.Y < m_PlayerPositionsInDemiBoard.GetLength(0) - 1) && m_PlayerPositionsInDemiBoard[currentPoint.X + 1, currentPoint.Y + 1] == i_OpponentCoin)
                {
                    UpdatePossibleMovesArray(i_MoveOptions, currentPoint, eDirections.Down, eDirections.Right, ePossibleDirections.DownRight, i_OpponentCoin);
                }
            }
        }

        private static void UpdatePossibleMovesArray(LinkedList<MoveOption> i_MoveOptions, Point currentPoint, eDirections i_UpOrDown, eDirections i_LeftOrRight, ePossibleDirections i_OptionalDiractions, char i_OpponentCoin)
        {
            int numOfFlippedCoins = 0;
            Point tempOfcurrentPoint = new Point();
            tempOfcurrentPoint = currentPoint;
            MoveOption foundOptionalMove = new MoveOption(new Point(0, 0));
            bool isPointInListAlready = false;

            tempOfcurrentPoint.X += (int)i_UpOrDown;
            tempOfcurrentPoint.Y += (int)i_LeftOrRight;

            while (tempOfcurrentPoint.X <= m_PlayerPositionsInDemiBoard.GetLength(0) - 1 && tempOfcurrentPoint.X >= 0 && tempOfcurrentPoint.Y >= 0 && tempOfcurrentPoint.Y <= m_PlayerPositionsInDemiBoard.GetLength(0) - 1)
            {
                if (m_PlayerPositionsInDemiBoard[tempOfcurrentPoint.X, tempOfcurrentPoint.Y] != i_OpponentCoin && m_PlayerPositionsInDemiBoard[tempOfcurrentPoint.X, tempOfcurrentPoint.Y] != 0)
                {
                    break;
                }

                if (m_PlayerPositionsInDemiBoard[tempOfcurrentPoint.X, tempOfcurrentPoint.Y] == 0)
                {
                    if (!InputManager.IsInputInListOfPossibleMoves(i_MoveOptions, tempOfcurrentPoint, ref foundOptionalMove))
                    {
                        foundOptionalMove.PossibleMove = tempOfcurrentPoint;
                        i_MoveOptions.AddFirst(foundOptionalMove);
                    }
                    else
                    {
                        isPointInListAlready = InputManager.IsInputInListOfPossibleMoves(i_MoveOptions, tempOfcurrentPoint, ref foundOptionalMove);
                    }

                    foundOptionalMove.m_WhereIsTheDamageOfTheMove[(int)i_OptionalDiractions] = numOfFlippedCoins;
                    foundOptionalMove.DamageOfMove = numOfFlippedCoins + foundOptionalMove.DamageOfMove;

                    break;
                }

                numOfFlippedCoins++;
                tempOfcurrentPoint.X += (int)i_UpOrDown;
                tempOfcurrentPoint.Y += (int)i_LeftOrRight;
            }
        }
    }
}