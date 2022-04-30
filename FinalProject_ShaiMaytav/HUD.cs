using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace FinalProject_ShaiMaytav
{
    public class HUD
    {

        List<string> occurrances = new List<string> { " ", " ", " ", " ", " "};
        public void DisplayHUD(string[,] grid, int currentlvl)
        {
            DisplayHP(grid);
            DisplayDamage(grid);
            DisplayCoins(grid);
            DisplayLog(grid);
            DisplayCurrentLvl(grid, currentlvl);
        }

        private void DisplayDamage(string[,] grid)
        {
            SetCursorPosition(10, grid.GetLength(0));
            Console.Write("Damage: " + Game.player.dmg + " ");
        }

        private void DisplayCurrentLvl(string[,] grid, int lvl)
        {
            SetCursorPosition(35, grid.GetLength(0));
            Console.Write("Level: " + lvl);
        }

        private void DisplayHP(string[,] grid)
        {
            SetCursorPosition(0, grid.GetLength(0));
            Console.Write("HP: " + Game.player.hp + " ");
        }

        private void DisplayCoins(string[,] grid)
        {
            SetCursorPosition(23, grid.GetLength(0));
            Console.Write("COINS: " + Game.player.coins);
        }

        public void InsertToLog(string ocur)
        {
            occurrances.Insert(0, ocur);
            occurrances.RemoveAt(5);
        }// Adds strings to the log's list

        private void DisplayLog(string[,] grid)
        {
            SetCursorPosition(grid.GetLength(1), 1);
            Console.WriteLine(occurrances[0]);
            SetCursorPosition(grid.GetLength(1), 2);
            Console.WriteLine(occurrances[1]);
            SetCursorPosition(grid.GetLength(1), 3);
            Console.WriteLine(occurrances[2]);
            SetCursorPosition(grid.GetLength(1), 4);
            Console.WriteLine(occurrances[3]);
            SetCursorPosition(grid.GetLength(1), 5);
            Console.WriteLine(occurrances[4]);

        }
    }
}
