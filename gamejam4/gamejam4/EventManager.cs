using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamejam4
{
    // フレーム数に従うイベント発生装置
    class EventManager
    {
        private Random rand = new Random();
        private asd.Scene scene;
        private asd.Layer2D mainLayer;
        private asd.Layer2D subLayer;

        private const int spawnCircleFrame = 60;

        public EventManager(asd.Scene scene)
        {
            this.scene = scene;
            List<asd.Layer> layers = scene.Layers.ToList();
            mainLayer = layers[0] as asd.Layer2D;
            subLayer = layers[1] as asd.Layer2D;
        }

        public void updateByFrame(int frameCount)
        {

            if (frameCount == 0)
            {
                spawnPlayer();
            }

            if (frameCount % spawnCircleFrame == 0)
            {
                spawnCircle();
            }
        }

        public void updateByCollision()
        {
            PlayerRect player;
            Circle circle;
            foreach (var p in mainLayer.Objects)
                if ((player = p as PlayerRect) != null)
                    foreach (var e in mainLayer.Objects)
                    {
                        // player との当たり判定を書けるブロック

                        if ((circle = e as Circle) != null)
                        {
                            asd.Vector2DF circleToPlayer = player.Position - circle.Position;
                            if (circleToPlayer.Length < 3*player.height)
                            {
                                asd.Vector2DF xh, xv;

                                // 交差判定
                                circle.MoveBack();
                                player.MoveBack();
                                for (int i = 0; i < 10; i++)
                                {
                                    circle.DeltaMove();
                                    player.DeltaMove();
                                    circleToPlayer = player.Position - circle.Position;
                                    xh = asd.Vector2DF.Dot(player.direction.Normal, circleToPlayer) * player.direction.Normal;
                                    xv = circleToPlayer - xh;
                                    if (xh.Length <= player.width / 2 + circle.radius
                                    && xv.Length <= player.height / 2 + circle.radius)
                                    {
                                        int id_se1 = asd.Engine.Sound.Play(se1);
                                        circle.Color = new asd.Color(255, 0, 0, 255);
                                        float dot = asd.Vector2DF.Dot(player.direction, circleToPlayer.Normal);
                                        asd.Vector2DF relationalVelocity = circle.velocity - player.velocity;
                                        asd.Vector2DF deltaVelocity;
                                        if (xh.Length > player.width / 2
                                        && xv.Length > player.height / 2)
                                        {
                                            deltaVelocity = 1.5f * (asd.Vector2DF.Dot(relationalVelocity, -circleToPlayer.Normal)*-circleToPlayer.Normal);
                                        }
                                        else if (dot * dot > .038f)
                                        {
                                            deltaVelocity = 1.5f * (asd.Vector2DF.Dot(relationalVelocity, (dot * player.direction).Normal) * (dot * player.direction).Normal);
                                        }
                                        else
                                        {
                                            deltaVelocity = 1.5f * (relationalVelocity - asd.Vector2DF.Dot(relationalVelocity, (dot * player.direction).Normal) * (dot * player.direction).Normal);
                                        }
                                        circle.velocity -= deltaVelocity;
                                        while (++i < 10)
                                        {
                                            player.DeltaMove();
                                            circle.DeltaMove();
                                            circleToPlayer = player.Position - circle.Position;
                                            xh = asd.Vector2DF.Dot(player.direction.Normal, circleToPlayer) * player.direction.Normal;
                                            xv = circleToPlayer - xh;
                                            while (xh.Length <= player.width / 2 + circle.radius
                                            && xv.Length <= player.height / 2 + circle.radius)
                                            {
                                                circleToPlayer = player.Position - circle.Position;
                                                xh = asd.Vector2DF.Dot(player.direction.Normal, circleToPlayer) * player.direction.Normal;
                                                xv = circleToPlayer - xh;
                                                dot = asd.Vector2DF.Dot(player.direction, circleToPlayer.Normal);

                                                if (dot * dot > .038f)
                                                {
                                                    circle.Position -= (dot * player.direction).Normal;
                                                }
                                                else
                                                {
                                                    circle.Position -= (player.direction - (dot * player.direction)).Normal;
                                                }
                                            }

                                        }
                                        
                                        break;
                                    }
                                }
                            }
                        }
                    }
        }

        private void spawnPlayer()
        {
            mainLayer.AddObject(new PlayerRect());
        }

        private void spawnCircle()
        {
            float x = asd.Engine.WindowSize.X / 2;
            float y = asd.Engine.WindowSize.Y;
            float vx = (rand.Next() % 256) / 255f * 5 - 2;
            float vy = -(rand.Next() % 256) / 255f * 8 - 2;
            float r = 20;
            Circle circle = new Circle(new asd.Vector2DF(x, y), 20 * (new asd.Vector2DF(vx, vy)).Normal, r);
            circle.isGravity = true;
            circle.AddChild(new Circle(new asd.Vector2DF(-r / 6, -r / 6), 3 * (new asd.Vector2DF(vx, vy)).Normal, r / 2), asd.ChildMode.Position);
            circle.AddChild(new Circle(new asd.Vector2DF(-r / 4, -r / 4), 3 * (new asd.Vector2DF(vx, vy)).Normal, r / 4), asd.ChildMode.Position);
            mainLayer.AddObject(circle);
            foreach (var e in circle.Children)
                subLayer.AddObject(e);

        }
    }
}
