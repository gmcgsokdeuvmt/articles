using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamejam4
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Altseedを初期化する。
            asd.Engine.Initialize("gamejam4", 1000, 800, new asd.EngineOption());

            int frameCount = 0;
            asd.Scene scene = new asd.Scene();
            
            //// layers
            //layers[0]: mainLayer
            //layers[1]: subLayer
            for (int i = 0; i < 2; i++)
            {
                scene.AddLayer(new asd.Layer2D());
            }


            EventManager eventManager = new EventManager(scene); 
            asd.Engine.ChangeScene(scene);

            // Altseedのウインドウが閉じられていないか確認する。
            while (asd.Engine.DoEvents())
            {
                // Altseedを更新する。
                asd.Engine.Update();

                // 当たり判定を処理する。
                eventManager.updateByCollision();

                // フレーム数による更新
                eventManager.updateByFrame(frameCount++);
            }

            // Altseedの終了処理をする。
            asd.Engine.Terminate();
        }
    }
}
