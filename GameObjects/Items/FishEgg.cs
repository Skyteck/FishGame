using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.GameObjects.Items
{
    class FishEgg : Sprite
    {
        float sinSeed = 0;
        float fallSpeed = 30f;

        double fallTime = 0f;
        double HatchTime = ArmadaRandom.NextDouble(10, 13, 20);
        public bool makeFish = false;
        int yLanding = ArmadaRandom.Next(420, 470);

        protected override void UpdateActive(GameTime gt)
        {


            if (this._Position.Y < 70)
            {
                this._Position.Y += (float)((fallSpeed * 5) * gt.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                if(this._Position.Y < yLanding)
                {

                    this._Position.Y += (float)(fallSpeed * gt.ElapsedGameTime.TotalSeconds);
                    this._Position.X += (float)(Math.Sin(sinSeed));
                }

                sinSeed += 0.125f;


                fallTime += gt.ElapsedGameTime.TotalSeconds;

                this._Opacity = (float)(1 - (fallTime / HatchTime));

                if(fallTime >= HatchTime)
                {
                    makeFish = true;
                    this.Deactivate();
                }

            }
            base.UpdateActive(gt);


        }

        public override void Activate(Vector2 pos)
        {
            sinSeed = ArmadaRandom.Next(0, 5);
            fallTime = 0;
            makeFish = false;
            HatchTime = ArmadaRandom.NextDouble(10, 38, 50);
            yLanding = ArmadaRandom.Next(425, 480);
            base.Activate(pos);
        }
    }
}
