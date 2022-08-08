using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoockData
{
    class DB
    {
        private static DB _instance;
        private static Box[] _boxes;

        public static Box[] Boxes { get => _boxes; set => _boxes = value; }
        public static DB Instance
        //SingleTone
        {
            get
            {
                if (_instance == null)
                    _instance = new DB();
                return _instance;
            }
        }
        private DB()
        {
            Boxes = new Box[36];

            Boxes[0] = new Box(5, 5.5, 25);
            Boxes[1] = new Box(5, 6, 25);
            Boxes[2] = new Box(5, 6.5, 25);
            Boxes[3] = new Box(5, 9.5, 25);
            Boxes[4] = new Box(5, 2.5, 25);
            Boxes[5] = new Box(5, 2.5, 25);
            Boxes[0] = new Box(5, 5.5, 25);
            Boxes[1] = new Box(5, 6, 25);
            Boxes[2] = new Box(5, 6.5, 25);
            Boxes[3] = new Box(5, 1.5, 25);
            Boxes[4] = new Box(5, 3.5, 25);
            Boxes[5] = new Box(5, 3.5, 25);
            Boxes[6] = new Box(15, 8, 10);
            Boxes[7] = new Box(20, 10, 10);
            Boxes[8] = new Box(20, 2, 1000);
            Boxes[9] = new Box(20, 7, 10);
            Boxes[10] = new Box(20, 2, 10);
            Boxes[11] = new Box(20, 7, 560);
            Boxes[12] = new Box(20, 10, 40);
            Boxes[13] = new Box(25, 2, 40);
            Boxes[14] = new Box(25, 8, 30);
            Boxes[15] = new Box(30, 10, 30);
            Boxes[16] = new Box(5, 2, 10);
            Boxes[17] = new Box(3, 2, 10);
            Boxes[18] = new Box(5, 2, 100);
            Boxes[19] = new Box(3, 2, 100);
            Boxes[20] = new Box(18, 18, 100);
            Boxes[21] = new Box(2, 2, 100);
            Boxes[22] = new Box(16, 11, 50);
            Boxes[23] = new Box(17, 12, 100);
            Boxes[24] = new Box(20, 13, 100);
            Boxes[25] = new Box(22, 11, 100);
            Boxes[26] = new Box(17, 13.4, 100);
            Boxes[27] = new Box(16, 13.34, 100);
            Boxes[28] = new Box(9, 2, 100);
            Boxes[29] = new Box(7.7, 7.7, 45);
            Boxes[30] = new Box(12, 13.34, 23);
            Boxes[31] = new Box(14, 13.4, 100);
            Boxes[32] = new Box(16, 13.34, 100);
            Boxes[33] = new Box(18, 10, 100);
            Boxes[34] = new Box(20, 11, 45);
            Boxes[35] = new Box(24, 19, 23);
        }
    }
}
