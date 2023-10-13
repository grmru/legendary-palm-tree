using System;

namespace PalmTree.Items;

public class Item
{
    public Item(string _n, char _c, int _x = 0, int _y = 0){
        this._char = _c;
        this.itemName = _n;
        this.X = _x;
        this.Y = _y;
    }

    public string itemName { get; set; } = "";
    public char _char { get; set; } = '_';
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
}

// public class Bullet : Item
// {

// }