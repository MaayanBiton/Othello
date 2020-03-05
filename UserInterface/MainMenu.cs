using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamePlayers;
using GameLogic;

namespace UserInterface
{
    public class MainMenu
    {
        private Player          m_BlackPlayer = new Player();
        private Player          m_WhitePlayer;
        private int             m_ChosenOpponent;
        private bool            m_IsBlackPlayerTurn = true;

        public enum eOpponentOption
        {
            Human = 1,
            Computer = 2
        }

        public enum eBoardSizeOption
        {
            Six = 6,
            Eight = 8
        }

        public Player BlackPlayer
        {
            get { return m_BlackPlayer; }
        }

        public Player WhitePlayer
        {
            get { return m_WhitePlayer; }
        }

        public bool IsBlackTurn
        {
            get { return m_IsBlackPlayerTurn; }
            set { m_IsBlackPlayerTurn = value; }
        }

        public int Opponent
        {
            get { return m_ChosenOpponent; }
        }

        public Player[] SetPlayers(int i_OpponentType)
        {
            Player[] playerArray = new Player[2];
            m_BlackPlayer.UserType = Player.eUserType.Human;
            m_BlackPlayer.CoinColor = 'X';
            m_BlackPlayer.IsMyTurn = true;

            m_WhitePlayer = new Player(); 
            if ((eOpponentOption)i_OpponentType == eOpponentOption.Human)
            {
                m_WhitePlayer.UserType = Player.eUserType.Human;
            }
            else if ((eOpponentOption)i_OpponentType == eOpponentOption.Computer)
            {
                m_WhitePlayer.UserType = Player.eUserType.Computer;
            }

            m_WhitePlayer.CoinColor = 'O';
            playerArray[0] = m_BlackPlayer;
            playerArray[1] = m_WhitePlayer;
            m_ChosenOpponent = i_OpponentType;
            return playerArray;
        }
    }
}