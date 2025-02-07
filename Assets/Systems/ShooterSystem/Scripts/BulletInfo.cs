namespace Systems.RunnerSystem
{
    public class BulletInfo
    {
        public float Damage = 0;
        public float Lifetime = 3f;
        public float Velocity = 20f;
        public string BulletText = "45";

        public BulletInfo(float damage, float lifetime, float velocity, string bulletText)
        {
            Damage = damage;
            Lifetime = lifetime;
            Velocity = velocity;
            BulletText = bulletText;
        }

        public BulletInfo(float damage)
        {
            Damage = damage;
            Lifetime = 3f;
            Velocity = 20f;
            BulletText = damage.ToString();
        }
        public BulletInfo()
        {
            Damage = 0;
            Lifetime = 3f;
            Velocity = 20f;
            BulletText = "0";
        }
            
    }
}