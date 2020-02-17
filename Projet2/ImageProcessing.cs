using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Projet2
{
    class ImageProcessing
    {
        //TEST TES TEST
        private VideoCapture _capture = null;
        private bool CaptureInProgress;
        private bool RetreivedFrame = false;
        private Mat Frame;
        private Mat GrayFrame;
        public Mat CannyFrame;

        public ImageProcessing()
        {
            CvInvoke.UseOpenCL = false;
            try
            {
                // Select camera
                // 0 => first camera usually Laptop one
                // 1 => second camera => External USB one
                _capture = new VideoCapture(0);
                // Set Capture property Width + Height
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 480);
                // Set the function linked to ImageGrabbed event
                _capture.ImageGrabbed += GrabProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            Frame = new Mat();
            GrayFrame = new Mat();
            CannyFrame = new Mat();
        }

        private void GrabFrame()
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(Frame, 0);
                // Set flag to TRUE
                RetreivedFrame = true;

            }
        }

        private void ProcessFrame()
        {
            if (RetreivedFrame)
            {
                // Convert BGR image to Gray
                CvInvoke.CvtColor(Frame, GrayFrame, ColorConversion.Bgr2Gray);
                // Contour finding with Canny Edge Detector filter
                CvInvoke.Canny(GrayFrame, CannyFrame, 100, 60);
                // Associate Images with GUI
                //ImageBox.Image = CannyFrame;
                // Toggle Flag
                RetreivedFrame = !RetreivedFrame;
            }

        }

        public void FindCircle()
        {
            Console.WriteLine(CvInvoke.FitEllipse(CannyFrame));
        }

        private void GrabProcessFrame(object sender, EventArgs arg)
        {
            GrabFrame();
            ProcessFrame();
            FindCircle();
        }

        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }
    }
}
