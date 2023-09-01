
namespace WY_App.Utility
{

    public class Constructor
    {
        public class CameraParams
        {
            public double[] Gain = new double[4];
            public double[] Shutter = new double[4];
            public double[] Black_Level = new double[4];
            public double[] Gamma = new double[4];
            public string[] CameraID = new string[4];
            public CameraParams()
            {
                for (int i = 0; i < 4; i++)
                {
                    Gain[i] = 10;
                    Shutter[i] = 10;
                    Black_Level[i] = 0;
                    Gamma[i] = 0;
                    CameraID[i] = "Cam" + i;
                }
            }
        }
       
        
       
        public class Motor
        {
            public int HighSpeed;
            public int GoHomeSpeed;
            public int FirstPosition;
            public int HandStep;
            public int AutoStep;
            public Motor()
            {
                HighSpeed = 1000;
                GoHomeSpeed = 2000;
                FirstPosition = 50;
                HandStep = 200;
                AutoStep = 300;
            }
        }
    }
}
