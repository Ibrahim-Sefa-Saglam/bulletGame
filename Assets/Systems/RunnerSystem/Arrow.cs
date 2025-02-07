using System;
using UnityEngine;

namespace Systems.RunnerSystem
{
    public class Arrow : MonoBehaviour, IBullet
    {
        public BulletInfo BulletInfo { get; set; }
        public void Fire()
        {
            
        }

        public void Fire(BulletInfo bulletInfo)
        {
            
        }

        public void Hit(Action hitCallback)
        {
            
        }

        public void DestroySelf()
        {
           
        }
    }
}