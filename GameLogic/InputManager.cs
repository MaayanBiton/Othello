using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing;
using GamePlayers;

namespace GameLogic
{
    public class InputManager
    {
        private const char      k_UpperCaseQuit = 'Q';
        private const char      k_LowerCaseQuit = 'q';
        private const char      k_LeftBorder = 'A';
        private const char      k_AsciiZero = '0';
        private const char      k_UpBorder = '1';
        private const char      k_BlackPlayerChar = 'X';

        public static void UpdateBoard(MoveOption i_ChosenInputMove, char i_PlayerColor)
        {
            Point tempBoardPoint = new Point();
                  
            for (int i = 0; i < i_ChosenInputMove.m_WhereIsTheDamageOfTheMove.Length; i++)
            {
                tempBoardPoint = i_ChosenInputMove.PossibleMove;
                for (int j = 0; j <= i_ChosenInputMove.m_WhereIsTheDamageOfTheMove[i]; j++)
                {
                    BoardState.m_PlayerPositionsInDemiBoard[tempBoardPoint.X, tempBoardPoint.Y] = i_PlayerColor;

                    if (i_PlayerColor == k_BlackPlayerChar)
                    {
                        if (!BoardState.m_BlackPositionList.Contains(tempBoardPoint))
                        {
                            BoardState.m_BlackPositionList.AddFirst(tempBoardPoint);
                            BoardState.m_WhitePositionList.Remove(tempBoardPoint);
                        }
                    }
                    else
                    {
                        if (!BoardState.m_WhitePositionList.Contains(tempBoardPoint))
                        {
                            BoardState.m_WhitePositionList.AddFirst(tempBoardPoint);
                            BoardState.m_BlackPositionList.Remove(tempBoardPoint);
                        }
                    }

                    tempBoardPoint = CheckWhatDirection(i, tempBoardPoint);
                }
            }
        }

        public static Point CheckWhatDirection(int i_DirectionArrayIndex, Point i_DemiPoint)
        {
            Point tempBoardPoint = new Point();

            switch (i_DirectionArrayIndex)
            {
                case (int)BoardState.ePossibleDirections.Up:
                    tempBoardPoint.X = i_DemiPoint.X + 1;
                    tempBoardPoint.Y = i_DemiPoint.Y;
                    break;
                case (int)BoardState.ePossibleDirections.Down:
                    tempBoardPoint.X = i_DemiPoint.X - 1;
                    tempBoardPoint.Y = i_DemiPoint.Y;
                    break;
                case (int)BoardState.ePossibleDirections.Right:
                    tempBoardPoint.X = i_DemiPoint.X;
                    tempBoardPoint.Y = i_DemiPoint.Y - 1;
                    break;
                case (int)BoardState.ePossibleDirections.Left:
                    tempBoardPoint.X = i_DemiPoint.X;
                    tempBoardPoint.Y = i_DemiPoint.Y + 1;
                    break;
                case (int)BoardState.ePossibleDirections.DownLeft:
                    tempBoardPoint.X = i_DemiPoint.X - 1;
                    tempBoardPoint.Y = i_DemiPoint.Y + 1;
                    break;
                case (int)BoardState.ePossibleDirections.UpLeft:
                    tempBoardPoint.X = i_DemiPoint.X + 1;
                    tempBoardPoint.Y = i_DemiPoint.Y + 1;
                    break;
                case (int)BoardState.ePossibleDirections.DownRight:
                    tempBoardPoint.X = i_DemiPoint.X - 1;
                    tempBoardPoint.Y = i_DemiPoint.Y - 1;
                    break;
                case (int)BoardState.ePossibleDirections.UpRight:
                    tempBoardPoint.X = i_DemiPoint.X + 1;
                    tempBoardPoint.Y = i_DemiPoint.Y - 1;
                    break;

                default:
                    break;
            }

            return tempBoardPoint;
        }

        public static bool IsInputInListOfPossibleMoves(LinkedList<MoveOption> i_ValidMoves, Point i_InputMove, ref MoveOption io_FoundPointInList)
        {
            bool isMoveFoundInList = false;

            foreach (MoveOption MoveInArray in i_ValidMoves)
            {
                if (MoveInArray.PossibleMove == i_InputMove)
                {
                    isMoveFoundInList = true;
                    io_FoundPointInList = MoveInArray;
                    break;
                }
            }

            return isMoveFoundInList;
        }

        public static MoveOption FindMaxDamage(LinkedList<MoveOption> i_ComputerValidMoves)
        {
            MoveOption bestOptionalMove = new MoveOption();
            int maxDamage = i_ComputerValidMoves.First.Value.DamageOfMove;
            bestOptionalMove = i_ComputerValidMoves.First.Value;

            foreach (MoveOption currentPossibleMove in i_ComputerValidMoves)
            {
                if (currentPossibleMove.DamageOfMove > maxDamage)
                {
                    maxDamage = currentPossibleMove.DamageOfMove;
                    bestOptionalMove = currentPossibleMove;
                }
            }

            return bestOptionalMove;
        }
    }
}
