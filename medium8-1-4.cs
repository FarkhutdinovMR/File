class Player
{
    public string Name { get; private set; }
    public int Age { get; private set; }
}

class Movement
{
    public float DirectionX { get; private set; }
    public float DirectionY { get; private set; }
    public float Speed { get; private set; }

    public void Move()
    {
        //Do move
    }
}

class Weapon
{
    public float Cooldown { get; private set; }
    public int Damage { get; private set; }

    public void Attack()
    {
        //attack
    }

    public bool IsReloading()
    {
        throw new NotImplementedException();
    }
}