using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamejam4
{
    class Circle : asd.GeometryObject2D
    {
        private asd.CircleShape circleShape;
        public asd.Vector2DF velocity;
        public bool isGravity;
        public float radius;

        public Circle(asd.Vector2DF position, asd.Vector2DF velocity, float diameter)
        {
            radius = diameter / 2;

            circleShape = new asd.CircleShape();
            circleShape.OuterDiameter = diameter;

            Shape = circleShape;
            Position = position;
            Color = new asd.Color(255, 255, 255, 128);

            this.velocity = velocity;
        }

        protected override void OnUpdate()
        {
            if (isGravity)
            {
                velocity.Y += .3f;
                Position += velocity;
                if (Position.Y > asd.Engine.WindowSize.Y)
                {
                    velocity.Y *= -.7f;
                    Position = new asd.Vector2DF(Position.X, asd.Engine.WindowSize.Y);
                }
                else if (Position.Y < 0)
                {
                    velocity.Y *= -.7f;
                    Position = new asd.Vector2DF(Position.X, 0);
                }
                else if (Position.X < 0)
                {
                    velocity.X *= -.7f;
                    Position = new asd.Vector2DF(0, Position.Y);
                }
                else if (Position.X > asd.Engine.WindowSize.X)
                {
                    velocity.X *= -.7f;
                    Position = new asd.Vector2DF(asd.Engine.WindowSize.X, Position.Y);
                }

                if (Position.X < -circleShape.OuterDiameter || Position.X > asd.Engine.WindowSize.X + circleShape.OuterDiameter)
                {
                    // 終了処理
                    int count = Children.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var buf = Children.First();
                        buf.Vanish();
                        RemoveChild(buf);
                    }
                    Vanish();
                }
            }

            // drag
            if (velocity.Length > 15)
            {
                velocity -= velocity*.05f;
            }
        }

        public void Reflect(asd.Vector2DF direction){
            direction.Normalize();
            float dot = asd.Vector2DF.Dot(velocity, direction);
            velocity -= 1.8f * dot * direction;
        }

        public void AddedForce(asd.Vector2DF acceleration)
        {
            velocity += acceleration;
        }

        public void DeltaMove()
        {
            Position += velocity / 10;
        }

        public void MoveBack()
        {
            Position -= velocity;
        }
        public void setVelocity(asd.Vector2DF velocity)
        {
            this.velocity = velocity;
        }
    }
}
