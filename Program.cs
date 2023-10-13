using System;
using PalmTree.Engine;

namespace PalmTree;

public class Program
{


    public static void Main()
    {
        
        GameEngine game = new GameEngine();

        Console.WriteLine("Enter Nickname: ");
        string nn = Console.ReadLine();

        game.NickName = nn;
        game.Run();

    }

}