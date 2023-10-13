using System;
using System.Dynamic;
using PalmTree.Engine;

namespace PalmTree.Items;

public abstract class Entity
{
    public Entity(string _n, char _c, int _x = 0, int _y = 0){
        this._char = _c;
        this.itemName = _n;
        this.X = _x;
        this.Y = _y;
    }

    public string itemName { get; set; } = "";
    public char _char { get; set; } = '_';
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

    public abstract void Do();
}

public class Item : Entity
{
    public Item(string _n, char _c, int _x = 0, int _y = 0) : base(_n, _c, _x, _y)
    {
        
    }

    public override void Do()
    {

    }
}

public class Bullet : Entity
{
    private int xDir;
    private int yDir;
    private GameEngine gm;

    public Bullet(GameEngine _gm, int _xDir, int _yDir,
                    string _n = "Bullet", char _c = '*', int _x = 0, int _y = 0) : base(_n, _c, _x, _y)
    {
        this._char = _c;
        this.itemName = _n;
        this.X = _x;
        this.Y = _y;

        this.gm = _gm;
        this.xDir = _xDir;
        this.yDir = _yDir;
    }

    public override void Do()
    {
        //Move
        X += xDir;
        Y += yDir;

        //Destroy if hit
        if(Hit() || OutOfBounds()) gm.entities.Remove(this);
        // if(OutOfBounds()) gm.entities.Remove(this);
    }

    private bool Hit(){

        bool val = false;

        foreach(Entity e in gm.entities){
            if(!(e is Bullet) && e.itemName != "Player" &&
                e.X == this.X && e.Y == this.Y) val = true;
        }

        return val;
    }

    private bool OutOfBounds(){
        bool val = false;

        if(this.X > gm.xCount || this.X < 0 || 
            this.Y > gm.yCount || this.Y < 0) val = true;

        return val;
    }
}