using Avalonia.Metadata;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1
{
    public class Log
    {
        private readonly StreamWriter streamWriter;
        public Log() 
        {
            DateTime dateTime = DateTime.Now;
            string fileName = "log_" + dateTime.Day + "_" + dateTime.Month + "_" + dateTime.Year + ".txt";
            if (!File.Exists(fileName))
            {
                streamWriter = File.CreateText(fileName);
            }
            else
            {
                streamWriter = File.AppendText(fileName);
            }

        }
        public void StartLogging()
        {
            streamWriter.WriteLine("[New program log]");
        }
        public void AddIDLogging(IDUpdateArgs args)
        {
            streamWriter.WriteLine("Changed object ID from [" + args.ObjectID + "] to [" + args.NewObjectID + "]");
        }
        public void AddPositionLogging(PositionUpdateArgs args)
        {
            streamWriter.WriteLine("Changed object [" + args.ObjectID + "] position to [" + args.Latitude + "] latitude, [" + args.Longitude + "] longtitude, [" + args.AMSL + "] AMSL");
        }
        public void AddContactInfoLogging(ContactInfoUpdateArgs args)
        {
            streamWriter.WriteLine("Changed object [" + args.ObjectID + "] contact info to [" + args.EmailAddress + "] email, [" + args.PhoneNumber + "] phone number");
        }
        public void AddErrorLogging(ulong ObjectID)
        {
            streamWriter.WriteLine("Cannot change object [" + ObjectID + "] info");
        }
        public void FinishLogging()
        {
            streamWriter.Close();
        }
    }
}
