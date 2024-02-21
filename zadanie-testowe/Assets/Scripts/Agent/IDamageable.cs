using System;
using System.Collections.Generic;


namespace TestTask
{
    public interface IDamageable
    {
        public int damage { get; set; }
        public void TakeDamage(int damage);

    }
}
