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
    int _lastId = 0;

    private Item _player;
    private List<Item> _gameObjects;

    public GameEngine()
    {
        this._player = new Item();
        this._gameObjects = new List<Item>();

        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 10 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 11 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 12 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 13 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 14 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 15 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 16 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 17 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 18 });
        _lastId++;
        this._gameObjects.Add(new Box() { ID = _lastId, X = 40, Y = 19 });
        _lastId++;
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
                            ID = _lastId,
                            initialX = _player.X,
                            initialY = _player.Y,
                            X = _player.X,
                            Y = _player.Y,
                            startFrame = _count
                        });
                        _lastId++;
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
        List<Item> objectsToRemove = new List<Item>();
        List<Item> drawingObjects = new List<Item>();
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            if (this._gameObjects[i] is Bullet)
            {
                Bullet bull = (Bullet)this._gameObjects[i];
                if (bull.startFrame < frameNumber)
                {
                    int deltaFrames = frameNumber - bull.startFrame;
                    bull.X = bull.initialX + deltaFrames;
                    if (bull.X > xCount)
                    {
                        objectsToRemove.Add(bull);
                    }
                }
                drawingObjects.Add(bull);
            }
            else
            {
                drawingObjects.Add(_gameObjects[i]);
            }
        }

        for (int n = 0; n < drawingObjects.Count; n++)
        {
            for (int m = 0; m < drawingObjects.Count; m++)
            {
                if (n != m)
                {
                    if (drawingObjects[n].X == drawingObjects[m].X &&
                        drawingObjects[n].Y == drawingObjects[m].Y)
                    {
                        if (drawingObjects[m] is Box)
                        {
                            objectsToRemove.Add(drawingObjects[m]);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < objectsToRemove.Count; i++)
        {
            this._gameObjects.Remove(objectsToRemove[i]);
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