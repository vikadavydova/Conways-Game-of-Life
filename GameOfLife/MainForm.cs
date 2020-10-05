using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class MainForm : Form
    {

        const int fieldSize = 15;
        const int cellSize = 30;
        bool[,] currentFieldState;
        Button nextGenerationButton;
        Button newGameButton;

        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            currentFieldState = CreateField(fieldSize);
        }

        //Создание общего поля для игры и необходимых контролов

        private bool[,] CreateField(int size)
        {
            this.Width = (fieldSize + 2) * cellSize;
            this.Height = (fieldSize + 4) * cellSize;

            bool[,] newFieldState = new bool[size + 2, size + 2];


            for (int i = 1; i < size + 1; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.Enabled = false;
                    this.Controls.Add(button);
                }
            }

            newFieldState = PrimaryFieldState(fieldSize);

            nextGenerationButton = new Button();
            nextGenerationButton.Location = new Point(cellSize, (size + 1) * cellSize);
            nextGenerationButton.Size = new Size(cellSize * size / 2, cellSize);
            nextGenerationButton.Click += new EventHandler(NextGenPressedButton);
            nextGenerationButton.Text = "Следующее поколение";
            this.Controls.Add(nextGenerationButton);

            newGameButton = new Button();
            newGameButton.Location = new Point(cellSize * (size + 1) / 2, (size + 1) * cellSize);
            newGameButton.Size = new Size(cellSize * (size + 1) / 2, cellSize);
            newGameButton.Click += new EventHandler(NewGamePressedButton);
            newGameButton.Text = "Начать сначала";
            this.Controls.Add(newGameButton);

            return newFieldState;
        }

        //Инициализация начального состояния поля

        private bool[,] PrimaryFieldState(int size)
        {
            bool[,] newFieldState = new bool[size + 2, size + 2];

            Random generator = new Random();
            for (int i = 1; i < fieldSize + 1; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    if (generator.Next(10) < 3)
                    {
                        newFieldState[i, j] = true;
                    }
                    else
                    {
                        newFieldState[i, j] = false;
                    }

                    this.Controls[(i - 1) * size + j - 1].BackColor = PaintCell(newFieldState[i, j]);

                }
            }

            return newFieldState;

        }

        //Обработка состояния поля при следующем поколении

        private bool[,] ChangeFieldState(int size)
        {
            bool[,] newFieldState = new bool[size + 2, size + 2];
            for(int i = 1; i < size + 1; i++)
            {
                for(int j = 1; j < size + 1; j++)
                {
                    newFieldState[i, j] = ChangeCellState(i, j);
                    this.Controls[(i - 1) * fieldSize + j - 1].BackColor = PaintCell(newFieldState[i, j]);
                }
            }
            return newFieldState;
        }

        //Обработка статуса клетки при следующем поколении

        private bool ChangeCellState(int currentX, int currentY)
        {
            int counter = 0;

            for (int i = currentX - 1; i < currentX + 2; i++)
            {
                for (int j = currentY - 1; j < currentY + 2; j++)
                {
                    if (currentFieldState[i, j])
                    {
                        counter++;
                    }
                }
            }


            if (currentFieldState[currentX, currentY])
            {
                if (counter == 3 || counter == 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {

                if (counter == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private Color PaintCell(bool cellState)
        {
            if(cellState)
            {
                return Color.PaleGreen;
            }
            else
            {
                return Color.White;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void NextGenPressedButton(object sender, EventArgs e)
        {
            currentFieldState = ChangeFieldState(fieldSize);
        }

        private void NewGamePressedButton(object sender, EventArgs e)
        {
            currentFieldState = PrimaryFieldState(fieldSize);
        }
    }
}
