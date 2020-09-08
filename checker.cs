using System;
using System.Collections.Generic;

    internal abstract class Alert
    {
        internal abstract void sendAlert(string vitalName, string message);
    }

    class AlertInSms : Alert
    {

        internal override void sendAlert(string vitalName, string message)
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

    class Vital
    {
        string vitalName;
        int lowerLimit, upperLimit;
        internal int vitalValue;

        internal string VitalName
        {
            get { return this.vitalName; }
        }

        internal int LowerLimit
        {
            get { return this.lowerLimit; }
        }

        internal int UpperLimit
        {
            get { return this.upperLimit; }
        }

        internal Vital(string name, int lower, int upper)
        {
            vitalName = name;
            lowerLimit = lower;
            upperLimit = upper;
        }
    }

    class Checker
    {
        static bool vitalIsOk(Alert alert,string vitalName,float value,int lower,int upper)
        {
            if(value<lower)
            {
                alert.sendAlert(vitalName,"is low");
                return false;
            }
            else if(value > upper)
            {
                alert.sendAlert(vitalName,"is high");
                return false;
            }
            return true;
        }

        static bool vitalsAreOk(Alert alert, List<Vital> vitalsList)
        {
            bool check = true;
            for (int i = 0; i < vitalsList.Count; i++)
            {
                if (!vitalIsOk(alert, vitalsList[i].VitalName, vitalsList[i].vitalValue, vitalsList[i].LowerLimit, vitalsList[i].UpperLimit))
                    check = false;
            }
            return check;
        }

        static void setVitalValue(List<Vital> vitalsList, Dictionary<String, int> vitalsIndex,string name,int value)
        {
            vitalsList[ vitalsIndex[name] - 1 ].vitalValue = value;
        }

        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                Console.WriteLine("Expected true, but got false");
                Environment.Exit(1);
            }
        }

        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                Console.WriteLine("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        static int Main()
        {
            AlertInSms smsAlert = new AlertInSms();
            AlertinSound soundAlert= new AlertinSound();
            
            ExpectTrue(vitalIsOk(smsAlert, "Test", 5, 2, 8));
            ExpectFalse(vitalIsOk(smsAlert, "Test", 5, 6, 8));
            ExpectFalse(vitalIsOk(soundAlert, "Test", 5, 2, 4));

            Dictionary<String, int> vitalsIndex = new Dictionary<string, int>();

            List<Vital> vitalsList = new List<Vital>();

            vitalsList.Add(new Vital("bpm", 70, 150));
            vitalsIndex.Add("bpm", 1);

            vitalsList.Add(new Vital("spo2", 90, 250));
            vitalsIndex.Add("spo2", 2);

            vitalsList.Add(new Vital("respRate", 30, 95));
            vitalsIndex.Add("respRate", 3);

            setVitalValue(vitalsList,vitalsIndex,"bpm", 95);
            setVitalValue(vitalsList,vitalsIndex, "spo2", 100);
            setVitalValue(vitalsList,vitalsIndex, "respRate", 70);
            ExpectTrue(vitalsAreOk(smsAlert, vitalsList));

            setVitalValue(vitalsList, vitalsIndex, "bpm", 60);
            setVitalValue(vitalsList, vitalsIndex, "spo2", 80);
            setVitalValue(vitalsList, vitalsIndex, "respRate", 40);
            ExpectFalse(vitalsAreOk(smsAlert, vitalsList));

            setVitalValue(vitalsList, vitalsIndex, "bpm", 170);
            setVitalValue(vitalsList, vitalsIndex, "spo2", 95);
            setVitalValue(vitalsList, vitalsIndex, "respRate", 100);
            ExpectFalse(vitalsAreOk(smsAlert, vitalsList));

            setVitalValue(vitalsList, vitalsIndex, "bpm", 80);
            setVitalValue(vitalsList, vitalsIndex, "spo2", 85);
            setVitalValue(vitalsList, vitalsIndex, "respRate", 40);
            ExpectFalse(vitalsAreOk(soundAlert, vitalsList));

            setVitalValue(vitalsList, vitalsIndex, "bpm", 80);
            setVitalValue(vitalsList, vitalsIndex, "spo2", 70);
            setVitalValue(vitalsList, vitalsIndex, "respRate", 20);
            ExpectFalse(vitalsAreOk(soundAlert, vitalsList));

            setVitalValue(vitalsList, vitalsIndex, "bpm", 80);
            setVitalValue(vitalsList, vitalsIndex, "spo2", 95);
            setVitalValue(vitalsList, vitalsIndex, "respRate", 100);
            ExpectFalse(vitalsAreOk(soundAlert, vitalsList));

            Console.WriteLine("All ok");
            return 0;
        }
    }
