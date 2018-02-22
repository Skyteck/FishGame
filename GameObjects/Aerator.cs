using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.GameObjects
{
    class Aerator : Sprite
    {
        Random ran = new Random();
        Texture2D bubbbleTex;

        double BubbleTime = 0;
        public override void LoadContent(string path, ContentManager content)
        {
            base.LoadContent(path, content);

            bubbbleTex = content.Load<Texture2D>(@"Art/Bubble");
        }

        public void UpdateActive(GameTime gameTime, List<Bubble> bList)
        {


            BubbleTime += gameTime.ElapsedGameTime.TotalSeconds;



            double BubblesPerSec = 0.1;
            int bubblecalc = (int)(BubblesPerSec / 60);
            double bubbleTimer = (1.0 / BubblesPerSec);
            for(int i = 0; i <= bubblecalc; i++)
            {

                if (BubbleTime > bubbleTimer)
                {
                    GetBubble(bList);
                    BubbleTime -= bubbleTimer;


                }
            }


            
            base.UpdateActive(gameTime);
        }


        private void GetBubble(List<Bubble> bList)
        {

            Bubble b = bList.Find(x => x._CurrentState == Sprite.SpriteState.kStateInActive);
            Vector2 spawnPos = new Vector2(this._Position.X, this._BoundingBox.Top);
            if (b != null)
            {
                b.Activate(spawnPos);
            }
            else
            {
                b = new Bubble();
                b.LoadContent(bubbbleTex);
                b._Position.X = ran.Next(0, 800);
                b._Position.Y = ran.Next(500, 600);
                b._Position = spawnPos;
                bList.Add(b);
            }
        }
    }

}
