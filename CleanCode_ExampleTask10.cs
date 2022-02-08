class Weapon
{
	private int _bullets;

	public bool CanShoot() => _bullets > Empty;

	public void Shoot() => _bullets -= BulletInOneShoot;

	private const int Empty = 0;
	private const int BulletInOneShoot = 1;
}