using System;

namespace PalmTree.Items;

public class Item
{
    public int ID { get; set; } = 0;
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
}

public class Bullet : Item
{
    public int initialX { get; set; } = 0;
    public int initialY { get; set; } = 0;
    public int startFrame { get; set; } = 0;
}

public class Box : Item
{
    
}
