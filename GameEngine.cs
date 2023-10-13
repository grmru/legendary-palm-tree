using System;
using PalmTree.Items;

namespace PalmTree.Engine;

public class GameEngine
{
    int yCount = 30;
    int xCount = 120;
    int framePause = 50;

    bool isRunning = false;

    int _count = 0;

    private Item _player;
    private List<Item> _gameObjects;

    public GameEngine()
    {
        this._player = new Item();
        this._gameObjects = new List<Item>();

        this._gameObjects.Add(new Bullet() { X = 10, Y = 10 });
        this._gameObjects.Add(new Box() { X = 40, Y = 10 });
    }

    public void Run()
    {
        isRunning = true;

        while (isRunning)
        {
            DrawFrame(_count);
            Console.WriteLine($"Кол-во объектов: {_gameObjects.Count} Кадр №: {_count}");
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
                        this._player.Y--;
                        break;
                    case ConsoleKey.DownArrow:
                        this._player.Y++;
                        break;
                    case ConsoleKey.Spacebar:
                        this._gameObjects.Add(new Bullet()
                        {
                            initialX = _player.X,
                            initialY = _player.Y,
                            X = _player.X,
                            Y = _player.Y,
                            startFrame = _count
                        });
                        break;
                    default:
                        break;
                }
            }
            
            System.Threading.Thread.Sleep(framePause);
            Console.Clear();
        }
    }

    private void DrawFrame(int frameNumber)
    {
        var frm = GetFrame(frameNumber);
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

    private char[,] GetFrame(int frameNumber)
    {
        List<Item> drawingObjects = new List<Item>();
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            if (this._gameObjects[i] is Bullet)
            {
                Bullet bull = (Bullet)this._gameObjects[i];
                if (bull.startFrame < frameNumber)
                {
                    int deltaFrames = frameNumber - bull.startFrame;
                    bull.X = bull.X + deltaFrames;
                }
                drawingObjects.Add(bull);
            }
            else
            {
                drawingObjects.Add(_gameObjects[i]);
            }
        }

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
                    bool alreadyDraw = false;
                    for (int o = 0; o < drawingObjects.Count; o++)
                    {
                        if (drawingObjects[o].X == j &&
                            drawingObjects[o].Y == i)
                        {
                            if (drawingObjects[o] is Bullet)
                            {
                                frame[i, j] = '-';
                                alreadyDraw = true;
                            }
                            else if (drawingObjects[o] is Box)
                            {
                                frame[i, j] = '#';
                                alreadyDraw = true;
                            }
                        }
                    }

                    if (!alreadyDraw)
                    {
                        frame[i, j] = '.';
                    }
                }
            }
        }

        return frame;
    }
}