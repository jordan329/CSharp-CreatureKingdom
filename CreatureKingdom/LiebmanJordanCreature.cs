using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace CreatureKingdom {
    class LiebmanJordanCreature :Creature {
        

        Image creatureImage;
        BitmapImage letBit;
        BitmapImage rightBit;
        private Thread thread = null;
        private Boolean isRight = true;
        double creatureWidth = 356;
        double increment = 2.0;
        double kingdomWidth = 0;

        public LiebmanJordanCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime = 100): base(kingdom, dispatcher, waitTime) {
            creatureImage = new Image();
            letBit = LoadBitmap(@"LiebmanJordan\creatureLeft.png", creatureWidth);
            rightBit = LoadBitmap(@"LiebmanJordan\creatureRight.png", creatureWidth);
        }

        public override void Shutdown(){
            if (thread != null) {
                thread.Abort();
            }
        }

        public override void Place(double x, double y){
            creatureImage.Source = rightBit;
            isRight = true;

            this.x = x;
            this.y = y;
            kingdom.Children.Add(creatureImage);
            creatureImage.SetValue(Canvas.LeftProperty, this.x);
            creatureImage.SetValue(Canvas.TopProperty, this.y);

            thread = new Thread(Position);
            thread.Start();
        }

        void Position() {
            while (true) {
                if (isRight && !Paused) {
                    x += increment;
                    if (x > kingdomWidth) {
                        isRight = false;
                        SwitchBitmap(letBit);
                    }
                }
                else if (!Paused) {
                    x -= increment;
                    if (x < 0) {
                        isRight = true;
                        SwitchBitmap(rightBit);
                    }
                }
                if (kingdomWidth != kingdom.RenderSize.Width - creatureWidth){
                    kingdomWidth = kingdom.RenderSize.Width - creatureWidth;
                }
                UpdatePosition();
                Thread.Sleep(WaitTime);
            }
        }

        void UpdatePosition() {
            Action a = () => { creatureImage.SetValue(Canvas.LeftProperty, x); creatureImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.BeginInvoke(a);
        }

        void SwitchBitmap(BitmapImage map) {
            Action a = () => { creatureImage.Source = map; };
            dispatcher.BeginInvoke(a);
        }

        /*
        private void Size_Changed(SizeChangedEventArgs e) {
            Console.WriteLine("Window has been resized");
        }
         */
    }
}
