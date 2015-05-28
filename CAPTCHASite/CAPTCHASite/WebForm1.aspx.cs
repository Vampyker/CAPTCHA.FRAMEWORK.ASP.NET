using System.Drawing;
using System.IO;
using System;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CAPTCHASite
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private List<Captcha> CaptchaList = new List<Captcha>();
        private SimulatedBot SimulatedTestingBot = new SimulatedBot();
        private int ActiveCaptchaID = 0;
        
        private Random rand = new Random();

        private double MaxPercentFromActiveBeforeReset = .8;
        private int MinSamplesBeforeReset = 200;
        private int StartingDataMissBuffer = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeAllCaptchas();
            InitializeActiveCaptcha();
            EnforceLiveData();
            InitializeSimulatedBot();
            DisplayCaptchaData();
        }

        protected void InitializeAllCaptchas()
        {

            string path = Server.MapPath("/");
            path += "\\CaptchaData.txt";
            
            if (File.Exists(path))
            {
                CaptchaList = new List<Captcha>();
                String CaptchaFileText = File.ReadAllText(path);
                String[] SingleCaptchaArray = CaptchaFileText.Split('\n');
                foreach (String SingleCaptcha in SingleCaptchaArray)
                {
                    String[] SingleValueArray = SingleCaptcha.Replace("\"", "").Split(',');
                    
                    //skip description row
                    if (SingleValueArray.Length == 5 && SingleValueArray[0] == "ID")
                        continue;

                    Captcha cData = new Captcha();
                    cData.CaptchaID = int.Parse(SingleValueArray[0]);
                    cData.CaptchaControlList = GetCaptchaControlListFromID(cData.CaptchaID);
                    cData.IsActiveCaptcha = false;
                    cData.Description = SingleValueArray[1];
                    cData.SuccessfulAttemptCount = int.Parse(SingleValueArray[2]);
                    cData.AttempCount = int.Parse(SingleValueArray[3]);
                    CaptchaList.Add(cData);
                    
                }

            }
            else
            {
                //Math Captcha
                Captcha cMath = new Captcha();
                cMath.CaptchaID = 1;
                cMath.CaptchaControlList = GetCaptchaControlListFromID(cMath.CaptchaID);
                cMath.IsActiveCaptcha = false;
                cMath.Description = "Math Captcha";
                CaptchaList.Add(cMath);

                //Image Captcha
                Captcha cImage = new Captcha();
                cImage.CaptchaID = 2;
                cImage.CaptchaControlList = GetCaptchaControlListFromID(cImage.CaptchaID);
                cImage.IsActiveCaptcha = false;
                cMath.Description = "Image Captcha";
                CaptchaList.Add(cImage);
            }
        }

        protected void EnforceLiveData()
        {

            int TotalAllCaptchaAttempts = 0;
            double activeCaptchaSuccessRate = 0;
            foreach(Captcha c in CaptchaList)
            {
                TotalAllCaptchaAttempts += c.AttempCount;
                if (c.CaptchaID == ActiveCaptchaID)
                {
                    activeCaptchaSuccessRate = c.GetRatio();
                }
            }

            if (MinSamplesBeforeReset < TotalAllCaptchaAttempts && activeCaptchaSuccessRate > MaxPercentFromActiveBeforeReset)
                ResetData(null, null);
        }

        protected List<object> GetCaptchaControlListFromID(int CaptchaID)
        {
            List<object> cControlList = new List<object>();

            if (CaptchaID == 1)
                cControlList.Add(ucCaptcha);
            else if (CaptchaID == 2)
                cControlList.Add(msCaptcha);

            return cControlList;
        }

        protected void InitializeSimulatedBot()
        {
            //Add Math Captcha Breaker
            CaptchaBreaker cbMath = new CaptchaBreaker();
            cbMath.CaptchaID = 1;
            cbMath.SuccessPercent = .55;
            SimulatedTestingBot.CaptchaBreakerList.Add(cbMath);

            //Add Image Captcha Breaker
            CaptchaBreaker cbImage = new CaptchaBreaker();
            cbImage.CaptchaID = 2;
            cbImage.SuccessPercent = .45;
            SimulatedTestingBot.CaptchaBreakerList.Add(cbImage);
                
        }

        protected void InitializeActiveCaptcha()
        {
            int captchaID = 0;
            double lowestCaptchaRatio = 1;
            Captcha ActiveCaptcha = null;

            foreach (Captcha c in CaptchaList)
            {
                if (c.GetRatio() < lowestCaptchaRatio || captchaID == 0)
                {
                    captchaID = c.CaptchaID;
                    lowestCaptchaRatio = c.GetRatio();
                    ActiveCaptcha = c;
                }
            }

            ActiveCaptchaID = captchaID;

            if (ActiveCaptcha != null)
            {
                ActiveCaptcha.IsActiveCaptcha = true;
                
                foreach (Captcha c in CaptchaList)
                {
                    if (c.CaptchaID != ActiveCaptcha.CaptchaID)
                    {
                        c.IsActiveCaptcha = false;
                    }
                }
            }

        }

        protected void DisplayCaptchaData()
        {
            foreach (Captcha c in CaptchaList)
            {
                if (c.CaptchaID == 1)
                {
                    txtSuccess1.Text = c.SuccessfulAttemptCount.ToString();
                    txtTotal1.Text = c.AttempCount.ToString();
                    txtRatio1.Text = c.GetRatio().ToString();
                }
                else if (c.CaptchaID == 2)
                {
                    txtSuccess2.Text = c.SuccessfulAttemptCount.ToString();
                    txtTotal2.Text = c.AttempCount.ToString();
                    txtRatio2.Text = c.GetRatio().ToString();
                }
            }
        }

        protected void Submit(object sender, EventArgs e)
        {
            if (txtCaptcha.Text != null && ActiveCaptchaID > 0)
            {
                bool isValid =false;
                string captchaInput = txtCaptcha.Text.Trim();
                if (ActiveCaptchaID == 1)
                {
                    isValid = ucCaptcha.Validate(captchaInput);
                }
                else if(ActiveCaptchaID == 2)
                {
                    msCaptcha.ValidateCaptcha(captchaInput);
                    isValid = msCaptcha.UserValidated;
                }

                if (isValid)
                {
                    lblMessage.Text = "Valid!";
                    lblMessage.ForeColor = Color.Green;
                }
                else
                {
                    lblMessage.Text = "Invalid!";
                    lblMessage.ForeColor = Color.Red;
                }

                CaptchaAttempt(isValid, ActiveCaptchaID);
                SaveCaptchaData();
                InitializeActiveCaptcha();
            }
            DisplayCaptchaData();
        }

        protected void ResetData(object sender, EventArgs e)
        {
            String InitialDataString = "\"ID\",\"Description\",\"Success\",\"Total\",\"Ratio\"\n\"1\",\"Math Captcha\",\"0\",\"" + StartingDataMissBuffer + "\",\"0.5\"\n\"2\",\"Image Captcha\",\"0\",\"" + StartingDataMissBuffer + "\",\"0.5\"";
            string path = Server.MapPath("/");
            path += "\\CaptchaData.txt";

            if (File.Exists(path))
                File.Delete(path);
            
            File.WriteAllText(path, InitialDataString);
            InitializeAllCaptchas();
            InitializeActiveCaptcha();
            DisplayCaptchaData();
        }

        protected void CaptchaAttempt(bool IsSuccess, int CaptchaId)
        {
            foreach (Captcha c in CaptchaList)
            {
                if (c.CaptchaID == CaptchaId)
                {
                    c.AttempCount++;
                    if (IsSuccess)
                        c.SuccessfulAttemptCount++;
                    break;
                }
            }
        }

        protected void PerformTest(object sender, EventArgs e)
        {
            int testAmount = 0;

            int.TryParse(txtTestLength.Text,out testAmount);

            if (testAmount < 1)
                testAmount = 1;

            for (int i = 0; i < testAmount; i++)
            {
                InitializeActiveCaptcha();
                SingleTest();
            }

            SaveCaptchaData();
            DisplayCaptchaData();
        }

        protected void SingleTest()
        {
            //obtain a random number to simulate the bot's success chance
            
            int testNumber = rand.Next(1,100);
            double testNumberPercent = Convert.ToDouble(testNumber);
            testNumberPercent = testNumberPercent / 100;

            //obtain the simulated bot's % success rate
            double BotSuccessChance = 0;
            foreach (CaptchaBreaker cb in SimulatedTestingBot.CaptchaBreakerList)
            {
                if (cb.CaptchaID == ActiveCaptchaID)
                {
                    BotSuccessChance = cb.SuccessPercent;
                    break;
                }
            }

            //does the bot successfully break the captcha?
            bool IsCaptchaBroken = (BotSuccessChance >= testNumberPercent);

            //record the test
            CaptchaAttempt(IsCaptchaBroken, ActiveCaptchaID);
        }

        protected void SaveCaptchaData()
        {
            
            string path = Server.MapPath("/");
            path += "\\CaptchaData.txt";

            if (File.Exists(path))
                File.Delete(path);
            //File.Create(path);

            String descriptionString = "\"ID\",\"Description\",\"Success\",\"Total\",\"Ratio\"\n";
            String fileData = descriptionString;
            
            foreach(Captcha c in CaptchaList)
            {
                fileData += "\"" + c.CaptchaID +  "\",";
                fileData += "\"" + c.Description +  "\",";
                fileData += "\"" + c.SuccessfulAttemptCount +  "\",";
                fileData += "\"" + c.AttempCount +  "\",";
                fileData += "\"" + c.GetRatio() +  "\"\n";
            }

            if (fileData.Substring(fileData.Length - 1, 1) == "\n")
                fileData = fileData.Substring(0, fileData.Length - 1);

            File.WriteAllText(path, fileData);

            /*
            
            "ID","Description","Success","Total","Ratio"
            "1","Math Captcha","0","0","0.5"
            "2","Image Captcha","0","0","0.5"
            
            */

        }
    }
}