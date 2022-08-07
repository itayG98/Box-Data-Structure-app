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
            Boxes = new Box[32];
            Boxes[0] = new Box(16, 13.3, 10, new DateTime(2021, 5, 15));
            Boxes[1] = new Box(9, 2, 40, new DateTime(2022, 5, 15));
            Boxes[2] = new Box(9, 10, 50, new DateTime(2022, 7, 15));
            Boxes[3] = new Box(13, 7, 30, new DateTime(2022, 5, 15));
            Boxes[4] = new Box(10, 10, 10, new DateTime(2022, 5, 15));
            Boxes[5] = new Box(15, 6, 20, new DateTime(2022, 5, 15));
            Boxes[6] = new Box(15, 8, 10, new DateTime(2022, 5, 15));
            Boxes[7] = new Box(20, 10, 10, new DateTime(2022, 5, 15));
            Boxes[8] = new Box(20, 2, 1000, new DateTime(2022,3, 15));
            Boxes[9] = new Box(20, 7, 10, new DateTime(2022, 4, 15));
            Boxes[10] = new Box(20, 2, 10, new DateTime(2022, 5, 15));
            Boxes[11] = new Box(20, 7, 560, new DateTime(2022, 5, 15));
            Boxes[12] = new Box(20, 10, 40, new DateTime(2022, 5, 15));
            Boxes[13] = new Box(25, 2, 40, new DateTime(2022, 5, 15));
            Boxes[14] = new Box(25, 8, 30, new DateTime(2022, 7, 15));
            Boxes[15] = new Box(30, 10, 30, new DateTime(2022, 7, 15));
            Boxes[16] = new Box(5, 2, 10, new DateTime(2022, 10, 15));
            Boxes[17] = new Box(3, 2, 10, new DateTime(2022, 5, 1));
            Boxes[18] = new Box(5, 2, 100, new DateTime(2022, 5, 2));
            Boxes[19] = new Box(3, 2, 100, new DateTime(2022, 5, 2));
            Boxes[20] = new Box(18, 18, 100, new DateTime(2022, 5, 1));
            Boxes[21] = new Box(2, 2, 100, new DateTime(2022, 5, 1));
            Boxes[22] = new Box(16, 11, 50, new DateTime(2022, 5, 1));
            Boxes[23] = new Box(17, 12, 100, new DateTime(2022, 5, 9));
            Boxes[24] = new Box(20, 13, 100, new DateTime(2022, 5, 9));
            Boxes[25] = new Box(22, 11, 100, new DateTime(2022, 5, 5));
            Boxes[26] = new Box(17, 13.4, 100, new DateTime(2022, 5, 5));
            Boxes[27] = new Box(16, 13.34, 100, new DateTime(2022, 5, 5));
            Boxes[28] = new Box(9, 2, 100, new DateTime(2022, 2, 15));
            Boxes[29] = new Box(7.7, 7.7, 45, new DateTime(2022, 2, 15));
            Boxes[30] = new Box(8.8, 13.34, 23, new DateTime(2022, 2, 15));
            Boxes[31] = new Box(4, 4, 67, new DateTime(2022, 5, 15));
        }
    }
}
