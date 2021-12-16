class Weapon
{
    private int _damage;
    private int _bullets;

    public Weapon(int damage, int bullets)
    {
        _damage = damage;
        _bullets = bullets;
    }

    public void Fire(Player player)
    {
        if (_bullets <= 0)
            return;

        player.TakeDamage(_damage);
        _bullets -= 1;
    }
}

class Player
{
    private int _health;

    public Player(int health)
    {
        _health = health;
    }

    public void TakeDamage(int value)
    {
        if (_health <= 0)
            return;

        if (value < 0)
            throw new ArgumentOutOfRangeException();

        _health -= value;
    }
}

class Bot
{
    private Weapon _weapon;

    public Bot(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void OnSeePlayer(Player player)
    {
        _weapon.Fire(player);
    }
}