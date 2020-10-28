using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace T_Core
{
    [TestClass]
    public class OpenCVTest
    {
        [TestMethod]
        public void ShowImage()
        {
            byte[] img = File.ReadAllBytes(Path.Combine("input", "220px-Lenna_(test_image).png"));
            //Mat src = Mat.FromImageData(img, ImreadModes.Grayscale);
            Mat src = Mat.ImDecode(img, ImreadModes.Grayscale);

            Cv2.ImShow("Data", src);

            int keyPressed = Cv2.WaitKey(0);
            if (keyPressed == 27)
            {
                Cv2.DestroyAllWindows();
            }
        }
    }
}
