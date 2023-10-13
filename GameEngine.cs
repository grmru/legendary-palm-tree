using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using PalmTree.Items;

namespace PalmTree.Engine;

public class GameEngine
{

    public int yCount = 20;
    public int xCount = 120;

    int frameRate = 120;

    bool isRunning = true;
    
    int lastXInput = 1;
    int lastYInput;

    int _count = 0;

    public List<Entity> entities = new List<Entity>();
    private Item _player = new Item("Player",'@');


    //---------------------------------------Main-------------------------------------------------

    public GameEngine()
    {
        //Items creation
        this.entities.Add(_player);
        this.entities.Add(new Item("Box",'+', 16, 5));
        this.entities.Add(new Item("Box",'+', 16, 6));
        this.entities.Add(new Item("Box",'+', 16, 7));
        this.entities.Add(new Item("Box",'+', 16, 8));
        this.entities.Add(new Item("Box",'+', 16, 9));


        //Player set
        foreach(Item i in entities){
            if(i.itemName == "Player"){
                this._player = i;
                break;
            }
            
        }
    }
    public void Run()
    {
        while(isRunning){
        
            DrawFrame();
            
            //Debug
            Console.WriteLine("GameObjects: " + entities.Count);
            Console.WriteLine("Frame: " + _count);
            _count++;

            if (Console.KeyAvailable)
            {
                Input();
            }

            foreach(Entity e in entities.ToList()) e.Do();

            System.Threading.Thread.Sleep(frameRate);
            Console.Clear();
        }
    }

    //-----------------------------------------------------------------------------------------------

    #region FrameRender
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
    #endregion


    private void Input(){

        ConsoleKeyInfo key = Console.ReadKey(true);

        switch(key.Key)
        {
            case ConsoleKey.Q:
                isRunning = false;
                break;
            case ConsoleKey.RightArrow:

                lastXInput = 1;
                lastYInput = 0;

                this._player.X++;
                break;
            case ConsoleKey.LeftArrow:

                lastXInput = -1;
                lastYInput = 0;

                this._player.X--;
                break;
            case ConsoleKey.UpArrow:

                lastXInput = 0;
                lastYInput = -1;

                this._player.Y--;
                break;
            case ConsoleKey.DownArrow:

                lastXInput = 0;
                lastYInput = 1;

                this._player.Y++;
                break;
            case ConsoleKey.Spacebar:
                Shoot();
                break;
            default:
                break; 
        }
    }

    private void Shoot(){

        entities.Add(new Bullet(_gm:this, _xDir:lastXInput, _yDir:lastYInput, 
                                _x:_player.X, _y:_player.Y));

    }

}