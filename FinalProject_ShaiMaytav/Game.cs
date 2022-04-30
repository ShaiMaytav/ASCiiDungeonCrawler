using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Media;

namespace FinalProject_ShaiMaytav
{
    class Game
    {
        Sounds sounds = new Sounds();
        List<Enemy> enemies = new List<Enemy>();
        Queue<string> levels = new Queue<string>();
        Random rand = new Random();
        private HUD HUD = new HUD();
        static public World myWorld;
        static public Player player;
        private int currentLvl = 1;
        int chance;
        int reward = 2;

        // Game methods:

        
        public void Run()
        {
            player = new Player(5, 23);

            //Enters maps to list of levels
            levels.Enqueue("Lvl1.txt");
            levels.Enqueue("Lvl2.txt");
            levels.Enqueue("Lvl3.txt");
            levels.Enqueue("Lvl4.txt");
            levels.Enqueue("Lvl5.txt");
            levels.Enqueue("Lvl6.txt");
            levels.Enqueue("Lvl7.txt");
            levels.Enqueue("Lvl8.txt");
            levels.Enqueue("Lvl9.txt");
            levels.Enqueue("Lvl10.txt");
            Title = "Shai's Game";
            CursorVisible = false;
            Clear();
            Intro();
            GenerateLvl();
            RunGameLoop();

            Outro();
        }// Starts game
        private void RunGameLoop()
        {
            while (true)
            {
                HUD.DisplayHUD(myWorld.grid, currentLvl);
                DrawFrame();
                EnemyMovement();
                HandlePlayerInput();
                PlayerBehavior();
                Combat();

                if (IsPlayerDead())
                {
                    sounds.death.Play();
                    DeathScreen();
                }

                System.Threading.Thread.Sleep(20);
            }

        }// Runs game loop...
//--------------------------------------------------------------

        // Player methods:

        private void HandlePlayerInput()
        {
            ConsoleKey key;
            do
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                key = keyInfo.Key;
            } while (KeyAvailable);

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (myWorld.IsPositionLegal(player.X, player.Y - 1))
                    {
                        player.Y -= 1;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (myWorld.IsPositionLegal(player.X, player.Y + 1))
                    {
                        player.Y += 1;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (myWorld.IsPositionLegal(player.X + 1, player.Y))
                    {
                        player.X += 1;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (myWorld.IsPositionLegal(player.X - 1, player.Y))
                    {
                        player.X -= 1;
                    }
                    break;
                default:
                    break;
            }
        }// Player's controls
        private void PlayerBehavior()
        {
            string playerPos = myWorld.GetPos(player.X, player.Y);
            if (levels.Any() && playerPos == "X")// If player reaches "X"(level exit)
            {
                GenerateLvl();
                player.xpLvl++;
                currentLvl++;
                SpawnPoints(currentLvl);
            }
            else if (playerPos == "X")// If player reaches last "X"(level exit)
            {
                Outro();
            }
            if (playerPos == "^")// If player steps on "^"(trap)
            {
                player.hp -= 5;
                myWorld.UpdateElement(player.X, player.Y, "!");
                HUD.InsertToLog("It's a trap! -5 HP                             ");
            }
            if (playerPos == "♥" && player.hp < 100)// if player steps on "♥"(hp drops)
            {
                player.hp += 5;
                sounds.eat.Play();
                myWorld.UpdateElement(player.X, player.Y, " ");
                HUD.InsertToLog("You gained 5 HP                                ");
                if (player.maxHP < player.hp)
                {
                    player.hp = 100;
                }
            }
            if (playerPos == "$")// If players steps on "$"(chest)
            {
                player.coins += 5;
                myWorld.UpdateElement(player.X, player.Y, "s");
                HUD.InsertToLog("You opened a chest, +5 COINS                   ");
            }
        }// Player's interactions with world
        private bool IsPlayerDead()
        {
            return player.hp <= 0;
        }
//--------------------------------------------------------------
        
        // PvE methods:

        private void EnemyMovement()
        {
            foreach (var enemy in enemies)
            {

                if (enemy.RadiusToPlayer(player.X, player.Y) <= 5)
                {
                    if (enemy.X < player.X && myWorld.IsPositionLegal(enemy.X + 1, enemy.Y))
                    {
                        enemy.X += 1;
                        break;
                    }
                    else if (enemy.X > player.X)
                    {
                        if (myWorld.IsPositionLegal(enemy.X - 1, enemy.Y))
                        {
                            enemy.X -= 1;
                            break;
                        }
                    }
                    else if (enemy.Y < player.Y)
                    {
                        if (myWorld.IsPositionLegal(enemy.X, enemy.Y + 1))
                        {
                            enemy.Y += 1;
                            break;
                        }
                    }
                    else if (enemy.Y > player.Y)
                    {
                        if (myWorld.IsPositionLegal(enemy.X, enemy.Y - 1))
                        {
                            enemy.Y -= 1;
                            break;
                        }
                    }
                }
            }
        }
        private void Combat()
        {
            List<Enemy> toRemove = new List<Enemy>();
            foreach (var enemy in enemies)
            {

                if (enemy.RadiusToPlayer(player.X, player.Y) <= 1)
                {
                    enemy.TakeDamage(player.dmg);
                    player.TakeDamage(enemy.dmg);
                    HUD.InsertToLog("The enemy took " + player.dmg + " damage          ");
                    HUD.InsertToLog("You took " + enemy.dmg + " damage                 ");
                    if (enemy.hp <= 0)
                    {
                        HUD.InsertToLog("The enemy died                   ");
                        sounds.eDeath.Play();
                        player.xpLvl += 5;
                        chance = rand.Next(0, 5);
                        toRemove.Add(enemy);
                        if (chance == 1)
                        {
                            player.dmg += reward * currentLvl;
                            HUD.InsertToLog("Enemy dropped a rune, +" + reward * currentLvl + " damage     ");
                        }
                        break;
                    }
                }
            }
            foreach (var enemy in toRemove)
            {
                enemies.Remove(enemy);
            }
        } // Simple combat system
//--------------------------------------------------------------

        // World methods:

        private void GenerateLvl()
        {
            Clear();
            string[,] map = LvlParser.ConTxtToArr(levels.Dequeue());

            myWorld = new World(map);
        }
        private void DrawFrame()
        {
            myWorld.Draw();
            player.Draw();
            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }
        }
        private void SpawnPoints(int lvl)
        {

            switch (lvl)
            {
                case 2:
                    player.X = 1;
                    player.Y = 14;
                    enemies.Add(new Enemy(14, 2, 0, 0));
                    enemies.Add(new Enemy(34, 11, 0, 0));
                    break;
                case 3:
                    enemies.Clear();
                    player.X = 1;
                    player.Y = 4;
                    enemies.Add(new Enemy(34, 5, 0, 0));
                    enemies.Add(new Enemy(24, 15, 0, 0));
                    enemies.Add(new Enemy(26, 15, 0, 0));
                    enemies.Add(new Enemy(63, 7, 0, 0));
                    break;
                case 4:
                    enemies.Clear();
                    player.X = 31;
                    player.Y = 1;
                    enemies.Add(new Enemy(28, 15, 0, 2));
                    enemies.Add(new Enemy(24, 15, 1, 2));
                    enemies.Add(new Enemy(24, 15, -1, 6));
                    enemies.Add(new Enemy(24, 15, 2, -1));
                    enemies.Add(new Enemy(24, 15, 0, 3));
                    enemies.Add(new Enemy(24, 15, 0, 2));
                    break;
                case 5:
                    enemies.Clear();
                    player.X = 42;
                    player.Y = 22;
                    enemies.Add(new Enemy(49, 17, 4, -5));
                    enemies.Add(new Enemy(30, 15, 4, -5));
                    enemies.Add(new Enemy(9, 9, 3, 0));
                    enemies.Add(new Enemy(7, 19, -1, 10));
                    enemies.Add(new Enemy(49, 3, 3, 0));
                    break;
                case 6:
                    enemies.Clear();
                    player.X = 16;
                    player.Y = 7;
                    enemies.Add(new Enemy(11, 3, 6, 20));

                    break;
                case 7:
                    enemies.Clear();
                    player.X = 21;
                    player.Y = 23;
                    enemies.Add(new Enemy(33, 18, 6, -2));
                    enemies.Add(new Enemy(11, 9, 6, -2));
                    enemies.Add(new Enemy(35, 12, -1, 20));
                    enemies.Add(new Enemy(36, 8, 4, 5));
                    enemies.Add(new Enemy(39, 10, 4, 5));

                    break;
                case 8:
                    enemies.Clear();
                    player.X = 40;
                    player.Y = 4;
                    enemies.Add(new Enemy(8, 18, 8, 5));
                    enemies.Add(new Enemy(9, 3, 8, 5));
                    enemies.Add(new Enemy(34, 8, 10, -5));

                    break;
                case 9:
                    enemies.Clear();
                    player.X = 43;
                    player.Y = 9;
                    enemies.Add(new Enemy(28, 4, 13, -5));
                    enemies.Add(new Enemy(28, 12, 13, -5));
                    enemies.Add(new Enemy(14, 8, 10, 20));
                    enemies.Add(new Enemy(14, 9, 10, 20));
                    enemies.Add(new Enemy(14, 10, 10, 20));

                    break;
                case 10:
                    enemies.Clear();
                    player.X = 27;
                    player.Y = 12;
                    enemies.Add(new Enemy(26, 17, -1, 100));
                    enemies.Add(new Enemy(10, 11, 15, 150));
                    enemies.Add(new Enemy(43, 5, 10, 25));
                    enemies.Add(new Enemy(47, 5, 10, 25));
                    enemies.Add(new Enemy(52, 5, 10, 25));

                    break;
                default:
                    break;
            }
        }// Player's and enemies spawn points for each level
//--------------------------------------------------------------

        // Menus methods:

        private void DeathScreen()
        {
            Clear();
            Console.WriteLine("You died, bad at game!");
            Console.WriteLine("Your score: " + (player.xpLvl + player.hp + player.dmg + player.coins));
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
            Environment.Exit(0);
        }
        private void Intro()
        {
            Console.WriteLine("Hi! Welcome to my game.");
            Console.WriteLine("\nUse the arrow keys to move around.");
            Console.WriteLine("To finish a level, reach the \"X\"");
            Console.WriteLine("\nGood luck");
            Console.WriteLine("\nPress any key to start the game");
            ReadKey(true);
        }
        private void Outro()
        {
            Clear();
            Console.WriteLine("Thank you for playing!");
            Console.WriteLine("Your score: " + (player.xpLvl + player.hp + player.dmg + player.coins) );
            Console.WriteLine("\nPress any key to exit");
            ReadKey(true);
            Environment.Exit(0);
        }

    }
}
