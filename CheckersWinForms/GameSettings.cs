using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersWinForms
{
    public class GameSettings : Form
    {
        private const int k_MaxNameLength = 20;
        private const int k_BoardSize6 = 6;
        private const int k_BoardSize8 = 8;
        private const int k_BoardSize10 = 10;
        private readonly Label labelBoardSize = new Label();
        private readonly Label labelPlayers = new Label();
        private readonly Label labelPlayer1 = new Label();
        private readonly Button buttonDone = new Button();
        private readonly TextBox textBoxPlayer1Name = new TextBox();
        private readonly TextBox textBoxPlayer2Name = new TextBox();
        private readonly CheckBox checkBoxDoesWantPlayer2 = new CheckBox();
        private readonly RadioButton radioButtonBoardSize6 = new RadioButton();
        private readonly RadioButton radioButtonBoardSize8 = new RadioButton();
        private readonly RadioButton radioButtonBoardSize10 = new RadioButton();

        public GameSettings()
        {
            initializeForm();
            initializeLabels();
            initializeCheckBoxes();
            initializeTextBoxes();
            initializeButton();
            initializeRadioButtons();
            initializeCheckBoxes();
            checkBoxDoesWantPlayer2.CheckedChanged += new EventHandler(checkBoxDoesWantPlayer2_CheckedChanged);
            buttonDone.Click += new EventHandler(buttonDone_Click);
        }

        private void initializeForm()
        {
            this.Controls.AddRange(new Control[] { checkBoxDoesWantPlayer2, labelBoardSize, labelPlayers, labelPlayer1, textBoxPlayer2Name, textBoxPlayer1Name, buttonDone, radioButtonBoardSize6, radioButtonBoardSize8, radioButtonBoardSize10 });
            this.Text = "GameSettings";
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Size = new Size(270, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void initializeLabels()
        {
            labelBoardSize.Text = "Board Size:";
            labelBoardSize.Location = new Point(this.Location.X + 10, this.Location.Y + 20);
            labelPlayers.Text = "Players:";
            labelPlayers.Location = new Point(labelBoardSize.Left, labelBoardSize.Top + 50);
            labelPlayer1.Text = "Player 1:";
            labelPlayer1.Location = new Point(labelPlayers.Left + 10, labelPlayers.Top + 25);
        }

        private void initializeButton()
        {
            buttonDone.Text = "Done";
            buttonDone.Location = new Point(textBoxPlayer1Name.Left + 25, textBoxPlayer2Name.Top + 54);
        }

        private void initializeTextBoxes()
        {
            textBoxPlayer2Name.Text = "[Computer]";
            textBoxPlayer2Name.Enabled = false;
            textBoxPlayer2Name.Location = new Point(labelPlayer1.Left + 115, checkBoxDoesWantPlayer2.Top + 2);
            textBoxPlayer1Name.Location = new Point(textBoxPlayer2Name.Left, labelPlayer1.Top - 3);
        }

        private void initializeRadioButtons()
        {
            radioButtonBoardSize6.Text = "6 x 6";
            radioButtonBoardSize6.Location = new Point(labelPlayer1.Left, labelBoardSize.Top + 20);
            radioButtonBoardSize6.Size = new Size(50, 20);
            radioButtonBoardSize6.Checked = true;
            radioButtonBoardSize8.Text = "8 x 8";
            radioButtonBoardSize8.Location = new Point(radioButtonBoardSize6.Left + 60, radioButtonBoardSize6.Top);
            radioButtonBoardSize8.Size = new Size(60, 20);
            radioButtonBoardSize10.Text = "10 x 10";
            radioButtonBoardSize10.Location = new Point(radioButtonBoardSize8.Left + 60, radioButtonBoardSize6.Top);
            radioButtonBoardSize10.Size = new Size(60, 20);
        }

        private void initializeCheckBoxes()
        {
            checkBoxDoesWantPlayer2.Enabled = true;
            checkBoxDoesWantPlayer2.Text = "Player 2:";
            checkBoxDoesWantPlayer2.Location = new Point(labelPlayer1.Left, labelPlayer1.Top + 20);
        }

        private void checkBoxDoesWantPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPlayer2Name.Enabled = checkBoxDoesWantPlayer2.Checked;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            bool isPlayer1NameValid;
            bool isPlayer2NameValid = true;

            isPlayer1NameValid = checkIfTextBoxTextIsValidAndHandleInvalidText(textBoxPlayer1Name.Text, 1);
            if (checkBoxDoesWantPlayer2.Checked && isPlayer1NameValid)
            {
                isPlayer2NameValid = checkIfTextBoxTextIsValidAndHandleInvalidText(textBoxPlayer2Name.Text, 2);
            }

            if (isPlayer1NameValid && isPlayer2NameValid)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        ////The name validations are according to the name validations in exercise 2
        private bool checkIfTextBoxTextIsValidAndHandleInvalidText(string i_PlayersName, int i_PlayersNumber)
        {
            bool isNameValid;
            string player = "Player " + i_PlayersNumber;

            if (i_PlayersName == string.Empty)
            {
                MessageBox.Show(player + " name cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isNameValid = false;
            }
            else if (i_PlayersName.Length > k_MaxNameLength)
            {
                MessageBox.Show(player + " name is too long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isNameValid = false;
            }
            else if (doesNameContainSpace(i_PlayersName))
            {
                MessageBox.Show(player + " name cannot contain spaces", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isNameValid = false;
            }
            else
            {
                isNameValid = true;
            }

            return isNameValid;
        }

        private bool doesNameContainSpace(string i_Name)
        {
            bool doesNameContainSpace = false;

            foreach (char ch in i_Name)
            {
                if (ch == ' ')
                {
                    doesNameContainSpace = true;
                }
            }

            return doesNameContainSpace;
        }

        public string Player1Name
        {
            get
            {
                return textBoxPlayer1Name.Text;
            }
        }

        public bool IsOpponentComputer
        {
            get
            {
                return !checkBoxDoesWantPlayer2.Checked;
            }
        }

        public string Player2Name
        {
            get
            {
                return textBoxPlayer2Name.Text;
            }
        }

        public int BoardSize
        {
            get
            {
                int boardSize;
                if (radioButtonBoardSize6.Checked)
                {
                    boardSize = k_BoardSize6;
                }
                else if (radioButtonBoardSize8.Checked)
                {
                    boardSize = k_BoardSize8;
                }
                else
                {
                    boardSize = k_BoardSize10;
                }

                return boardSize;
            }
        }
    }
}
