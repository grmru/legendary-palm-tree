using System;
using PalmTree.Items;

namespace PalmTree.Engine;

public class GameEngine
{
    int yCount = 30;
    int xCount = 120;

    bool isRunning = false;

    int _count = 0;

    private Item _player;

    public GameEngine()
    {
        this._player = new Item();
    }

    public void Run()
    {
        isRunning = true;

        while (isRunning)
        {
            DrawFrame();
            Console.WriteLine(_count);
            _count++;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        isRunning = false;
                        break;
                    case ConsoleKey.RightArrow:
                        this._player.X++;
                        break;
                    case ConsoleKey.LeftArrow:
                        this._player.X--;
                        break;
                    case ConsoleKey.UpArrow:
                        this._player.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        this._player.Y--;
                        break;
                    default:
                        break;
                }
            }
            
            System.Threading.Thread.Sleep(100);
            Console.Clear();
        }
    }

    private void DrawFrame()
    {
        var frm = GetFrame();
        for (int i = 0; i < yCount; i++)
        {
            char[] line = new char[xCount];
            for (int j = 0; j < xCount; j++)
            {
                line[j] = frm[i, j];
            }

            Console.WriteLine(line);
        }
    }

    private char[,] GetFrame()
    {
        char[,] frame = new char[yCount, xCount];
        for (int i = 0; i < yCount; i++)
        {
            for (int j = 0; j < xCount; j++)
            {
                if (this._player.X == j &&
                    this._player.Y == i)
                {
                    frame[i, j] = '@';
                }
                else
                {
                    frame[i, j] = '.';
                }
            }
        }

        return frame;
    }
}