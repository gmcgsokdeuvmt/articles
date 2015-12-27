using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamejam4
{
    class PlayerRect : asd.GeometryObject2D
    {
        private asd.RectangleShape rectShape;
        public asd.Vector2DF lastPosition;
        public asd.Vector2DF velocity;
        public asd.Vector2DF direction;
        public float width;
        public float height;

        public PlayerRect()
        {
            width = 12;
            height = 60;

            rectShape = new asd.RectangleShape();
            rectShape.DrawingArea = new asd.RectF(-width/2, -height/2, width, height);

            Shape = rectShape;
            Color = new asd.Color(255, 255, 255, 128);
            direction = new asd.Vector2DF(1, 0);
            direction.Degree = Angle;
        }

        protected override void OnUpdate()
        {
            lastPosition = Position;
            Position = asd.Engine.Mouse.Position;
            velocity = (Position - lastPosition);
            if (velocity.Length > 5)
            {
                Angle = velocity.Degree;
            }
            direction.Degree = Angle;
            direction.Normalize();
        }

        public void MoveBack()
        {
            Position = lastPosition;
        }

        public void DeltaMove()
        {
            Position += velocity / 10;
        }
    }
}
