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
    public partial class GameForm : Form
    {
        private static int          m_NumberOfGames = 1;
        private int                 m_BoardSize = 0;
        private Player[]            m_PlayerArray = new Player[2];
        private bool                m_ReRun = false;

        public GameForm()
        {
            this.InitializeComponent();
        }

        public int BoardSize
        {
            get { return this.m_BoardSize; }
            set { this.m_BoardSize = value; }
        }

        public bool ReRun
        {
            get { return this.m_ReRun; }
            set { this.m_ReRun = value; }
        }

        public Player[] PlayerArray
        {
            get { return this.m_PlayerArray; }
            set { this.m_PlayerArray = value; }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {       
            this.ResizeBoard(this.m_BoardSize);

            if (this.tableLayoutPanel1.Controls.Count != 0)
            {
                this.ClearCurrentGameBoard();
            }

            for (int i = 0; i < this.m_BoardSize; i++)
            {
                for (int j = 0; j < this.m_BoardSize; j++)
                {
                    this.tableLayoutPanel1.Controls.Add(this.SetButton(), i, j);   
                }
            }

            this.SetPlayerPosition();
            this.DrawCurrentPlayerValidMoves();
        }

        private void ClearCurrentGameBoard()
        {
            this.tableLayoutPanel1.Controls.Clear();
            this.PlayerArray[0].m_ValidMoves.Clear();
            this.PlayerArray[1].m_ValidMoves.Clear();
            BoardState.m_BlackPositionList.Clear();
            BoardState.m_WhitePositionList.Clear();
            BoardState.InitDemiBoard(this.m_BoardSize);
            BoardState.CheckOptionalMoves(this.PlayerArray[0].m_ValidMoves, this.PlayerArray[1].m_ValidMoves);
        }

        private void DrawCurrentPlayerValidMoves()
        {
            if (this.m_PlayerArray[0].IsMyTurn)
            {
                this.DrawValidMoves(this.m_PlayerArray[0].m_ValidMoves);
            }
            else
            {
                this.DrawValidMoves(this.m_PlayerArray[1].m_ValidMoves);
            }
        }

        private void DrawValidMoves(LinkedList<MoveOption> i_ValidMoves)
        {
            Point optionalMoveToDraw = new Point();

            foreach (MoveOption moveOption in i_ValidMoves)
            {
               optionalMoveToDraw = moveOption.PossibleMove;
               this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].Enabled = true;
               this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].BackColor = Color.Green;
               this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].Click += this.GameForm_Click;
            }
        }

        private void RemoveValidMoves(LinkedList<MoveOption> i_ValidMoves)
        {
            Point optionalMoveToDraw = new Point();

            foreach (MoveOption moveOption in i_ValidMoves)
            {
                optionalMoveToDraw = moveOption.PossibleMove;
                this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].Enabled = false;
                this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].BackColor = SystemColors.Control;
                this.tableLayoutPanel1.Controls[(this.m_BoardSize * optionalMoveToDraw.Y) + optionalMoveToDraw.X].Click -= this.GameForm_Click;
            }
        }

        private void GameForm_Click(object sender, EventArgs e)
        {
            int buttonCellLocation = (sender as Button).TabIndex;
            Point currentPositionInDemiBoard = new Point();
            currentPositionInDemiBoard.X = buttonCellLocation % this.m_BoardSize;
            currentPositionInDemiBoard.Y = (buttonCellLocation - currentPositionInDemiBoard.X) / this.m_BoardSize;
            MoveOption moveOption = new MoveOption(new Point());
            bool isPointInList;

            if (this.m_PlayerArray[0].IsMyTurn)
            {
                isPointInList = InputManager.IsInputInListOfPossibleMoves(this.m_PlayerArray[0].m_ValidMoves, currentPositionInDemiBoard, ref moveOption);
                InputManager.UpdateBoard(moveOption, this.m_PlayerArray[0].CoinColor);
            }
            else
            {
                isPointInList = InputManager.IsInputInListOfPossibleMoves(this.m_PlayerArray[1].m_ValidMoves, currentPositionInDemiBoard, ref moveOption);
                InputManager.UpdateBoard(moveOption, this.m_PlayerArray[1].CoinColor);
            }

            this.UpdateGameBoardForm(BoardState.m_PlayerPositionsInDemiBoard);

            if (this.PlayerArray[1].UserType == Player.eUserType.Computer && this.PlayerArray[1].IsMyTurn)
            {
                this.ComputerMoveManager();
            }

            this.CheckIfGameOver();
        }

        private void CheckIfGameOver()
        {
            // if both players lists are empty, open the game over form (need to check who won ==> who has more coins ==> who has a bigger score) 
            if (this.PlayerArray[0].m_ValidMoves.Count == 0 && this.PlayerArray[1].m_ValidMoves.Count == 0)
            {
                this.GameOverForm();
            }
            else if ((this.PlayerArray[0].m_ValidMoves.Count == 0 && this.PlayerArray[0].IsMyTurn) || (this.PlayerArray[1].m_ValidMoves.Count == 0 && this.PlayerArray[1].IsMyTurn))
            {
                this.SwitchTurn();
                if ((this.PlayerArray[1].UserType != Player.eUserType.Computer && this.PlayerArray[1].IsMyTurn) || this.PlayerArray[0].IsMyTurn)
                {
                    this.DrawCurrentPlayerValidMoves();
                }

                if (this.PlayerArray[1].UserType == Player.eUserType.Computer && this.PlayerArray[1].IsMyTurn)
                {
                    this.ComputerMoveManager();
                }

                if (this.PlayerArray[0].m_ValidMoves.Count == 0 && this.PlayerArray[1].m_ValidMoves.Count == 0)
                {
                    this.GameOverForm();
                }
            }
        }

        private void ComputerMoveManager()
        {
            MoveOption computerBestOptionMove = new MoveOption();

            computerBestOptionMove = InputManager.FindMaxDamage(this.PlayerArray[1].m_ValidMoves);
            InputManager.UpdateBoard(computerBestOptionMove, this.m_PlayerArray[1].CoinColor);
            this.UpdateGameBoardForm(BoardState.m_PlayerPositionsInDemiBoard);
        }

        private void GameOverForm()
        {
            string gameOverMessage;
            object[] msgArray = new object[6];
            DialogResult chosenButton;
            MessageBoxButtons yesNoButtons;

            if (BoardState.m_BlackPositionList.Count > BoardState.m_WhitePositionList.Count)
            {
                this.PlayerArray[0].NumberOfWins++;
                msgArray[0] = "Red";
                msgArray[1] = BoardState.m_BlackPositionList.Count;
                msgArray[2] = BoardState.m_WhitePositionList.Count;
                msgArray[3] = this.PlayerArray[0].NumberOfWins;
            }
            else
            {
                this.PlayerArray[1].NumberOfWins++;
                msgArray[0] = "Yellow";
                msgArray[1] = BoardState.m_WhitePositionList.Count;
                msgArray[2] = BoardState.m_BlackPositionList.Count;
                msgArray[3] = this.PlayerArray[1].NumberOfWins;
            }

            msgArray[4] = m_NumberOfGames;
            msgArray[5] = Environment.NewLine;

            gameOverMessage = string.Format("{0} Won!! ({1}/{2}) ({3}/{4}){5}Would you like another round?", msgArray);
            yesNoButtons = MessageBoxButtons.YesNo;

            chosenButton = MessageBox.Show(gameOverMessage, "Othello", yesNoButtons);
            if (chosenButton == DialogResult.No)
            {
                this.Close();
            }
            else if (chosenButton == DialogResult.Yes)
            {
                m_NumberOfGames++;
                this.m_ReRun = true;
                this.Close();
            }
        }

        private void UpdateGameBoardForm(char[,] i_DemiBoard)
        {
            for (int i = 0; i < i_DemiBoard.GetLength(0); i++)
            {
                for (int j = 0; j < i_DemiBoard.GetLength(0); j++)
                {
                    if (i_DemiBoard[i, j] == 0)
                    {
                        this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].BackColor = SystemColors.Control;
                    }
                    else if (i_DemiBoard[i, j] == 'X')
                    {
                        this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].BackgroundImage = Properties.Resources.CoinRed;
                    }
                    else if (i_DemiBoard[i, j] == 'O')
                    {
                        this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].BackgroundImage = Properties.Resources.CoinYellow;
                    }

                    if (this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].BackColor == Color.Green)
                    {
                        this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].BackColor = SystemColors.Control;
                    }

                    this.tableLayoutPanel1.Controls[(this.m_BoardSize * j) + i].Enabled = false;
                }
            }

            this.SwitchTurn();
            this.RemoveValidMoves(this.PlayerArray[0].m_ValidMoves);
            this.RemoveValidMoves(this.PlayerArray[1].m_ValidMoves);

            this.PlayerArray[0].m_ValidMoves.Clear();
            this.PlayerArray[1].m_ValidMoves.Clear();

            BoardState.CheckOptionalMoves(this.PlayerArray[0].m_ValidMoves, this.PlayerArray[1].m_ValidMoves);

            if (this.PlayerArray[1].UserType != Player.eUserType.Computer || !this.PlayerArray[1].IsMyTurn)
            {
                this.DrawCurrentPlayerValidMoves();
            }
        }

        private void SwitchTurn()
        {
            if (this.m_PlayerArray[0].IsMyTurn)
            {
                this.m_PlayerArray[0].IsMyTurn = false;
                this.m_PlayerArray[1].IsMyTurn = true;
                this.Text = "Othello - Yellow's Turn";
            }
            else
            {
                this.m_PlayerArray[0].IsMyTurn = true;
                this.m_PlayerArray[1].IsMyTurn = false;
                this.Text = "Othello - Red's Turn";
            }
        }

        private void ResizeBoard(int i_BoardSize)
        {
            this.tableLayoutPanel1.RowCount = this.m_BoardSize;
            this.tableLayoutPanel1.ColumnCount = this.m_BoardSize;

            switch (i_BoardSize)
            {
                case (int)GameLogic.BoardState.eBoardSizeOption.Six:
                    this.ClientSize = new System.Drawing.Size(277, 277);
                    break;
                case (int)GameLogic.BoardState.eBoardSizeOption.Eight:
                    this.ClientSize = new System.Drawing.Size(370, 370);
                    break;
                case (int)GameLogic.BoardState.eBoardSizeOption.Ten:
                    this.ClientSize = new System.Drawing.Size(460, 460);
                    break;
                case (int)GameLogic.BoardState.eBoardSizeOption.Twelve:
                    this.ClientSize = new System.Drawing.Size(555, 555);
                    break;

                default:
                    break;
            }
        }

        private Button SetButton()
        {
            Button button = new Button();
            button.Width = 40;
            button.Height = 40;
            button.Enabled = false;
            button.BackColor = SystemColors.Control;
            return button;
        }

        private void SetPlayerPosition()
        {
            this.tableLayoutPanel1.Controls[(((this.m_BoardSize / 2) - 1) * this.m_BoardSize) + (this.m_BoardSize / 2) - 1].BackgroundImage = Properties.Resources.CoinYellow;
            this.tableLayoutPanel1.Controls[(((this.m_BoardSize / 2) - 1) * this.m_BoardSize) + (this.m_BoardSize / 2)].BackgroundImage = Properties.Resources.CoinRed;
            this.tableLayoutPanel1.Controls[((this.m_BoardSize / 2) * this.m_BoardSize) + (this.m_BoardSize / 2) - 1].BackgroundImage = Properties.Resources.CoinRed;
            this.tableLayoutPanel1.Controls[((this.m_BoardSize / 2) * this.m_BoardSize) + (this.m_BoardSize / 2)].BackgroundImage = Properties.Resources.CoinYellow;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}