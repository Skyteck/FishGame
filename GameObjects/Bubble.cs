using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.GameObjects
{
    class Bubble : Sprite
    {
        float sinSeed = 0;
        public Bubble()
        {
            sinSeed = new Random().Next(0, 5);
            if(sinSeed == 1)
            {
                _FlipY = true;
            }
            else if(sinSeed == 2)
            {
                _FlipX = true;
            }
            else if(sinSeed == 3)
            {
                _FlipX = true;
                _FlipY = true;
            }
            else if(sinSeed == 4)
            {
                _Rotation = MathHelper.ToRadians(90);
            }
            else if(sinSeed == 5)
            {
                _Rotation = MathHelper.ToRadians(-90);
            }
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            float fallSpeed = 100f;

            this._Position.Y -= (float)(fallSpeed * gt.ElapsedGameTime.TotalSeconds);

            this._Position.X += (float)(Math.Sin(sinSeed));
            sinSeed += 0.125f;

            if (this._Position.Y < 50)
            {
                this.Deactivate();
            }
        }
    }
}
