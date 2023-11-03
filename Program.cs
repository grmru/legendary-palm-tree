using System;
using PalmTree.Engine;

namespace PalmTree;

public class Program
{


    public static void Main()
    {
        
        string nn = "";
        string onlineMode = "";
        GameEngine game;
        
        while(!onlineMode.Equals("y") && !onlineMode.Equals("n")){
            Console.Clear();
            Console.WriteLine("Online mode? (y/n): ");
            onlineMode = Console.ReadLine();
        }

        if(onlineMode == "n"){
            game = new GameEngine(_onlineMode:false);
            game.Run();
            return;
        }

        while(nn.Contains('_') || nn.Contains('-') || nn.Length <= 2){
            Console.Clear();
            Console.WriteLine("Enter Nickname: ");
            nn = Console.ReadLine();
        }

        Console.WriteLine("Connecting to server...");
        game = new GameEngine(_nn:nn);
        game.Run();

    }

}