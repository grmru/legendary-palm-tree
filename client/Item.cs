using System;
using System.Dynamic;
using PalmTree.Engine;

namespace PalmTree.Items;

/// <summary>
/// Базовый класс объекта игры.
/// В котором определяются наименование и координаты элемента игры на поле
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Базовая инициализация объекта игры
    /// </summary>
    /// <param name="_n">Наименование элемента</param>
    /// <param name="_c">Символ, которым отображается элемент на поле</param>
    /// <param name="_x">Координата X на поле</param>
    /// <param name="_y">Координата Y на поле</param>
    public Entity(string _n, char _c, int _x = 0, int _y = 0){
        this.character = _c;
        this.itemName = _n;
        this.X = _x;
        this.Y = _y;
    }

    /// <summary>
    /// Наименование элемента 
    /// </summary> 
    /// <value>Пусто</value>
    public string itemName { get; set; } = "";
    /// <summary>
    /// Символ, которым отображается элемент на поле
    /// </summary>
    public char character { get; set; } = '_';
    /// <summary>
    /// Координата X на поле
    /// </summary>
    public int X { get; set; } = 0;
    /// <summary>
    /// Координата Y на поле
    /// </summary>
    public int Y { get; set; } = 0;

    /// <summary>
    /// Метод, выполняемый на каждый кадр отрисовки
    /// </summary>
    public abstract void Do();
}

/// <summary>
/// Объект игрового элемента
/// </summary>
public class Item : Entity
{
    /// <summary>
    /// Инициализация игрового элемента
    /// </summary>
    /// <param name="_n">Наименование элемента</param>
    /// <param name="_c">Символ, которым отображается элемент на поле</param>
    /// <param name="_x">Координата X на поле</param>
    /// <param name="_y">Координата Y на поле</param>
    public Item(string _n, char _c, int _x = 0, int _y = 0) : base(_n, _c, _x, _y)
    {
        
    }

    /// <summary>
    /// Метод, выполняемый на каждый кадр отрисовки
    /// </summary>
    public override void Do()
    {

    }
}

/// <summary>
/// Объект реализующий функции снаряда (пули) в игре
/// </summary>
public class Bullet : Entity
{
    private int xDir;
    private int yDir;
    private GameEngine gm;

    /// <summary>
    /// Инициализация объекта
    /// </summary>
    /// <param name="_gm">Объект игрового движка</param>
    /// <param name="_xDir">Направление движения по X</param>
    /// <param name="_yDir">Направление движения по Y </param>
    /// <param name="_n">Наименование элемента</param>
    /// <param name="_c">Символ, которым отображается элемент на поле</param>
    /// <param name="_x">Координата X на поле</param>
    /// <param name="_y">Координата Y на поле</param>
    public Bullet(GameEngine _gm, int _xDir, int _yDir,
                    string _n = "Bullet", char _c = '*', int _x = 0, int _y = 0) : base(_n, _c, _x, _y)
    {
        this.gm = _gm;
        this.xDir = _xDir;
        this.yDir = _yDir;
    }

    /// <summary>
    /// Метод, выполняемый на каждый кадр отрисовки
    /// </summary>
    public override void Do()
    {
        //Move
        X += xDir;
        Y += yDir;

        //Destroy if hit
        if(Hit() || OutOfBounds()) gm.entities.Remove(this);
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
