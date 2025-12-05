using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public int poolSize = 50;

    private Queue<Bullet> pool = new Queue<Bullet>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateBullet();
        }
    }

    private Bullet CreateBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.SetActive(false);

        Bullet bullet = obj.GetComponent<Bullet>();
        pool.Enqueue(bullet);

        return bullet;
    }

    public Bullet GetBullet()
    {
        if (pool.Count == 0)
            CreateBullet(); // auto-expand pool

        Bullet bullet = pool.Dequeue();
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }
}
