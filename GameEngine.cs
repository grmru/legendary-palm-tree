using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using PalmTree.Items;

namespace PalmTree.Engine;

public class GameEngine
{
    int yCount = 20;
    int xCount = 120;

    bool isRunning = true;
    
    int _count = 0;

    private List<Item> entities = new List<Item>();
    private Item _player;

    public GameEngine()
    {
        //Items creation
        this.entities.Add(new Item("Player",'@'));
        this.entities.Add(new Item("Box",'+', 2, 5));
        this.entities.Add(new Item("Box",'+', 5, 5));


        //Player set
        foreach(Item i in entities){
            if(i.itemName == "Player"){
                this._player = i;
                // break;
            }
            
        }
        // this._player = new Item('@');
    }
    public void Run()
    {
        isRunning = true;

        while(isRunning){
        
            DrawFrame();
            Console.WriteLine("FPS: " + _count);
            _count++;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch(key.Key)
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
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                
                //Render all objects at once
                for(int e = 0; e < entities.Count; e++){
                    if(entities[e].X == x &&
                        entities[e].Y == y)
                    {
                        frame[y, x] = entities[e]._char;
                        break;
                    }
                    else{
                        frame[y, x] = '.';
                    }
                }
                
            }
        }
        return frame;
    }
}