using System;
using PalmTree.Engine;

namespace PalmTree;

public class Program
{
    public static void Main(string[] args)
    {
        string nn = "";
        string onlineMode = "";

        string addr = "127.0.0.1";
        int port = 1234;

        if (args.Length > 0)
        {
            addr = args[0];
        }
        if (args.Length > 1)
        {
            port = int.Parse(args[1]);
        }

        GameEngine game;
        
        while(!onlineMode.Equals("y") && !onlineMode.Equals("n")){
            Console.Clear();
            Console.WriteLine("Online mode? (y/n): ");
            onlineMode = Console.ReadLine();
        }

        if(onlineMode == "n")
        {
            game = new GameEngine(_onlineMode:false);
            game.Run();
        }
        else
        {
            while(nn.Contains('_') || nn.Contains('-') || nn.Length <= 2){
                Console.Clear();
                Console.WriteLine("Enter Nickname (> 2): ");
                nn = Console.ReadLine();
            }

            Console.WriteLine("Connecting to server...");
            game = new GameEngine(_nn:nn, _addr:addr, _port:port);
            game.Run();
        }
    }
}