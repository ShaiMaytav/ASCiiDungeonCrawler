using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Pastel;

namespace FinalProject_ShaiMaytav
{
    class Player
    {
        HUD HUD = new HUD();
        public int hp = 100;
        public int dmg = 5;
        public int coins = 0;
        public int xpLvl = 1;
        public int maxHP = 100;
        public int X { get; set; }
        public int Y { get; set; }
        private string PlayerAvatar;
        public Player(int spawnX, int spawnY)
        {
            X = spawnX;
            Y = spawnY;
            PlayerAvatar = "☻".Pastel("05858a");
        }

        public void Draw()
        {
            SetCursorPosition(X, Y);
            Write(PlayerAvatar);
        }

        public void TakeDamage(int damage)
        {
            HUD.InsertToLog("You took " + damage + "damage");
            hp -= damage;
        }

    }
}
