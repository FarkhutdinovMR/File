class Weapon
{
    private readonly int _damage;
    private readonly int _bulletsPerShot;
    private int _bullets;

    public Weapon(int damage, int bullets, int bulletsPerShot)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (bullets < 0)
            throw new ArgumentOutOfRangeException(nameof(bullets));

        if (bulletsPerShot < 0)
            throw new ArgumentOutOfRangeException(nameof(bulletsPerShot));

        _damage = damage;
        _bullets = bullets;
        _bulletsPerShot = bulletsPerShot;
    }

    public void Fire(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        if (_bullets - _bulletsPerShot < 0)
            throw new InvalidOperationException();

        player.TakeDamage(_damage);

        _bullets -= _bulletsPerShot;
    }
}

class Player
{
    private int _health;

    public Player(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _health = health;
    }

    public void TakeDamage(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (_health <= 0)
            throw new InvalidOperationException();

        _health -= value;

        if (_health < 0)
            _health = 0;
    }
}

class Bot
{
    private readonly Weapon _weapon;

    public Bot(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));

        _weapon = weapon;
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        _weapon.Fire(player);
    }
}