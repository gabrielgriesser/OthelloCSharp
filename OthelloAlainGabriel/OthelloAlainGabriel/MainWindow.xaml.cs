﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OthelloAlainGabriel
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            InitializeBoard();
        }
        #region Property
        Player player1, player2;
        Token token1, token2;
        #endregion

        #region Attribute
        bool isPlayer1;
        int[,] tabBoard;
        #endregion

        private void InitializeGame()
        {
            token1 = new Token(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\Assets\Tokens\token_black.png"));
            token2 = new Token(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\Assets\Tokens\token2.png"));

            player1 = new Player(token1, "Gabriel");
            player2 = new Player(token2, "Alain");

            isPlayer1 = true;

        }

        private void InitializeBoard()
        {
            tabBoard = new int[7, 9];
            tokenGrid.Background = Brushes.LightGreen;
            for (int i = 0; i < 7; i++)
            {
                tokenGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < 9; j++)
                {
                    Label lbl = new Label
                    {
                        ToolTip = ((Char)(j + 65)) + "" + (i + 1)
                    };

                    lbl.Name = "j" + j + "i" + i;
                    lbl.MouseDown += Btn_Click;
                    lbl.BorderThickness = new Thickness(0.1, 0.1, 0.1, 0.1);
                    lbl.BorderBrush = Brushes.Black;

                    if (tokenGrid.ColumnDefinitions.Count < 9)
                        tokenGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.SetColumn(lbl, j);
                    Grid.SetRow(lbl, i);
                    tokenGrid.Children.Add(lbl);

                    if ((i == 3 && j == 3) || (i == 4 && j == 4))
                    {
                        lbl.Background = player2.Token.ImgBrush;
                        lbl.MouseDown -= Btn_Click;
                        tabBoard[i, j] = 2;
                    }
                    if ((i == 3 && j == 4) || (i == 4 && j == 3))
                    {
                        lbl.Background = player1.Token.ImgBrush;
                        lbl.MouseDown -= Btn_Click;
                        tabBoard[i, j] = 1;
                    }
                    lblImgPlayerTurn.Content = player1.ToString();

                }
            }
            CheckCases();
        }

        #region ButtonFunction
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Label lbl = sender as Label;
            Console.WriteLine((sender as Label).Name);

            int col = (int)Char.GetNumericValue(lbl.Name[1]);
            int row = (int)Char.GetNumericValue(lbl.Name[3]);

            if (isPlayer1)
            {
                lbl.Background = player1.Token.ImgBrush;
                isPlayer1 = false;
                lblImgPlayerTurn.Content = player2.ToString();
                tabBoard[row, col] = 1;
            }
            else
            {
                lbl.Background = player2.Token.ImgBrush;
                isPlayer1 = true;
                lblImgPlayerTurn.Content = player1.ToString();
                tabBoard[row, col] = 2;
            }
            
            lbl.MouseDown -= Btn_Click;
            CheckCases();
        }
        #endregion

        #region gameAlgo
        private void CheckCases()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label myLabel = GetChildren(tokenGrid, i, j) as Label;
                    if (checkLeft(j, i) || checkRight(j, i) || checkTop(j, i) || checkBot(j, i) || checkTopLeft(j, i) || checkBotRight(j, i) || checkTopRight(j, i) || checkBotLeft(j, i))
                    {
                        myLabel.Background = Brushes.Green;
                    }
                    else if (tabBoard[i, j] == 0)
                    {
                        myLabel.Background = Brushes.Transparent;
                    }
                }
            }
        }

        private bool checkLeft(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (col == 0 || tabBoard[row, col - 1] == playerToken || tabBoard[row, col - 1] == 0 || tabBoard[row, col] != 0)
                return false;
            for (int i = col-2; i >= 0; i--)
            {
                if (tabBoard[row, i] == playerToken)
                    return true;
                if (tabBoard[row, i] == 0)
                    return false;
            }
            return false;
        }
        private bool checkRight(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (col == 8 || tabBoard[row, col + 1] == playerToken || tabBoard[row, col + 1] == 0 || tabBoard[row, col] != 0)
                return false;
            for (int i = col + 2; i < 9; i++)
            {
                if (tabBoard[row, i] == playerToken)
                    return true;
                if (tabBoard[row, i] == 0)
                    return false;
            }
            return false;
        }

        private bool checkTop(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 0 || tabBoard[row - 1, col] == playerToken || tabBoard[row - 1, col] == 0 || tabBoard[row, col] != 0)
                return false;
            for (int i = row - 2; i >= 0; i--)
            {
                if (tabBoard[i, col] == playerToken)
                    return true;
                if (tabBoard[i, col] == 0)
                    return false;
            }
            return false;
        }

        private bool checkBot(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 6 || tabBoard[row + 1, col] == playerToken || tabBoard[row + 1, col] == 0 || tabBoard[row, col] != 0)
                return false;
            for (int i = row + 2; i < 7; i++)
            {
                if (tabBoard[i, col] == playerToken)
                    return true;
                if (tabBoard[i, col] == 0)
                    return false;
            }
            return false;
        }

        private bool checkTopLeft(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 0 || col == 0 || tabBoard[row - 1, col - 1] == playerToken || tabBoard[row - 1, col - 1] == 0 || tabBoard[row, col] != 0)
                return false;
            while (row > 0 && col > 0)
            {
                row--;
                col--;
                if (tabBoard[row, col] == playerToken)
                    return true;
                if (tabBoard[row, col] == 0)
                    return false;
            }
            return false;
        }

        private bool checkBotRight(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 6 || col == 8 || tabBoard[row + 1, col + 1] == playerToken || tabBoard[row + 1, col + 1] == 0 || tabBoard[row, col] != 0)
                return false;
            while (row < 6 && col < 8)
            {
                row++;
                col++;
                if (tabBoard[row, col] == playerToken)
                    return true;
                if (tabBoard[row, col] == 0)
                    return false;
            }
            return false;
        }

        private bool checkTopRight(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 0 || col == 8 || tabBoard[row - 1, col + 1] == playerToken || tabBoard[row - 1, col + 1] == 0 || tabBoard[row, col] != 0)
                return false;
            while (row > 0 && col < 8)
            {
                row--;
                col++;
                if (tabBoard[row, col] == playerToken)
                    return true;
                if (tabBoard[row, col] == 0)
                    return false;
            }
            return false;
        }
        private bool checkBotLeft(int col, int row)
        {
            int playerToken = 1;
            if (!isPlayer1)
                playerToken = 2;
            if (row == 6 || col == 0 || tabBoard[row + 1, col - 1] == playerToken || tabBoard[row + 1, col - 1] == 0 || tabBoard[row, col] != 0)
                return false;
            while (row < 6 && col > 0)
            {
                row++;
                col--;
                if (tabBoard[row, col] == playerToken)
                    return true;
                if (tabBoard[row, col] == 0)
                    return false;
            }
            return false;
        }

        private static UIElement GetChildren(Grid grid, int row, int column)
        {
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row
                      &&
                   Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;   
        }
        #endregion

        #region MenuFunction       
        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuUndo_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("About : GG and AG Othello");
        }
        #endregion
    }
}