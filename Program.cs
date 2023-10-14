using System;
using PalmTree.Engine;

namespace PalmTree;

public class Program
{


    public static void Main()
    {
        

        string nn = "";

        while(nn.Contains('_') || nn.Contains('-') || nn.Length <= 2){
            Console.Clear();
            Console.WriteLine("Enter Nickname: ");
            nn = Console.ReadLine();
        }

        GameEngine game = new GameEngine(nn);
        game.Run();

    }

}