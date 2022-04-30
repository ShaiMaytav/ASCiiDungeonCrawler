using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalProject_ShaiMaytav
{
    class LvlParser
    {
        public static string[,] ConTxtToArr(string path) // Converts a text file to a 2D array
        {
            string[] lines = File.ReadAllLines(path);
            string firstLine = lines[0];
            int rows = lines.Length;
            int colls = firstLine.Length;
            string[,] grid = new string[rows, colls];
            for (int y = 0; y < rows; y++)
            {
                string line = lines[y];
                for (int x = 0; x < colls; x++)
                {
                    char currentChar = line[x];
                    grid[y, x] = currentChar.ToString();

                }
            }
            return grid;
        }  
    }
}
