using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchGame
{
    public partial class Form1 : Form
    {
        Rectangle hero = new Rectangle(280, 540, 40, 10);
        int heroSpeed = 10;

        List<Rectangle> balls = new List<Rectangle>();
        List<int> ballsSpeed = new List<int>();
        List<string> ballColour = new List<string>();

        int ballSize = 10;
        int ballSpeed = 10;

        int score = 0;
        int time = 500;

        bool leftDown = false;
        bool rightDown = false;

        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        Random randGen = new Random();
        int randValue = 0;

        int groundHeight = 50;

        string gameState = "waiting";
        public Form1()
        {
            InitializeComponent();


        }
        public void GameSetup() 
        {
            gameState = "running";

            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameLoop.Enabled = true;
            time = 500;
            score = 0;

            hero.X = 280;

            balls.Clear();
            ballsSpeed.Clear();
            ballColour.Clear();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                case Keys.Space:
                    if(gameState == "waiting")
                    {
                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }

        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //move player
            if (leftDown == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }

            if (rightDown == true && hero.X < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            //move ball objects
            for (int i = 0; i < balls.Count; i++)
            {
                int y =  balls[i].Y + ballsSpeed[i];
                balls[i] = new Rectangle(balls[i].X, y, ballSize, ballSize);
            }

            //generate random value
            randValue = randGen.Next(1, 101);


            if (randValue < 3)
            {
                balls.Add(new Rectangle(randGen.Next(0, this.Width - ballSize), 0, ballSize, ballSize));
                ballColour.Add("yellow");
                ballsSpeed.Add(12);

            }

            else if (randValue < 10)
            {
                balls.Add(new Rectangle(randGen.Next(0, this.Width - ballSize), 0, ballSize, ballSize));
                ballColour.Add("red");
                ballsSpeed.Add(16);

            }

            else if (randValue <= 20)
            {
                balls.Add(new Rectangle(randGen.Next(0, this.Width - ballSize), 0, ballSize, ballSize));
                ballColour.Add("green");
                ballsSpeed.Add(8);
  
            }


            //remove ball if goes off screen

            for (int i = 0; i < balls.Count; i++)
            {
                if(balls[i].Y >= this.Height)
                {
                    balls.RemoveAt(i);
                    ballsSpeed.RemoveAt(i);
                    ballColour.RemoveAt(i);
                }
            }

            // check for collision between any ball and player

            for(int i = 0; i < balls.Count; i++)
            {
                if (hero.IntersectsWith(balls[i]))
                {
                    if (ballColour[i] == "green")
                    {
                        score += 5;
                    }
                    else if (ballColour[i] == "yellow")
                    {
                        time += 250;
                    }
                    if (ballColour[i] == "red")
                    {
                        score -= 50;
                    }

                    balls.RemoveAt(i);
                    ballsSpeed.RemoveAt(i);
                    ballColour.RemoveAt(i);

                }
            }

            //decrease time
            time--;

            if (time <= 0)
            {
                gameLoop.Enabled = false;
                gameState = "over";
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Catch Game";
                subtitleLabel.Text = "Press Space to start or Esc to exit";
            }
            else if (gameState == "running")
            {
                //update labels
                timeLabel.Text = $"{time}";
                scoreLabel.Text = $"Score: {score}";

                //draw ground
                e.Graphics.FillRectangle(greenBrush, 0, this.Height - groundHeight,
                    this.Width, groundHeight);

                //draw hero
                e.Graphics.FillRectangle(whiteBrush, hero);

                //draw balls
                for (int i = 0; i < balls.Count(); i++)
                {
                    if (ballColour[i] == "green")
                    {
                        e.Graphics.FillEllipse(greenBrush, balls[i]);
                    }
                    else if (ballColour[i] == "red")
                    {
                        e.Graphics.FillEllipse(redBrush, balls[i]);
                    }
                    else if (ballColour[i] == "yellow")
                    {
                        e.Graphics.FillEllipse(yellowBrush, balls[i]);
                    }

                }
            }
            else if (gameState == "over")
            {
                titleLabel.Text = "Catch Game";
                subtitleLabel.Text = "Press Space to start or Esc to exit";
            }

        }
    }
}
