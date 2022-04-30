using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Pastel;

namespace FinalProject_ShaiMaytav
{
    class World
    {
        public string[,] grid;
        private int yAxis;
        private int xAxis;

        public World(string[,] _grid)
        {
            grid = _grid;
            yAxis = grid.GetLength(0);
            xAxis = grid.GetLength(1);
        }

        public void Draw()
        {
            for (int y = 0; y < yAxis; y++)
            {
                for (int x = 0; x < xAxis; x++)
                {
                    string item = grid[y, x];
                    SetCursorPosition(x, y);
                    string itemColor;
                    if (item == "^")
                    {
                        itemColor = "000000";
                    }
                    else if (item == "♥")
                    {
                        itemColor = "D8464B";
                    }
                    else if (item == "$")
                    {
                        itemColor = "CAAF02";
                    }
                    else if (item == "X")
                    {
                        itemColor = "0B6D20";
                    }
                    else
                        itemColor = "FFFFFF";
                    Write(item.Pastel(itemColor));
                }
            }
        }

        public string GetPos(int x, int y)
        {
            return grid[y, x];
        } // Returns the element in the given coordinates

        public void UpdateElement(int x, int y, string element) // Changes elements symbol
        {
            grid[y, x] = element;
        }

        public bool IsPositionLegal(int x, int y)
        {
            //checks bounds
            if (x< 0 || y < 0 || x >= xAxis || y >= yAxis || grid[y, x] == "☺" || grid[y, x] == "☻" )
            {
                return false;
            }

            //checks if position legal
            return grid[y, x] == " " || grid[y, x] == "X" || grid[y, x] == "$" || grid[y, x] == "♥" || grid[y, x] == "^" || grid[y, x] == "s" || grid[y, x] == "!";
        } // Checks if an entity can move to a set position

    }
}
