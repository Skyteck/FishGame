using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame.GameObjects.Items
{
    class FoodPellet : Sprite
    {
        float sinSeed = 0;
        float fallSpeed = 50f;

        public FoodPellet()
        {
            Activate();
        }

        protected override void UpdateActive(GameTime gt)
        {


            if(this._Position.Y < 70)
            {
                this._Position.Y += (float)((fallSpeed*2) * gt.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                this._Position.Y += (float)(fallSpeed * gt.ElapsedGameTime.TotalSeconds);

                this._Position.X += (float)(Math.Sin(sinSeed));
                sinSeed += 0.125f;

                if(this._Position.Y > 500)
                {
                    this.Deactivate();
                }
            }
            base.UpdateActive(gt);


        }

        public override void Activate(Vector2 pos)
        {
            sinSeed = ArmadaRandom.Next(0, 5);

            base.Activate(pos);
        }
    }
}
