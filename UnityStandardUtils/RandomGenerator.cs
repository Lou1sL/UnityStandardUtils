using System;

namespace UnityStandardUtils
{
    /// <summary>
    /// 服从特定数学模型下的随机数生成器
    /// </summary>
    public class RandomGenerator
    {
        static Random aa = new Random((int)(DateTime.Now.Ticks / 10000));
        /// <summary>
        /// 均匀分布
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public double AverageRandom(double min, double max)
        {
            int MINnteger = (int)(min * 10000);
            int MAXnteger = (int)(max * 10000);
            int resultInteger = aa.Next(MINnteger, MAXnteger);
            return resultInteger / 10000.0;
        }
        public int AverageRandom(int min, int max)
        {
            return aa.Next(min, max);
        }
        /// <summary>
        /// 正态分布
        /// </summary>
        /// <param name="miu"></param>
        /// <param name="sigma"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public double GaussianDist(double miu, double sigma, double min, double max)
        {
            double x;
            double dScope;
            double y;
            do
            {
                x = AverageRandom(min, max);
                y = GDFunc(x, miu, sigma);
                dScope = AverageRandom(0, GDFunc(miu, miu, sigma));
            } while (dScope > y);
            return x;
        }
        private double GDFunc(double x, double miu, double sigma) //正态分布概率密度函数
        {
            return 1.0 / (x * Math.Sqrt(2 * Math.PI) * sigma) * Math.Exp(-1 * (Math.Log(x) - miu) * (Math.Log(x) - miu) / (2 * sigma * sigma));
        }

        /// <summary>
        /// 指数分布
        /// </summary>
        /// <param name="lambda">参数</param>
        /// <returns></returns>
        public double ExponentialDist(double lambda)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            double p;
            double temp;
            if (lambda != 0)
                temp = 1 / lambda;
            else
                throw new InvalidOperationException("除数不能为零！不能产生参数为零的指数分布！");
            double randres;
            while (true) //用于产生随机的密度，保证比参数λ小
            {
                p = rand.NextDouble();
                if (p < lambda)
                    break;
            }
            randres = -temp * Math.Log(temp * p, Math.E);
            return randres;
        }

        Random ran;
        public RandomGenerator()
        {
            ran = new Random();
        }
        /// <summary>
        /// 负指数分布
        /// </summary>
        /// <param name="lambda">参数</param>
        /// <returns></returns>
        public double NegativeExponentialDist(double lambda)
        {
            double dec = ran.NextDouble();
            while (dec == 0)
                dec = ran.NextDouble();
            return -Math.Log(dec) / lambda;
        }
        
        /// <summary>
        /// 泊松分布
        /// </summary>
        /// <param name="lambda">参数</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public double PoissonDist(double lambda, double time)
        {
            int count = 0;

            while (true)
            {
                time -= NegativeExponentialDist(lambda);
                if (time > 0)
                    count++;
                else
                    break;
            }
            return count;
        }

        public DateTime RandomDate(DateTime min,DateTime max)
        {
            int rangeSecound = (int)(max - min).TotalSeconds;
            return min.AddSeconds(AverageRandom(0, rangeSecound));
        }


    }
}
