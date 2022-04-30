using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using static System.Console;
namespace FinalProject_ShaiMaytav
{
    class Enemy
    {
        HUD HUD = new HUD();
        public int hp = 20;
        public int dmg = 2;
        public int X { get; set; }
        public int Y { get; set; }
        public string enemyAvatar;

        public Enemy(int spawnX, int spawnY, int _dmg, int _hp)
        {
            X = spawnX;
            Y = spawnY;
            dmg += _dmg;
            hp += _hp;
            enemyAvatar = "☺".Pastel("D32D2D");
        }

        public double RadiusToPlayer(int x, int y) // Returns the range betwqeen the enemy and the player
        {
            int xAxys = X - x;
            int yAxys = Y - y;
            Math.Abs(xAxys);
            Math.Abs(yAxys);
            return Math.Sqrt(xAxys * xAxys + yAxys * yAxys);
        }

        public void Draw()
        {
            SetCursorPosition(X, Y);
            Write(enemyAvatar);
        }

        public void TakeDamage(int damage)
        {
            HUD.InsertToLog("You dealt " + damage + "to the enemy");
            hp -= damage;
        }
    }
}
