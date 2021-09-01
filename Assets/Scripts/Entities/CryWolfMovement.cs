using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public class CryWolfMovement : MovementBase
    {
        public bool stop;
        public CryWolf cryWolf;
        protected override void Update()
        {
            if (stop || cryWolf.stats.IsDead)
                return;
            base.Update();



        }

        protected override void FixedUpdate()
        {
            if (stop || cryWolf.stats.IsDead)
                return;
            base.FixedUpdate();
        }
    }
}
