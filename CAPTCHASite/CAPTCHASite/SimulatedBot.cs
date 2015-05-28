using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTCHASite
{
    public class SimulatedBot
    {
        private List<CaptchaBreaker> _CaptchaBreakerList = new List<CaptchaBreaker>();

        public List<CaptchaBreaker> CaptchaBreakerList
        {
            get { return _CaptchaBreakerList; }
            set { _CaptchaBreakerList = value; }
        }
    }

    public class CaptchaBreaker
    {
        private int _CaptchaID;
        private double _SuccessPercent;

        public int CaptchaID
        {
            get { return _CaptchaID; }
            set { _CaptchaID = value; }
        }

        public double SuccessPercent
        {
            get { return _SuccessPercent; }
            set { _SuccessPercent = value; }
        }
    }
}