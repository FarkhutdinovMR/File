class Weapon
{
	private int _bullets;

	public bool CanShoot() => _bullets > Empty;
	public void Shoot() => _bullets -= BulletsPerShot;

	private const int Empty = 0;
	private const int BulletInOneShoot = 1;
}