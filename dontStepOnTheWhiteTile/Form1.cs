using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace dontStepOnTheWhiteTile
{
    public partial class Form1 : Form
    {
        /* 
         * Dont Step On The White Tile
         * Harhil Parikh
         * 2013
         **/

            // Globals
        int lowestLine = 4;
        double longRunTick = 10.0f;
        double changeTickCol = -1.0f;
        int boxpass = 0;
        Color fillBoxC = Color.Black;
        Color emptyBoxC = Color.White;
        Color wrongBoxC = Color.Red;
        //Keep it 15 as you can't move blocks at float values
        int smoothness = 15;
        PictureBox[] tiles = new PictureBox[16];

        public Form1()
        {            
            InitializeComponent();
        }

        // Form Initialization
        private void Form1_Load(object sender, EventArgs e)
        {
            newGameButton.Visible = false;
            String basePB = "pictureBox";

            // Create an array of all the PictureBox
            for (int i = 1; i <= 16; i++)
            {
                tiles[i - 1] = (Controls[basePB + i.ToString()] as PictureBox);
            }

            // Create the game
            generateBlock();
        }

        // Color one box in a row black
        private void colourBox(int i)
        {
            int randomNumber;
            Random random = new Random();

            // Make random box in row black
            randomNumber = random.Next(4);
            tiles[i * 4 + randomNumber].BackColor = fillBoxC;   
            
            // Place start text
            if (i == 0)
            {
                startLabel.Location = new Point
                    (randomNumber * tiles[0].Size.Width,
                    startLabel.Location.Y);
            }      
        }

        // Color each row with one unique black box
        private void generateBlock()
        {
            for (int i = 0; i < 4; i++)
            {
                colourBox(i);
            }
        }

        // What happends when user clicks the correct black box
        private void moveDown()
        {
            // Initial box moving
            if (boxpass == 0)
            {
                longRunTimer.Start();
                boxesPassed.Visible = true;
                startLabel.Visible = false;
            }

            // Add to the score and display
            boxpass += 1;
            boxesPassed.Text = boxpass.ToString();

            // If score is a multiple of 30 add 10 sec to the clock
            if (boxpass % 30 == 0)
            {
                longRunTick += 10.0f;
                changeTickCol = longRunTick - 1.0f;
                Timer.ForeColor = Color.Green;
            }

            // Animation of the boxes moving
            for (int f = 0; f < smoothness; f++)
            {
                for (int i = 1; i <= 16; i++)
                {
                    tiles[i - 1].Top = tiles[i - 1].Location.Y + 135 / smoothness;
                }
            }

            // Bottom row is now moved to the top row
            for (int i = (lowestLine*4)-3; i <= (lowestLine * 4); i++)
            {
                tiles[i - 1].Top = 0;
                tiles[i - 1].BackColor = emptyBoxC;
            }

            // Make this new top row have a different black box
            colourBox(lowestLine - 1);
        }

        // What happens when the game ends
        private void endGameBoard()
        {
            // Make the whole grid red
            for (int i = 1; i <= 16; i++)
            {
                tiles[i - 1].BackColor = wrongBoxC;
            }

            // Stop the clock
            longRunTimer.Stop();
            longRunTimer.Equals(0.0);

            // Show the new game button
            newGameButton.Visible = true;

        }


        // Check if the box clicked is the correct one
        private void checkBox(PictureBox tile)
        {
            
            int picBoxNum = Int32.Parse(Regex.Match(tile.Name, @"\d+").Value);
            while (picBoxNum % 4 != 0)
            {
                picBoxNum++;
            }

            // Is the box chosen the correct one?
            if (tile.BackColor == fillBoxC && picBoxNum / 4 == lowestLine)
            {

                // if so move the grid down
                moveDown();
                if (lowestLine == 1)
                {
                    lowestLine = 4;
                }
                else
                {
                    lowestLine -= 1;
                }
            }
            // Otherwise show the end game board
            else
            {
                endGameBoard();
            }
        }


        // When a PictureBox is clicked this method calls checkBox(PictureBox tile)
        private void Tile_Click(object sender, EventArgs e)
        {
            checkBox(sender as PictureBox);
        }

        // The constant running timer
        private void longRunTimer_Tick(object sender, EventArgs e)
        {
            // Update to new time
            longRunTick = Math.Round(longRunTick - 0.1f, 1);

            // If the player runs out of time
            if (longRunTick == 0)
            {
                endGameBoard();
            }
            // To change timer color back to white when it is green due to extra time being granted
            else if (changeTickCol == longRunTick)
            {
                Timer.ForeColor = Color.White;
            }

            // Update timer text
            Timer.Text = longRunTick.ToString() + "\"";
        }


        // Creating a new game
        private void NewGame_Click(object sender, EventArgs e)
        {

            // Reset statistics
            boxpass = 0;
            longRunTick = 10.0f;

            // Make whole board white again
            for (int i = 1; i <= 16; i++)
            {
                tiles[i - 1].BackColor = emptyBoxC;
            }

            // Reset labels
            Timer.Text = longRunTick.ToString() + "\"";
            boxesPassed.Text = boxpass.ToString();
            startLabel.Visible = true;


            // Draw the new tiles
            generateBlock();

            // Remove the new game button
            newGameButton.Visible = false;

        }
    }
}
