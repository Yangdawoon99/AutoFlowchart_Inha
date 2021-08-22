using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ex
{
    public partial class Form1 : Form
    {

        //프로젝트 파일안에 있는 이미지 가져오기
        Bitmap start = new Bitmap("start.bmp");
        Bitmap card = new Bitmap("card.bmp");
        Bitmap decision = new Bitmap("decision.bmp");
        Bitmap document = new Bitmap("document.bmp");
        Bitmap end = new Bitmap("end.bmp");
        Bitmap input = new Bitmap("input.bmp");
        Bitmap output = new Bitmap("output.bmp");
        Bitmap process = new Bitmap("process.bmp");


        //
        private float pictureScale = 1.0f;
        private const int WORLD_WIDTH = 200;
        private const int WORLD_HEIGHT = 200;
        private Matrix inverseTransform = new Matrix();


        public Form1()
        {
            InitializeComponent();

        }
        private void SetScale(float scale) 
        { 
            this.pictureScale = scale; 
            this.pictureBox1.ClientSize = new Size((int)(WORLD_WIDTH * this.pictureScale), (int)(WORLD_HEIGHT * this.pictureScale)); 
            this.inverseTransform = new Matrix(); 
            this.inverseTransform.Scale(1.0f / scale, 1.0f / scale); 
            //this.pictureBox1.Refresh();
        }
        private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e) 
        { 
            switch (this.scaleComboBox.Text)
            { 
                case "x 1/4": SetScale(0.25f); break; 
                case "x 1/2": SetScale(0.5f); break; 
                case "x 1": SetScale(1.0f); break; 
                case "x 2": SetScale(2.0f); break; 
                case "x 4": SetScale(4.0f); break; 
                case "x 8": SetScale(8.0f); break; 
            } 
        }
        //화살표 그리는 함수
        public void Arrow(int x1, int y1, int x2, int y2) 
        {
            Graphics G = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);
            pen.StartCap = LineCap.ArrowAnchor;
            G.DrawLine(pen, x1, y1, x2, y2);

            G.Dispose();
        }

        public void Screen() //pictureBox1 캡쳐해서 저장하기
        {
            Bitmap bitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(PointToScreen(new Point(this.pictureBox1.Location.X, this.pictureBox1.Location.Y)), new Point(0, 0), this.pictureBox1.Size);
            bitmap.Save(@"C:\Temp\flowchart\EX_flowchart.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            //pictureBox1.Image.Save(@"C:\Temp\flowchart\EX_flowchart.png", System.Drawing.Imaging.ImageFormat.Png);
            MessageBox.Show("저장이 완료되었습니다.", "저장");

        }


        private void btn_Screen_Click(object sender, EventArgs e) //이미지 저장
        {
            //bmp1.Save(@"C:\Temp\flowchart\flowchart2.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //bmp2.Save(@"C:\Temp\flowchart\flowchart3.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            //Size sz = new Size(this.Bounds.Width, this.Bounds.Height);
            //FormCapture(sz, @"C:\Temp\flowchart\autoflowchart.jpg");

            //panel2를 비트맵으로 전환하여 이미지 저장
            //Bitmap plbmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //pictureBox1.DrawToBitmap(plbmp, new System.Drawing.Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

            Screen();
        }

        private void btn_Change_Click(object sender, EventArgs e)// 이미지 전환
        {
            Bitmap plbmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            //Graphics flowchart = Graphics.FromImage(plbmp);

            //패널에 이미지 그리기
            Graphics g = pictureBox1.CreateGraphics();

            //"시작 도형" 절반줄임
            int width = start.Width / 2;
            int height = start.Height / 2;
            Size resize = new Size(width, height);
            Bitmap resize_start = new Bitmap(start, resize);


            // ';'문자를 기준으로 텍스트 한줄씩 분해해서 label에 붙이기
            string[] allLines = textBox1.Text.Split(';');

            //"card" 절반줄임
            int c_width = 50;
            int c_height = 50;
            c_width = allLines[0+1].Length * 20;
            c_height = allLines[0+1].Length + 30;
            Size c_resize = new Size(c_width, c_height);
            Bitmap resize_card = new Bitmap(card, c_resize);

            ////"decision" 절반줄임
            //int d_width = allLines[1].Length * 20;
            //int d_height = allLines[1].Length + 30;
            //Size d_resize = new Size(d_width, d_height);
            //Bitmap resize_decision = new Bitmap(decision, d_resize);

            //"document" 절반줄임
            //Bitmap resize_document = new Bitmap(document, resize);

            //pictureBox.Location = new Point(x - pictureBox.Size.Width / 2, y - pictureBox.Size.Height / 2);
            
            
            //도형의 파일명 가져오기
            string filepath1 = @"C:\Users\didek\OneDrive\Desktop\ex (2)\bin\Debug\decision.bmp";
            string filepath2 = @"C:\Users\didek\OneDrive\Desktop\ex (2)\bin\Debug\document.bmp";
            //Console.WriteLine(Path.GetFileName(filepath1));

            //화살표 만들기
            int[] Point_Array = new int[4] { 0, 0, 0, 0 };

            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            int CenterX = 0;

            Boolean arrowistrue = false;


            if (arrowistrue == false) //시작도형 만들기
            {
                Point pt1 = new Point(150, 50);
                g.DrawImage(resize_start, pt1);//시작도형
                g.DrawString("시작", Font, Brushes.Black, 175, 65);

                x1 = 80 + resize_start.Width / 2;
                y1 = 50 + resize_start.Height;

                //Console.WriteLine("시작도형의 i: {0}", i);

                //가운데 정렬하기위한 중앙값 x좌표를 구하는 공식
                CenterX = pt1.X + resize_start.Width / 2;

                Point_Array[0] = CenterX;
                Point_Array[1] = y1;   //start도형이 가지고 있어야 하는 0,1좌표

                arrowistrue = true;
            }



            for (int i = 0; i < allLines.Length; i++)//textarea의 문자의 줄 수만큼 반복(Length의 반대)   
            {
                
                
                
                
                Point pt = new Point(80, 10 *(6 *(i+2)));//15*i 값 수정, 도형의 위치를 제대로 주기 위함
                // Point pt = new Point(80, 10 *(5 *(i+1)));

                // 스타트 도형-----------------------------------------------------------------

                if (arrowistrue == true) {

                    if (allLines[i].Contains("if"))//마름모꼴


                    {
                        Console.WriteLine("if도형의 i: {0}", i);
                        //각각 decision도형마다 resize
                        if (filepath1.Contains("decision"))
                        {

                            int count = 0;

                            int d_width = allLines[i + 1].Length * 20;
                            int d_height = allLines[i + 1].Length + 30;
                            Size d_resize = new Size(d_width, d_height);
                            Bitmap resize_decision = new Bitmap(decision, d_resize);


                            x2 = pt.X + resize_decision.Width / 2;//이미 변화한 좌표값
                            y2 = pt.Y;//화살표 그리기위한 좌표

                            Point pt2 = new Point(CenterX - (resize_decision.Width / 2), pt.Y);//두번째 도형을 그리는 가운데정렬 포인트, 그림그리기 위해선 포인트
                                                                                               //값이 필요하므로 임시로 그림을 위한 좌표값이다.

                            g.DrawImage(resize_decision, pt2);
                            g.DrawString(allLines[i + 1], Font, Brushes.Black, CenterX - (resize_decision.Width / 8), pt.Y);



                            Point_Array[2] = CenterX;
                            Point_Array[3] = y2;//첫번째 도형에서 두번째 도형까지의 도착

                            Console.WriteLine("PointA 2의 값 : {0}", Point_Array[2]);
                            Console.WriteLine("PointA 3의 값 : {0} \r\n", Point_Array[3]);

                            Arrow(Point_Array[2], Point_Array[3], Point_Array[0], Point_Array[1]);
                            Array.Clear(Point_Array, 0, 4);//배열 비우기
                            count++;

                            Console.WriteLine("0: {0} 비어야한다", Point_Array[0]);
                            Console.WriteLine("1: {0} 비어야한다", Point_Array[1]);
                            Console.WriteLine("2: {0} 비어야한다", Point_Array[2]);
                            Console.WriteLine("3: {0} 비어야한다", Point_Array[3]);

                            Point_Array[0] = CenterX;
                            Point_Array[1] = pt.Y + resize_decision.Height;//두번째 도형에서의 시작

                            Console.WriteLine("PointA 0의 값 : {0}", Point_Array[0]);
                            Console.WriteLine("PointA 1의 값 : {0}", Point_Array[1]);

                            Console.WriteLine("화살표가 그려진 횟수 : {0}", count);

                        }

                    }
                    //print문이 나오면 document도형 생성하기
                    else if (allLines[i].Contains("print"))//document도형
                    {
                        //각각 document도형마다 resize
                        if (filepath2.Contains("document"))
                        {
                            int dc_width = allLines[i + 1].Length * 20;
                            int dc_height = allLines[i + 1].Length + 30;
                            Size dc_resize = new Size(dc_width, dc_height);
                            Bitmap resize_document = new Bitmap(document, dc_resize);


                            //document 그림을그리기위한 pt값을 줘서 
                            Point pt3 = new Point(CenterX - (resize_document.Width / 2), pt.Y);


                            g.DrawImage(resize_document, pt3);//pt값을 적용
                                                              //g.DrawString(allLines[i + 1], Font, Brushes.Black, 70, i * 70);
                            g.DrawString(allLines[i + 1], Font, Brushes.Black, CenterX - (resize_document.Width / 8), pt.Y);


                            Point_Array[2] = CenterX;
                            Point_Array[3] = pt3.Y;
                            Arrow(Point_Array[2], Point_Array[3], Point_Array[0], Point_Array[1]);
                            Array.Clear(Point_Array, 0, 4);//배열 비우기

                            Point_Array[0] = CenterX;
                            Point_Array[1] = pt3.Y + resize_document.Height;


                        }

                    }
                    else if (allLines[i].Contains("for"))//card
                    {
                        Console.WriteLine("card도형");
                    }

                    else if (allLines[i].Contains("scanf"))//입력
                    {
                        Console.WriteLine("입출력도형");
                    }

                    else if (allLines[i].Contains("[]"))//배열선언
                    {
                        Console.WriteLine("준비도형");
                    }


                   
                }//arrowistrue가 true

                


            }//for문

        }

    }
}