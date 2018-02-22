using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame.GameObjects
{
    class FishTail : Sprite
    {


        float scaleSeed = 0f;


        protected override void UpdateActive(GameTime gameTime)
        {
            if(parent is Fish)
            {
                this._Position = (parent as Fish).TailPosition;
                if((parent as Fish)._FlipX)
                {
                    this._FlipX = true;
                    this._Position.X += this._BoundingBox.Width / 2;
                }
                else
                {
                    this._FlipX = false;
                    this._Position.X -= this._BoundingBox.Width / 2;
                }
            }


            this._Scale.X = Math.Abs((float)Math.Cos(scaleSeed));
            //Console.WriteLine(this._Scale.X);
            if (this._Scale.X <= 0.3f)
            {
                this._Scale.X = 0.3f;
            }
            else if (this._Scale.X >= 0.9f)
            {
                this._Scale.X = 0.9f;
            }

            this._Scale.X *= (parent as Fish)._Scale.X;

            this._Scale.Y = Math.Abs((float)Math.Sin(scaleSeed)) + (parent as Fish)._Scale.Y;
            if (this._Scale.Y < 0.8f)
            {
                this._Scale.Y = 0.8f;
            }
            else if (this._Scale.Y > 1.2f)
            {
                this._Scale.Y = 1.2f;
            }


            this._Scale.Y *= (parent as Fish)._Scale.Y;

            //if(InputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T))
            //{
            scaleSeed += 0.03f;

            //}


            base.UpdateActive(gameTime);
        }
    }
}
