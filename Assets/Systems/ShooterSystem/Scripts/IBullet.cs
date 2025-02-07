
using System;

namespace Systems.RunnerSystem
{
    public interface IBullet
    {
        public BulletInfo BulletInfo {
            get;
            set;
        }
        
        void Fire();
        
        void Fire(BulletInfo bulletInfo);
        

        void Hit(Action hitCallback);
        
        void DestroySelf();
    }
}