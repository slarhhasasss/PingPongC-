using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPongForTwo
{

    public partial class Form1 : Form
    {
        //Скорости мячика
        private int speed_vertical_ball = 3;
        private int speed_horizontal_ball = 3;
        //счет слева и справа
        private int score_left = 0;
        private int score_right = 0;
        //скорости передвижения панелек
        private int speed_right_panel = 5;
        private int speed_left_panel = 5;

        //определители, была ли нажата  та или иная клавиша
        private int iAPBtn_l_ctrl = 0;
        private int iAPBtn_r_ctrl = 0;
        private int iAPBtn_l_shift = 0;
        private int iAPBtn_r_shift = 0;

        public Form1()
        {
            InitializeComponent();

            //запускаем таймер
            //gameTimer.Enabled = true;

            //Прячем курсор в окне
            //Cursor.Hide();

            //Устанавливаем отсутствие рамок
            this.FormBorderStyle = FormBorderStyle.None;
            
            //наше приложение будет выводиться поверх всех остальных
            this.TopMost = true;

            //Устанавливаем размер окна на весь экран
            Bounds = Screen.PrimaryScreen.Bounds;

            //Устанавливаем, что нельзя трогать это поле, оно будет изменяться программно
            textBoxScore.Enabled = false;

            //задаем положение правой и левой панельки
            rightPictureBox.Left = gameField.Right - 50;
            leftPictureBox.Left = 25;

            //Положение кнопки
            btnStart.Left = gameField.Width / 2 - 50;
            btnExit.Left = btnStart.Right + 10;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                //выходим из прилолжения
                case Keys.Escape:
                    this.Close();
                    break;
                //прописываем действия при нажатии на клавиши
                case Keys.Z:
                    iAPBtn_l_ctrl = 1;
                    break;
                case Keys.M:
                    iAPBtn_r_ctrl = 1;
                    break;
                case Keys.A:
                    iAPBtn_l_shift = 1;
                    break;
                case Keys.K:
                    iAPBtn_r_shift = 1;
                    break;
                case Keys.Enter:
                    startGame();
                    break;
            }
        }


        //Функция таймер
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //отслеживаем изменение положения наших панелей для отбивания мячика
            changePositionOfPannels();


            //movement of ball:
            ball.Left += speed_horizontal_ball;
            ball.Top += speed_vertical_ball;

            //если касается верха или низа, то изменяет направление
            if(ball.Top >= gameField.Top)
            {
                speed_vertical_ball *= -1;
            }

            if (ball.Bottom <= gameField.Bottom)
            {
                speed_vertical_ball *= -1;
            }

            //Если касается экрана слева
            if(ball.Left <= gameField.Left)
            {
                score_right += 1;
                stopGame();
            }
            //Right
            if(ball.Right >= gameField.Right)
            {
                score_left += 1;
                stopGame();
            }

            //отслеживаем касания наших панелей для отбивания мяча
            if(ball.Right <= rightPictureBox.Right && ball.Right >= rightPictureBox.Left 
                && ball.Top >= rightPictureBox.Top && ball.Bottom <= rightPictureBox.Bottom)
            {
                speed_horizontal_ball *= -1;

                //Change background to random color
                Random randColor = new Random();
                gameField.BackColor = Color.FromArgb(randColor.Next(150, 255), randColor.Next(150, 255), randColor.Next(150, 255));
            }
            if(ball.Left >= leftPictureBox.Left && ball.Left <= leftPictureBox.Right
                && ball.Top >= leftPictureBox.Top && ball.Bottom <= leftPictureBox.Bottom)
            {
                //Меняем его направление
                speed_horizontal_ball *= -1;
                //Ускоряем мячик
                if (speed_horizontal_ball < 0) speed_horizontal_ball--;
                else speed_horizontal_ball++;

                if (speed_vertical_ball < 0) speed_vertical_ball--;
                else speed_vertical_ball++;
                
                //Change background to random color
                Random randColor = new Random();
                gameField.BackColor = Color.FromArgb(randColor.Next(150, 255), randColor.Next(150, 255), randColor.Next(150, 255));
            }

        }

        private void stopGame()
        {
            gameTimer.Enabled = false;
            textBoxScore.Text = $"{score_left} : {score_right}";
            btnStart.Enabled = true;
            btnStart.Visible = true;
            Cursor.Show();
            btnExit.Enabled = true;
            btnExit.Visible = true;

            speed_horizontal_ball = 3 * speed_horizontal_ball / Math.Abs(speed_horizontal_ball);
            speed_vertical_ball = 3 * speed_vertical_ball / Math.Abs(speed_vertical_ball);

            speed_horizontal_ball *= -1;
        }

        //Изменяем позиции наших панелей для отбивания
        private void changePositionOfPannels()
        {
            if(iAPBtn_l_ctrl == 1)
            {
                if(leftPictureBox.Bottom - speed_left_panel <= gameField.Bottom)
                    leftPictureBox.Top += speed_left_panel;
                //iAPBtn_l_ctrl = 0;
            }
            if(iAPBtn_r_ctrl == 1)
            {
                if (rightPictureBox.Bottom - speed_right_panel <= gameField.Bottom)
                    rightPictureBox.Top += speed_right_panel;
                //iAPBtn_r_ctrl = 0;
            }
            if(iAPBtn_l_shift == 1)
            {
                if (leftPictureBox.Top + speed_left_panel >= gameField.Top)
                    leftPictureBox.Top -= speed_left_panel;
                //iAPBtn_l_shift = 0;
            }
            if(iAPBtn_r_shift == 1)
            {
                if (rightPictureBox.Top - speed_right_panel >= gameField.Top)
                    rightPictureBox.Top -= speed_left_panel;
                //iAPBtn_r_shift = 0;
            }
        }


        //Обрабатываем отпускание клавиш
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                //прописываем действия при отжатии клавиш
                case Keys.Z:
                    iAPBtn_l_ctrl = 0;
                    break;
                case Keys.M:
                    iAPBtn_r_ctrl = 0;
                    break;
                case Keys.A:
                    iAPBtn_l_shift = 0;
                    break;
                case Keys.K:
                    iAPBtn_r_shift = 0;
                    break;
            }
        }

        private void onClickStart(object sender, EventArgs e)
        {
            startGame();
        }

        private void startGame()
        {
            btnStart.Visible = false;
            btnStart.Enabled = false;
            //start timer
            gameTimer.Enabled = true;
            gameTimer.Start();
            //move ball to center
            ball.Left = Screen.PrimaryScreen.Bounds.Width / 2;
            //hide cursor
            Cursor.Hide();
            btnExit.Enabled = false;
            btnExit.Visible = false;
        }

        private void onClickExit(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
