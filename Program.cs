using System;
using PalmTree.Engine;

namespace PalmTree;

public class Program
{


    public static void Main()
    {
        

        Console.WriteLine("Enter Nickname: ");
        string nn = Console.ReadLine();

        GameEngine game = new GameEngine(nn);
        game.Run();

    }

}