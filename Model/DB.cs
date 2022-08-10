using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoockData
{

    public class DB
    {
        private static DB _instance;
        private static Box[] _boxes;
        private readonly Configuration _config;
        private readonly double limitPercebtage ;
        private readonly int maxBoxesPerSize;
        private readonly int minBoxesPerSize;
        private readonly int maxDays ;

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

        public Configuration Config { get => _config; }
        public double LimitPercebtage => limitPercebtage;
        public int MaxBoxesPerSize => maxBoxesPerSize;
        public int MinBoxesPerSize => minBoxesPerSize;
        public int MaxDays => maxDays;

        private DB()
        {
            _config = new Configuration();

            limitPercebtage = Config.ConfigData.LIMIT_PERCENTAGE;
            maxBoxesPerSize = Config.ConfigData.MAX_BOXES_PER_SIZE;
            minBoxesPerSize = Config.ConfigData.MIN_BOXES_PER_SIZE;
            maxDays = Config.ConfigData.MAX_DAYS;

            Boxes = new Box[17];
            Boxes[0] = new Box(5, 5.5, 70);
            Boxes[1] = new Box(5, 6, 45);
            Boxes[2] = new Box(6, 6.5, 45);
            Boxes[3] = new Box(6, 9.5, 45);
            Boxes[4] = new Box(8, 8.5, 54);
/*            Boxes[5] = new Box(8, 2.5, 45);
            Boxes[0] = new Box(8, 5.5, 45);
            Boxes[1] = new Box(10, 6, 50);
            Boxes[2] = new Box(11, 6.5, 50);
            Boxes[3] = new Box(10, 7, 25);
            Boxes[4] = new Box(12, 7, 66);
            Boxes[5] = new Box(12, 6, 66);
            Boxes[6] = new Box(15, 8, 78);
            Boxes[7] = new Box(20.12, 10, 10);
            Boxes[8] = new Box(20.12, 12, 80);
            Boxes[9] = new Box(14, 15, 80);
            Boxes[10] = new Box(15, 15, 80);
            Boxes[11] = new Box(17, 17, 76);
            Boxes[12] = new Box(20, 20, 76);
            Boxes[13] = new Box(25, 20, 76);
            Boxes[14] = new Box(25, 25, 6);
            Boxes[15] = new Box(22, 18, 76);
            Boxes[16] = new Box(21, 16, 76);*/
        }
    }
}

public class Configuration
{
    public ConfigData ConfigData { get; private set; }

    public Configuration()
    {
        var currentDir = Environment.CurrentDirectory;
        var fileName = "ConfigData.json";
        var configPath = Path.Combine(currentDir, fileName);
        var raw = File.ReadAllText(configPath);
        ConfigData = JsonConvert.DeserializeObject<ConfigData>(raw);
    }
}

public class ConfigData
{
    public double LIMIT_PERCENTAGE { get; set; }
    public int MAX_BOXES_PER_SIZE { get; set; }
    public int MIN_BOXES_PER_SIZE { get; set; }
    public int MAX_DAYS { get; set; }

}

