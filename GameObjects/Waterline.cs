using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame.GameObjects
{
    class Waterline : Sprite
    {
        public Waterline()
        {
            this._Scale.X += 0.05f;
        }

        public override void Update(GameTime gt)
        {
            float speed = 50f;

            this._Position.X += (float)(speed * gt.ElapsedGameTime.TotalSeconds);

            if(this._Position.X > 900)
            {
                this._Position.X = -32;
            }

            base.Update(gt);
        }
    }
}
