using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.GameObjects
{
    class WhirlPoolBubble : Sprite
    {
        float sinSeed = 0;
        float scaleSeed = 0f;
        //Random ran;
        int spinDir;
        float fallSpeed = 100f;

        float bubbleMinSizeX = 0.3f;
        float bubbleMinSizeY = 0.5f;

        float wobble;

        float XsizeBonus;
        float YsizeBonus;
        public WhirlPoolBubble()
        {
            calcStuff();
        }

        protected override void UpdateActive(GameTime gt)
        {


            this._Position.Y += (float)(fallSpeed * gt.ElapsedGameTime.TotalSeconds);

            this._Position.X += (float)(Math.Sin(sinSeed) * wobble);

            //wobble -= 0.02f;
            if(wobble < 0)
            {
                wobble = 0;
            }
            sinSeed += 0.125f;

            this._Scale.X = Math.Abs((float)Math.Cos(scaleSeed)) + XsizeBonus;
            if (this._Scale.X < 0.6f)
            {
                this._Scale.X = 0.6f;
            }
            this._Scale.Y = Math.Abs((float)Math.Cos(scaleSeed)) + YsizeBonus;
            if (this._Scale.Y < 0.8f)
            {
                this._Scale.Y = 0.8f;
            }


            scaleSeed += 0.05f;

            if (spinDir == 0)
            {
                this._Rotation = (float)Math.Sin(scaleSeed * 7) + MathHelper.ToRadians(90);

            }

            if (this._Position.Y > 500)
            {
                this.Deactivate();
            }


            base.UpdateActive(gt);
        }

        public override void Activate(Vector2 pos)
        {
            calcStuff();

            base.Activate(pos);
        }

        private void calcStuff()
        {

            sinSeed = ArmadaRandom.Next(0, 5);
            wobble = ArmadaRandom.NextFloat(10, 10, 50);
            if (sinSeed == 1)
            {
                _FlipY = true;
            }
            else if (sinSeed == 2)
            {
                _FlipX = true;
            }
            else if (sinSeed == 3)
            {
                _FlipX = true;
                _FlipY = true;
            }
            else if (sinSeed == 4)
            {
                _Rotation = MathHelper.ToRadians(90);
            }
            else if (sinSeed == 5)
            {
                _Rotation = MathHelper.ToRadians(-90);
            }
            fallSpeed = 100f + ArmadaRandom.Next(-10, 50);
            scaleSeed = ArmadaRandom.Next(0, 5);
            spinDir = ArmadaRandom.Next(0, 2);

            YsizeBonus = ArmadaRandom.NextFloat(10, 0, 5);

            XsizeBonus = ArmadaRandom.NextFloat(10, 0, 5);
        }
    }
}
