using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTCHASite
{
    public class Captcha
    {

        private int _CaptchaID = 0;
        private int _AttemptCount = 0;
        private int _SuccessfulAttemptCount;
        private List<object> _CaptchaControlList = new List<object>();
        private bool _IsActiveCaptcha;
        private string _Description;

        public int CaptchaID
        {
            get { return _CaptchaID; }
            set { _CaptchaID = value; }
        }
        public int AttempCount
        {
            get { return _AttemptCount; }
            set { _AttemptCount = value; }
        }
        public int SuccessfulAttemptCount
        {
            get { return _SuccessfulAttemptCount; }
            set { _SuccessfulAttemptCount = value; }
        }
        public List<object> CaptchaControlList
        {
            get { return _CaptchaControlList; }
            set { _CaptchaControlList = value; }
        }
        public bool IsActiveCaptcha
        {
            get { return _IsActiveCaptcha; }
            set
            {
                _IsActiveCaptcha = value;


                foreach (object o in _CaptchaControlList)
                {
                    if (o.GetType() == typeof(ASPNET_Captcha.ASPNET_Captcha))
                    {
                        ASPNET_Captcha.ASPNET_Captcha tempObj = (ASPNET_Captcha.ASPNET_Captcha)o;
                        tempObj.Visible = _IsActiveCaptcha;
                    }
                    else if (o.GetType() == typeof(MSCaptcha.CaptchaControl))
                    {
                        MSCaptcha.CaptchaControl tempObj = (MSCaptcha.CaptchaControl)o;
                        tempObj.Visible = _IsActiveCaptcha;
                    }
                }
            }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public Double GetRatio()
        {
            Double attempts = Convert.ToDouble(_AttemptCount);
            Double successfulAttempts = Convert.ToDouble(_SuccessfulAttemptCount);

            Double Ratio;

            if (attempts == 0)
                Ratio = 0.5;
            else
                Ratio = successfulAttempts / attempts;

            return Ratio;
        }
        




    }
}