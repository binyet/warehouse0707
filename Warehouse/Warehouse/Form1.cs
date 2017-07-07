using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Warehouse
{
    public partial class Form1 : Form
    {
        private List<int> angle_list = new List<int>();

        double CHECK_PERCENT_VALUE = 0.07;    //数据检测过滤前后点阈值
        float[] radius2_array;//根据高度进行过滤的角度值
        float[] height2_array;//根据高度进行过滤的高度值
        float[] distance2_array;//根据高度进行过滤的长度值
        float[] RadiusArrayDistance;//根据长度进行过滤的角度值
        float[] HeightArrayDistance;//根据长度进行过滤的高度值
        float[] DistanceArrayDistance;//根据长度进行过滤的长度值
        float diameter;
        float height_total = 0;
        float top_height = 0.3F;
        public Form1()
        {
            InitializeComponent();
            this.Text = "料仓测量数据模拟显示";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //showForm form = new showForm();
            //form.Show();
            this.Refresh();
            Graphics g = panel1.CreateGraphics();
            
            /**
            Pen myPen = new Pen(Color.Black, 4);
            Point p1 = new Point(40, 40);
            Point p2 = new Point(70, 20);
            Point p3 = new Point(110, 70);

            Point p4 = new Point(70, 130);
            Point[] points = { p1, p2, p3, p4 };
            g.DrawPolygon(myPen, points);
             * */
            float x_max = panel1.Width - 10;
            float y_max = panel1.Height - 10;
            System.Console.WriteLine("x-max:{0}\n", x_max);
            System.Console.WriteLine("y-max:{0}\n", y_max);
            float data_x_max = 0;
            float data_y_max = 0;
            float x_init = 0;


            string str_distance = textBox1.Text;//测量距离数据
            //string str_height = textBox2.Text;
            string str_x_init = textBox2.Text;
            string str_height_total = textBox4.Text;
            string str_top_height = textBox5.Text;

            try
            {
                diameter = float.Parse(textBox3.Text)*100;
            }
            catch
            {
                MessageBox.Show("请输入有效直径");
                return;
            }
            try
            {
                x_init = float.Parse(str_x_init)*100;
            }
            catch
            {
                MessageBox.Show("请输入有效距仓壁距离值");
                return;
            }
            try
            {
                height_total = float.Parse(str_height_total)*100;
            }
            catch
            {
                MessageBox.Show("请输入有效料仓高度值");
                return;
            }
            try
            {
                top_height = float.Parse(str_top_height) * 100;
            }
            catch
            {
                MessageBox.Show("请输入有效料仓高度值");
                return;
            }
            string[] distance_strarray = str_distance.Split(',');
            //string[] height_strarray = str_height.Split(' ');
            /**
            if (radius_strarray.Length != height_strarray.Length)
            {
                MessageBox.Show("半径数据必须和高度数据数目相同");
                return;
            }
             * */
            float[] distance_array = new float[distance_strarray.Length];
            distance2_array = new float[distance_strarray.Length];
            DistanceArrayDistance = new float[distance_strarray.Length];
            //float[] height_array = new float[height_strarray.Length];
            for (int i = 0; i < distance_strarray.Length; i++)
            {
                try
                {
                    distance_array[i] = float.Parse(distance_strarray[i].Trim());
                    /**
                    if (radius_array[i] > data_x_max)
                    {
                        data_x_max = radius_array[i];
                    }
                     * */
                }
                catch
                {
                    MessageBox.Show("距离数据有不合法数据！");
                    return;
                }
            }
            //计算半径值和物料高度值
            float[] radius_array = new float[distance_array.Length];
            float[] height_array = new float[distance_array.Length];
            radius2_array = new float[distance_array.Length];
            height2_array = new float[distance_array.Length];
            RadiusArrayDistance = new float[distance_array.Length];
            HeightArrayDistance = new float[distance_array.Length];
            if (angle_list.Count != 0)
            {
                for (int i = 0; i < distance_strarray.Length; i++)
                //将i改为真正获取的角度值
                {
                    radius_array[i] = distance_array[i] * (float)(Math.Sin(angle_list[i] * Math.PI / 180)) + x_init;
                    if (radius_array[i] > data_x_max)
                    {
                        data_x_max = radius_array[i];
                    }
                    height_array[i] = height_total - distance_array[i] * (float)(Math.Cos(angle_list[i] * Math.PI / 180));
                    if (height_array[i] > data_y_max)
                    {
                        data_y_max = height_array[i];
                    }
                }
            }

            //二次校验的高度和距离
            for (int i = 0; i < distance_strarray.Length; i++)
            {
                //radius2_array[i] = radius_array[i];
                height2_array[i] = height_array[i];
                distance2_array[i] = distance_array[i];
                HeightArrayDistance[i] = height_array[i];
                DistanceArrayDistance[i] = distance_array[i];
            }
            //根据长度进行数据检验
            DataCheck(angle_list[angle_list.Count - 1]);

            //根据高度进行数据检验
            DataCheckDistance(angle_list[angle_list.Count - 1]);

            //二次校验的半径
            if (angle_list.Count != 0)
            {
                for (int i = 0; i < distance_strarray.Length; i++)
                //将i改为真正获取的角度值
                {
                    radius2_array[i] = distance2_array[i] * (float)(Math.Sin(angle_list[i] * Math.PI / 180)) + x_init;
                    if (radius2_array[i] > data_x_max)
                    {
                        data_x_max = radius2_array[i];
                    }
                    height2_array[i] = height_total - distance2_array[i] * (float)(Math.Cos(angle_list[i] * Math.PI / 180));
                    if (height2_array[i] > data_y_max)
                    {
                        data_y_max = height2_array[i];
                    }
                }
            }

            //根据长度进行二次校验的半径
            if (angle_list.Count != 0)
            {
                for (int i = 0; i < distance_strarray.Length; i++)
                //将i改为真正获取的角度值
                {
                    RadiusArrayDistance[i] = DistanceArrayDistance[i] * (float)(Math.Sin(angle_list[i] * Math.PI / 180)) + x_init;
                    if (RadiusArrayDistance[i] > data_x_max)
                    {
                        data_x_max = radius2_array[i];
                    }
                    HeightArrayDistance[i] = height_total - DistanceArrayDistance[i] * (float)(Math.Cos(angle_list[i] * Math.PI / 180));
                    if (HeightArrayDistance[i] > data_y_max)
                    {
                        data_y_max = HeightArrayDistance[i];
                    }
                }
            }
            if (diameter > data_x_max)
            {
                data_x_max = diameter;
            }
            
            if (height_total > data_y_max)
            {
                data_y_max = height_total;
            }
            
            //画直径示意线
            Pen p = new Pen(Color.Green, 2);
            
            g.DrawLine(p, new PointF(0, y_max), new PointF(x_max, y_max));
            //画边框线
            p = new Pen(Color.Black, 1);
            //p.DashStyle =
            g.DrawLine(p, new PointF(0, 0), new PointF(0, y_max));
            g.DrawLine(p, new PointF(0, 0), new PointF(x_max, 0));
            g.DrawLine(p, new PointF(x_max, 0), new PointF(x_max, y_max));
            //画中心线
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawLine(p, new PointF(x_max/2, 0), new PointF(x_max/2, y_max));
            /**
            for (int i = 0; i < height_strarray.Length; i++)
            {
                height_array[i] = float.Parse(height_strarray[i]);
                if (height_array[i] > data_y_max)
                {
                    data_y_max = height_array[i];
                }
            }
             * */
            Font drawfont = new Font("宋体",8);
            SolidBrush drawbrush = new SolidBrush(Color.Black);
            //画一次校验线
            for (int i = 0; i < radius_array.Length;i++ )
            {
                String drawstring = "(" + radius_array[i] + "," + height_array[i] + ")";
                float x1 = radius_array[i];
                x1 = x1 * (x_max / data_x_max);
                float y1 = height_array[i];
                y1 = y_max-y1 * (y_max / data_y_max);
                System.Console.WriteLine("y1:{0}\n",y1);
                //g.DrawString(drawstring, drawfont, drawbrush, new PointF(x1, y1));
                if (i + 1 < radius_array.Length)
                {
                    float x2 = radius_array[i + 1];
                    x2 = x2 * (x_max / data_x_max);
                    float y2 = height_array[i + 1];
                    y2 = y_max - y2 * (y_max / data_y_max);
                    p = new Pen(Color.Blue, 2);
                    
                    g.DrawLine(p, new PointF(x1, y1), new PointF(x2, y2));

                }
            }
            Thread.Sleep(500);
            //画二次检验线
            for (int i = 0; i < radius2_array.Length; i++)
            {
                //String drawstring = "(" + radius2_array[i] + "," + height2_array[i] + ")";
                float x1 = radius2_array[i];
                x1 = x1 * (x_max / data_x_max);
                float y1 = height2_array[i];
                y1 = y_max - y1 * (y_max / data_y_max);
                //System.Console.WriteLine("y1:{0}\n", y1);
                //g.DrawString(drawstring, drawfont, drawbrush, new PointF(x1, y1));
                if (i + 1 < radius2_array.Length)
                {
                    float x2 = radius2_array[i + 1];
                    x2 = x2 * (x_max / data_x_max);
                    float y2 = height2_array[i + 1];
                    y2 = y_max - y2 * (y_max / data_y_max);
                    p = new Pen(Color.Red, 2);

                    g.DrawLine(p, new PointF(x1, y1), new PointF(x2, y2));
                    

                }
            }

            //Thread.Sleep(500);
            ////画根据长度检验的二次检验线
            //for (int i = 0; i < RadiusArrayDistance.Length; i++)
            //{
            //    //String drawstring = "(" + radius2_array[i] + "," + height2_array[i] + ")";
            //    float x1 = RadiusArrayDistance[i];
            //    x1 = x1 * (x_max / data_x_max);
            //    float y1 = HeightArrayDistance[i];
            //    y1 = y_max - y1 * (y_max / data_y_max);
            //    //System.Console.WriteLine("y1:{0}\n", y1);
            //    //g.DrawString(drawstring, drawfont, drawbrush, new PointF(x1, y1));
            //    if (i + 1 < RadiusArrayDistance.Length)
            //    {
            //        float x2 = RadiusArrayDistance[i + 1];
            //        x2 = x2 * (x_max / data_x_max);
            //        float y2 = HeightArrayDistance[i + 1];
            //        y2 = y_max - y2 * (y_max / data_y_max);
            //        p = new Pen(Color.Black, 2);

            //        g.DrawLine(p, new PointF(x1, y1), new PointF(x2, y2));


            //    }
            //}

        }

        //const double[] g_faCosTable={1.0000,0.9998,0.9994,0.9986,0.9976,0.9962,0.9945,0.9925,0.9903,0.9877,
        //                                                0.9848,0.9816,0.9781,0.9744,0.9703,0.9659,0.9613,0.9563,0.9511,0.9455,
        //                                                0.9397,0.9336,0.9272,0.9205,0.9135,0.9063,0.8988,0.8910,0.8829,0.8746,
        //                                                0.8660,0.8572,0.8480,0.8387,0.8290,0.8192,0.8090,0.7986,0.7880,0.7771,
        //                                                0.7660,0.7547,0.7431,0.7314,0.7193,0.7071,0.6947,0.6820,0.6691,0.6561,
        //                                                0.6428,0.6293,0.6157,0.6018,0.5878,0.5736,0.5592,0.5446,0.5299,0.5150,
        //                                                0.5000,0.4848,0.4695,0.4540,0.4384,0.4226,0.4067,0.3907,0.3746,0.3584,
        //                                                0.3420,0.3256,0.3090,0.2924,0.2756,0.2588,0.2419,0.2250,0.2079,0.1908,
        //                                                0.1736,0.1564,0.1392,0.1219,0.1045,0.0872,0.0698,0.0523,0.0349,0.0175};

        //Warehouse.ColumnHeight   仓库柱体高度
        //Warehouse.VertebralHeight  仓库下锥高度
        //上面两个变量之和，可以用程序里面的总高度来代替
        //Warehouse.TopHeight  距离顶端的高度														
				
        /**
          * @brief  数据有效性检验
	        * @param  angle 总测量角度值
          * @retval none
        */
        public void DataCheck(int angle)
        {//根据高度进行过滤
	        int i;
	        float average = 0;
	
	        //检验其余点正确性
	
	        //计算垂直高度平均值
	        for(i=0; i<angle; i++)
	        {
                
                //MeansureValue[i].CalcHeight = MeansureValue[i].MeansureLength*g_faCosTable[i];
		        average += height2_array[i];
	        }
	        average /= angle;
	
	        //进行数据检验，粗过滤
	        for(i=1; i<angle; i++)
	        {
                float heightAir_i = height_total - height2_array[i];//当前料面距顶的高度
                float heightAir_i_1 = height_total - height2_array[i - 1];//前一个点料面距顶的高度
                if (((heightAir_i > average * 3)//条件1：大于1.5倍平均值
                    && (heightAir_i > heightAir_i_1 * 2))//条件2：并且比前一个值的2倍还大
                    || (heightAir_i > height_total - top_height)//条件3：比仓库相对高度还大
		        )
		        {
                    
                    //height2_array[i] = height2_array[i-1];//使用前一个数据覆盖

                    float height_air = height_total - height2_array[i-1];

                    distance2_array[i] = (float)(height_air / Math.Cos(angle_list[i] * Math.PI / 180));

                    radius2_array[i] = (float)(distance2_array[i] * Math.Sin(angle_list[i]*Math.PI/180));
                    height2_array[i] = height_total - height_air;
                    //MessageBox.Show(angle_list[i]+"\r\n"+height_air.ToString()+"\r\n"+distance2_array[i].ToString()+"\r\n"+radius2_array[i].ToString());
		        }
	        }
	
            //进行数据滤波，细过滤
            for (i = 1; i <= angle; i++)
            {
                float heightAir_i = height_total - height2_array[i];//当前料面距顶的高度
                float heightAir_i_1 = height_total - height2_array[i - 1];//前一个点料面距顶的高度
                if ((heightAir_i > heightAir_i_1 * (1 + CHECK_PERCENT_VALUE))//比前一个值的1.07倍大
                    || (heightAir_i < heightAir_i_1 * (1 - CHECK_PERCENT_VALUE)))//比前一个0.93倍小
                {
                    //MessageBox.Show(i.ToString());
                    if (i == angle)//最后一个点，使用覆盖
                    {
                        //MessageBox.Show(height2_array[i].ToString());
                        float height_air = height_total - height2_array[i - 1];

                        distance2_array[i] = (float)(height_air / Math.Cos(angle_list[i] * Math.PI / 180));

                        radius2_array[i] = (float)(distance2_array[i] * Math.Sin(angle_list[i]*Math.PI/180));
                        height2_array[i] = height_total - height_air;
                        //MessageBox.Show(height_air.ToString()+"\r\n"+distance2_array[i].ToString() + "\r\n" + angle_list[i].ToString());
                        
                    }
                    else//其余点使用平均
                    {
                        distance2_array[i] = (distance2_array[i - 1] + distance2_array[i + 1]) / 2;//使用前后均值替换
                        height2_array[i] = (height2_array[i - 1] + height2_array[i + 1]) / 2;//使用前后均值替换
                        //MessageBox.Show(i + " " + distance2_array[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 根据长度值进行过滤
        /// </summary>
        /// <param name="angle"></param>
        public void DataCheckDistance(int angle)
        {//根据高度进行过滤
            int i;
            float average = 0;

            //检验其余点正确性

            //计算垂直高度平均值
            for (i = 0; i < angle; i++)
            {

                //MeansureValue[i].CalcHeight = MeansureValue[i].MeansureLength*g_faCosTable[i];
                average += HeightArrayDistance[i];
            }
            average /= angle;

            //进行数据检验，粗过滤
            for (i = 1; i < angle; i++)
            {
                if (((HeightArrayDistance[i] > average * 3)//条件1：大于3倍平均值
                    && (HeightArrayDistance[i] > HeightArrayDistance[i - 1] * 2))//条件2：并且比前一个值的2倍还大
                    || (HeightArrayDistance[i] > height_total - top_height)//条件3：比仓库相对高度还大
                )
                {
                    HeightArrayDistance[i] = HeightArrayDistance[i - 1];//使用前一个数据覆盖
                    //使用上一个点的高度==> 当前点的高度==》反推当前的长度==>反推半径

                    //DistanceArrayDistance[i] = h
                }
            }

            //进行数据滤波，细过滤
            for (i = 1; i < angle; i++)
            {
                if ((DistanceArrayDistance[i] > DistanceArrayDistance[i - 1] * (1 + CHECK_PERCENT_VALUE))//比前一个值的1.07倍大
                    || (DistanceArrayDistance[i] < DistanceArrayDistance[i - 1] * (1 - CHECK_PERCENT_VALUE)))//比前一个0.93倍小
                {
                    if (i == angle - 1)//最后一个点，使用覆盖
                    {
                        DistanceArrayDistance[i] = DistanceArrayDistance[i - 1];//使用前一个数据覆盖
                    }
                    else//其余点使用平均
                    {
                        DistanceArrayDistance[i] = (DistanceArrayDistance[i - 1] + DistanceArrayDistance[i + 1]) / 2;//使用前后均值替换
                        HeightArrayDistance[i] = (HeightArrayDistance[i - 1] + HeightArrayDistance[i + 1]) / 2;//使用前后均值替换
                        //MessageBox.Show(i + " " + distance2_array[i]);
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();   //显示选择文件对话框 
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath + "\\back_data";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                angle_list.Clear();
                int i = 0;
                StreamReader sr = File.OpenText(openFileDialog1.FileName);
                string line = "";
                int isfirst = 1;//判断是否是第一行，若不是，需要在前面添加','
                while ((line = sr.ReadLine()) != null)
                {
                    if (isfirst == 0)
                        textBox1.AppendText(",");
                    string[] data = line.Split(' ');
                    textBox1.AppendText(data[1]);
                    try
                    {
                        angle_list.Add(Int32.Parse(data[0]));
                        //textBox2.AppendText(angle_list[i].ToString());
                        i++;
                    }
                    catch (Exception exc)
                    {

                    }
                    isfirst = 0;
                }

                //for (int j = 0; j < angle_list.Count; j++)
                //{
                //    textBox2.AppendText(angle_list[j].ToString() + ",");
                //}
                //this.textBox1.Text = openFileDialog1.FileName;     //显示文件路径 
            } 

        }
    }
}
