using System;

namespace VitalsChecker
{

    internal abstract class Alert
    {
           internal abstract void sendAlert(string vitalName,string message);
    }

    class AlertInSms : Alert
    {
 
        internal override void  sendAlert(string vitalName,string message)
        {
            Console.WriteLine("SMS sent - " + vitalName + " " + message);
        }
    }

    class AlertinSound : Alert
    {
        internal override void sendAlert(string vitalName, string message)
        {
            Console.WriteLine("Sound Alert - " + vitalName + " " + message);
        }

    }
}
