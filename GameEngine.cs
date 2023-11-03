using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using PalmTree.Items;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;

namespace PalmTree.Engine;

public class GameEngine
{
    private static readonly CancellationTokenSource cts = new CancellationTokenSource();

    private string NickName = "Debug_Nickname";

    public int yCount = 20;
    public int xCount = 120;

    int frameRate = 120;

    bool isRunning = true;
    
    int lastXInput = 1;
    int lastYInput;

    int _count = 0;

    public List<Entity> entities = new List<Entity>();
    private Item _player = new Item("Player",'@');
    private List<Item> other_players = new List<Item>();

    private string _p_data;

    private string _mainUrl = "https://25d6-91-227-189-186.ngrok-free.app/";

    //---------------------------------------Main-------------------------------------------------

    #region Main
    public GameEngine(string _nn)
    {
        Console.CancelKeyPress += (sender, args) => Exit();

        this.NickName = _nn;

        #region Join

        string url = _mainUrl + "mmo/join/";
        
        using (var wb = new WebClient())
        {
            var data = new NameValueCollection();

            data["nickname"] = NickName;

            var response = wb.UploadValues(url, "POST", data);
            string result = Encoding.UTF8.GetString(response);
        }
        #endregion

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
            
            
            #region Online players get
            //Get Players
            _p_data = new WebClient().DownloadString(_mainUrl + "mmo/getPlayers/");
            
            //Split Players
            string[] temp_p_data = _p_data.Split('_');
            temp_p_data = temp_p_data.Take(temp_p_data.Length - 1).ToArray();

            //Set Players
            other_players.Clear();
            for(int i=0;i<temp_p_data.Length;i++){
                //Get player variables
                Console.WriteLine(temp_p_data[i]);

                string[] temp_p_var = temp_p_data[i].Split('-');

                // for(int v=0;v<temp_p_var.Length;v++){
                // }

                //Check if isn't local player
                // if(temp_p_var[0] != NickName)
                    other_players.Add(new Item(temp_p_var[0], '@', int.Parse(temp_p_var[1]), int.Parse(temp_p_var[2])));


            }
            #endregion

            
            //Debug
            Console.WriteLine("Nickname:" + NickName);
            Console.WriteLine("GameObjects: " + entities.Count);
            Console.WriteLine("Frame: " + _count);
            // Console.WriteLine("Data: " + );

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
    #endregion

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

                //Render all other_players at once
                for(int e = 0; e < other_players.Count; e++){
                    if(other_players[e].X == x &&
                        other_players[e].Y == y)
                    {
                        frame[y, x] = other_players[e]._char;
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
                Exit();
                break;
            case ConsoleKey.RightArrow:

                if(this._player.X == xCount-1) return;
                lastXInput = 1;
                lastYInput = 0;

                this._player.X++;
                break;
            case ConsoleKey.LeftArrow:

                if(this._player.X == 0) return;
                lastXInput = -1;
                lastYInput = 0;

                this._player.X--;
                break;
            case ConsoleKey.UpArrow:

                if(this._player.Y == 0) return;
                lastXInput = 0;
                lastYInput = -1;

                this._player.Y--;
                break;
            case ConsoleKey.DownArrow:

                if(this._player.Y == yCount-1) return;
                lastXInput = 0;
                lastYInput = 1;

                this._player.Y++;
                break;
            // case ConsoleKey.Spacebar:
            //     Shoot();
            //     break;
            default:
                break;
        }

        //Send local pos to server
        new WebClient().DownloadString(_mainUrl + "mmo/move/" + NickName + "-" + _player.X + "-" + _player.Y);

    }

    private void Shoot(){

        entities.Add(new Bullet(_gm:this, _xDir:lastXInput, _yDir:lastYInput, 
                                _x:_player.X, _y:_player.Y));

    }

    private void Exit(){
        
        //Log exit time
        string r = File.ReadAllText("log.txt");
        r += "\n" + System.DateTime.Now + " # Exit!";
        File.WriteAllText("log.txt", r);

        //Delete from server
        new WebClient().DownloadString(_mainUrl + "mmo/kill/" + NickName);

        Console.WriteLine("Exiting...");
        cts.Cancel();
        Console.Clear();
    }

}