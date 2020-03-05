using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLogic;
using GamePlayers;

namespace UserInterface
{
    public partial class MenuForm : Form
    {
        private MainMenu m_MenuSetting = new MainMenu();
        private int m_BoardSize = 6;
        private GameForm m_GameForm = new GameForm();

        public MenuForm()
        {
            this.InitializeComponent();
        }

        public GameForm GameForm
        {
            get { return this.m_GameForm; }
            set { this.m_GameForm = value; }
        }

        private void buttonHuman_Click(object sender, EventArgs e)
        {
            this.m_GameForm.PlayerArray = this.m_MenuSetting.SetPlayers((int)MainMenu.eOpponentOption.Human);
            BoardState.InitDemiBoard(this.m_BoardSize);
            BoardState.CheckOptionalMoves(this.m_GameForm.PlayerArray[0].m_ValidMoves, this.m_GameForm.PlayerArray[1].m_ValidMoves);
            this.Close();
            this.Hide();
            this.m_GameForm.BoardSize = this.m_BoardSize;
            this.m_GameForm.ShowDialog();   
        }

        private void buttonComputer_Click(object sender, EventArgs e)
        {
            this.m_GameForm.PlayerArray = this.m_MenuSetting.SetPlayers((int)MainMenu.eOpponentOption.Computer);
            BoardState.InitDemiBoard(this.m_BoardSize);
            BoardState.CheckOptionalMoves(this.m_GameForm.PlayerArray[0].m_ValidMoves, this.m_GameForm.PlayerArray[1].m_ValidMoves);
            this.Close();
            this.Hide();
            this.m_GameForm.BoardSize = this.m_BoardSize;
            this.m_GameForm.ShowDialog();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            this.m_BoardSize += 2;

            if (this.m_BoardSize == 14)
            {
                this.m_BoardSize = 6;
            }

            (sender as Button).Text = string.Format("Board Size: {0}x{0} (click to increase)", this.m_BoardSize);
        }
    }
}