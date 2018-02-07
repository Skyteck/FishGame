using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame
{
    class FoodPellet : Sprite
    {
        float sinSeed = 0;

        public FoodPellet()
        {
            sinSeed = new Random().Next(0, 5);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            float fallSpeed = 50f;

            this._Position.Y += (float)(fallSpeed * gt.ElapsedGameTime.TotalSeconds);

            this._Position.X += (float)(Math.Sin(sinSeed));
            sinSeed += 0.125f;

            if(this._Position.Y > 500)
            {
                this.Deactivate();
            }
        }
    }
}
