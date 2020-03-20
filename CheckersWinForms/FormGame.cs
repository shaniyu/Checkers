using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersWinForms
{
    public class FormGame : Form
    {
        private const int k_ButtonSize = 40;
        private const int k_ButtonHeightLocation = 50;
        private const int k_ButtonWidthLocation = 10;
        private readonly GameSettings r_GameSettings = new GameSettings();
        private readonly Label labelPlayer1 = new Label();
        private readonly Label labelPlayer2 = new Label();
        private readonly int r_BoardSize;
        private readonly Button[,] r_ButtonsBoard;
        private readonly GameLogic r_GameLogic = new GameLogic();
        private readonly Player r_Player1 = new Player(ePlayer.Player1);
        private readonly Player r_Player2 = new Player(ePlayer.Player2);
        private readonly Move r_NextMove = new Move(0, 0, 0, 0);
        private readonly List<string> r_Errors = new List<string>(4);
        private readonly Move r_PrevMove = new Move(0, 0, 0, 0);
        private bool m_CanPerformAnotherMoveInCurrentTurn;
        private Button clickedButton = null;
        private Player m_CurrentTurn;

        public FormGame()
        {
            r_GameSettings.ShowDialog();
            this.StartPosition = FormStartPosition.CenterScreen;
            initializeErrors();
            r_BoardSize = r_GameSettings.BoardSize;
            setFormSize();
            this.BackColor = Color.White;
            this.Text = "Damka";
            r_ButtonsBoard = new Button[r_BoardSize, r_BoardSize];
            initializeButtonBoard();
            setButtonBoard();
            setPlayers();
            initialzeLabels();
            r_GameLogic.StartNewGame(r_BoardSize, r_Player1, r_Player2);
            m_CurrentTurn = r_Player1;
        }

        private void initializeErrors()
        {
            r_Errors.Add("You don't have a soldier in your chosen moving from position");
            r_Errors.Add("Your chosen moving to position is not free");
            r_Errors.Add("Your chosen move to position is not valid");
            r_Errors.Add("Your chosen move is invalid. You have a soldier that can capture an opponent soldier");
        }

        private void initialzeLabels()
        {
            int xLocation;

            this.Controls.Add(labelPlayer1);
            this.Controls.Add(labelPlayer2);
            labelPlayer1.Text = r_Player1.PlayerName + ": " + r_Player1.Score;
            labelPlayer2.Text = r_Player2.PlayerName + ": " + r_Player2.Score;
            labelPlayer1.Size = new Size(80, 20);
            xLocation = this.Size.Width / 6;
            labelPlayer1.Location = new Point(this.Location.X + xLocation, this.Location.Y + 10);
            labelPlayer2.Location = new Point(this.Location.X + (3 * xLocation), this.Location.Y + 10);
        }

        private void setPlayers()
        {
            if (r_GameSettings.DialogResult == DialogResult.Cancel)
            {
                r_Player1.PlayerName = "Player1";
                r_Player2.IsComputer = true;
                r_Player2.PlayerName = "Computer";
            }
            else
            {
                r_Player1.PlayerName = r_GameSettings.Player1Name;
                if (r_GameSettings.IsOpponentComputer)
                {
                    r_Player2.IsComputer = true;
                    r_Player2.PlayerName = "Computer";
                }
                else
                {
                    r_Player2.PlayerName = r_GameSettings.Player2Name;
                }
            }
        }

        private void setFormSize()
        {
            int height, width;
            height = 60 + (k_ButtonSize * r_BoardSize);
            width = 20 + (k_ButtonSize * r_BoardSize);
            this.ClientSize = new Size(width, height);
        }

        private void cleanButtonBoard()
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    r_ButtonsBoard[i, j].Text = string.Empty;
                }
            }
        }

        private void setButtonBoard()
        {
            int trackParityForColorCounter = 0;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (trackParityForColorCounter % 2 != 0)
                    {
                        if (i > this.r_BoardSize / 2)
                        {
                            r_ButtonsBoard[i, j].Text = "X";
                        }
                        else if (i < (this.r_BoardSize / 2) - 1)
                        {
                            r_ButtonsBoard[i, j].Text = "O";
                        }
                    }

                    trackParityForColorCounter++;
                }

                trackParityForColorCounter++;
            }
        }

        private void initializeButtonBoard()
        {
            int buttonHeightLocation = k_ButtonHeightLocation;
            int buttonWidthLocation = k_ButtonWidthLocation;
            int trackParityForColorCounter = 0;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    r_ButtonsBoard[i, j] = new Button();
                    this.Controls.Add(r_ButtonsBoard[i, j]);
                    r_ButtonsBoard[i, j].Size = new Size(k_ButtonSize, k_ButtonSize);
                    r_ButtonsBoard[i, j].Location = new Point(this.Location.X + buttonWidthLocation, this.Location.Y + buttonHeightLocation);
                    if (trackParityForColorCounter % 2 == 0)
                    {
                        r_ButtonsBoard[i, j].BackColor = Color.Gray;
                        r_ButtonsBoard[i, j].Enabled = false;
                    }
                    else
                    {
                        r_ButtonsBoard[i, j].BackColor = Color.White;
                        r_ButtonsBoard[i, j].Click += new EventHandler(boardButton_Click);
                    }

                    trackParityForColorCounter++;
                    buttonWidthLocation += k_ButtonSize;
                }

                buttonWidthLocation = k_ButtonWidthLocation;
                buttonHeightLocation += k_ButtonSize;
                trackParityForColorCounter++;
            }
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            Button currentSquare = sender as Button;
            int errorNum;
            bool isMoveValid;
            bool isGameOver = false;
            bool isTie = false;

            if (currentSquare != clickedButton && clickedButton != null)
            {
                updateMoveToPosition(currentSquare);
                updateButtonColor(clickedButton);
                isMoveValid = r_GameLogic.CheckIfMoveIsValid(r_NextMove, m_CurrentTurn, out errorNum);
                if (m_CanPerformAnotherMoveInCurrentTurn)
                {
                    isMoveValid &= checkValidityOfExtraMove();
                }

                if (!isMoveValid)
                {
                    MessageBox.Show(r_Errors[errorNum], "Invalid move", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    handleValidMove(out isGameOver, out isTie);
                }

                clickedButton = null;
            }
            else if (clickedButton == null)
            {
                updateMoveFromPosition(currentSquare);
                updateButtonColor(currentSquare);
                clickedButton = currentSquare;
            }
            else
            {
                updateButtonColor(currentSquare);
                clickedButton = null;
            }

            if (isGameOver)
            {
                handleGameOver(isTie);
            }
        }

        private void handleValidMove(out bool o_IsGameOver, out bool o_IsTie)
        {
            performMoveAndUpdateCurrentTurnIfNeeded();
            o_IsGameOver = r_GameLogic.CheckIfGameIsOverAndUpdateScore(r_Player1, r_Player2, false, out o_IsTie);
            if (!o_IsGameOver && !m_CanPerformAnotherMoveInCurrentTurn && m_CurrentTurn.IsComputer)
            {
                List<Move> computerMoves;

                computerMoves = r_GameLogic.PlayAsComputer(m_CurrentTurn, r_Player1);
                foreach (Move move in computerMoves)
                {
                    r_NextMove.ColToMoveFrom = move.ColToMoveFrom;
                    r_NextMove.ColToMoveTo = move.ColToMoveTo;
                    r_NextMove.RowToMoveFrom = move.RowToMoveFrom;
                    r_NextMove.RowToMoveTo = move.RowToMoveTo;
                    updateButtonsBoard();
                }

                o_IsGameOver = r_GameLogic.CheckIfGameIsOverAndUpdateScore(r_Player1, r_Player2, false, out o_IsTie);
                m_CurrentTurn = r_Player1;
            }
        }

        private void handleGameOver(bool i_IsTie)
        {
            string currentlyWinningPlayerName = getCurrentlyWinningPlayerName();

            if (i_IsTie)
            {
                if (MessageBox.Show("Tie!" + Environment.NewLine + "Another Round?", "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cleanButtonBoard();
                    setButtonBoard();
                    r_GameLogic.StartNewGame(r_BoardSize, r_Player1, r_Player2);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                if (MessageBox.Show(currentlyWinningPlayerName + " Won!" + Environment.NewLine + "Another Round?", "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    labelPlayer1.Text = r_Player1.PlayerName + ": " + r_Player1.Score;
                    labelPlayer2.Text = r_Player2.PlayerName + ": " + r_Player2.Score;
                    cleanButtonBoard();
                    setButtonBoard();
                    r_GameLogic.StartNewGame(r_BoardSize, r_Player1, r_Player2);
                }
                else
                {
                    this.Close();
                }
            }

            m_CurrentTurn = r_Player1;
        }

        private string getCurrentlyWinningPlayerName()
        {
            string currentlyWinningPlayerName;

            if (r_Player1.IsCurrentlyWinning)
            {
                currentlyWinningPlayerName = r_Player1.PlayerName;
            }
            else
            {
                currentlyWinningPlayerName = r_Player2.PlayerName;
            }

            return currentlyWinningPlayerName;
        }

        private bool checkValidityOfExtraMove()
        {
            bool isExtraMoveValid;

            isExtraMoveValid = r_NextMove.ColToMoveFrom == r_PrevMove.ColToMoveTo && r_NextMove.RowToMoveFrom == r_PrevMove.RowToMoveTo;

            return isExtraMoveValid;
        }

        private void performMoveAndUpdateCurrentTurnIfNeeded()
        {
            Player opponent = whichPlayerDoesntHaveCurrentTurn();
            Move nextValidMove;

            m_CanPerformAnotherMoveInCurrentTurn = r_GameLogic.PerformTheChosenMoveAndCheckIfCanPerformAnotherMoveInCurrentTurn(r_NextMove, m_CurrentTurn, opponent, out nextValidMove);
            updateButtonsBoard();
            if (m_CanPerformAnotherMoveInCurrentTurn)
            {
                MessageBox.Show(string.Format("{0} can perform another move", m_CurrentTurn.PlayerName), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                r_PrevMove.ColToMoveFrom = r_NextMove.ColToMoveFrom;
                r_PrevMove.ColToMoveTo = r_NextMove.ColToMoveTo;
                r_PrevMove.RowToMoveTo = r_NextMove.RowToMoveTo;
                r_PrevMove.RowToMoveFrom = r_NextMove.RowToMoveFrom;
            }
            else
            {
                updateCurrentTurn();
            }
        }

        private void updateButtonsBoard()
        {
            char sign;

            r_ButtonsBoard[r_NextMove.RowToMoveFrom, r_NextMove.ColToMoveFrom].Text = string.Empty;
            if ((r_NextMove.ColToMoveTo - r_NextMove.ColToMoveFrom) % 2 == 0)
            {
                int capturedSoldierRow, capturedSoldierCol;

                capturedSoldierCol = r_NextMove.ColToMoveFrom + ((r_NextMove.ColToMoveTo - r_NextMove.ColToMoveFrom) / 2);
                capturedSoldierRow = r_NextMove.RowToMoveFrom + ((r_NextMove.RowToMoveTo - r_NextMove.RowToMoveFrom) / 2);
                r_ButtonsBoard[capturedSoldierRow, capturedSoldierCol].Text = string.Empty;
            }

            if (r_GameLogic.GetGameBoard()[r_NextMove.RowToMoveTo, r_NextMove.ColToMoveTo] != null)
            {
                sign = r_GameLogic.GetGameBoard()[r_NextMove.RowToMoveTo, r_NextMove.ColToMoveTo].SoldierSign;
                r_ButtonsBoard[r_NextMove.RowToMoveTo, r_NextMove.ColToMoveTo].Text = sign.ToString();
            }
        }

        private void updateCurrentTurn()
        {
            m_CurrentTurn = whichPlayerDoesntHaveCurrentTurn();
        }

        private Player whichPlayerDoesntHaveCurrentTurn()
        {
            Player doesntHaveCurrentTurn;
            if (m_CurrentTurn.WhichPlayerAmI == ePlayer.Player1)
            {
                doesntHaveCurrentTurn = r_Player2;
            }
            else
            {
                doesntHaveCurrentTurn = r_Player1;
            }

            return doesntHaveCurrentTurn;
        }

        private void updateMoveToPosition(Button i_CurrentSquare)
        {
            int rowToMoveTo, colToMoveTo;
            rowToMoveTo = (i_CurrentSquare.Location.Y - k_ButtonHeightLocation) / k_ButtonSize;
            colToMoveTo = (i_CurrentSquare.Location.X - k_ButtonWidthLocation) / k_ButtonSize;
            r_NextMove.ColToMoveTo = colToMoveTo;
            r_NextMove.RowToMoveTo = rowToMoveTo;
        }

        private void updateButtonColor(Button i_CurrentSquare)
        {
            if (i_CurrentSquare.BackColor == Color.LightBlue)
            {
                i_CurrentSquare.BackColor = Color.White;
            }
            else
            {
                i_CurrentSquare.BackColor = Color.LightBlue;
            }
        }

        private void updateMoveFromPosition(Button i_CurrentSquare)
        {
            int rowToMoveFrom, colToMoveFrom;
            rowToMoveFrom = (i_CurrentSquare.Location.Y - k_ButtonHeightLocation) / k_ButtonSize;
            colToMoveFrom = (i_CurrentSquare.Location.X - k_ButtonWidthLocation) / k_ButtonSize;
            r_NextMove.ColToMoveFrom = colToMoveFrom;
            r_NextMove.RowToMoveFrom = rowToMoveFrom;
        }
    }
}
