using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetectionAndRecognition
{
    public partial class Form1 : Form
    {
        HaarCascade faceDetected;
        Image<Bgr, Byte> Frame;
        Capture camera;
        Image<Gray, Byte> result;
        Image<Gray, Byte> trainedFace = null;
        Image<Gray, Byte> grayFace = null;
        List<Image<Gray, Byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> users = new List<string>();
        int count, numLabels, t;
        string name, names = null;

        public Form1()
        {
            InitializeComponent();
            faceDetected = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                string labelsInfo = File.ReadAllText(Application.StartupPath + "Faces/Faces.txt");
                string[] Labels = labelsInfo.Split(',');
                numLabels = Convert.ToInt16(Labels[0].Length);
                count = numLabels;
                string facesLoad;
                for (int i = 1; i < numLabels + 1; i++)
                {
                    facesLoad = "face" + i + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath+"/Faces/Faces.txt"));
                    labels.Add(Labels[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Nothing found.");
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            camera = new Capture();
            camera.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            users.Add("");
            Frame = camera.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            grayFace = Frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = grayFace.DetectHaarCascade(faceDetected,1.2,10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
        }
    }
}
