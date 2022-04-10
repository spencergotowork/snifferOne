using System;
using System.Windows.Forms;

namespace WinformsExample
{
    public static class PcapFilter
    {
        //ÉèÖÃ¹ýÂËÆ÷
        public static System.Collections.Generic.List<string> proto_filter = new System.Collections.Generic.List<string>();

        public static System.Collections.Generic.List<string> addr_filter = new System.Collections.Generic.List<string>();

        public static System.Collections.Generic.List<string> port_filter = new System.Collections.Generic.List<string>();

        public static string total_filter = "";
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainCaptureForm = new CaptureForm();
            Application.Run(mainCaptureForm);
        }
    }
}
