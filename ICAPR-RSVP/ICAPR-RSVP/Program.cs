using System;

using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP
{
    class Program
    {

        static void Main(string[] args)
        {
            Broker.Broker broker = new Broker.BrokerEyeTribeSpritz();
            Broker.Port input = new Broker.PortInputEyeTribe();
            broker.AddInput(input);
            Broker.Port output = new Broker.PortOutputMergedData();
            broker.AddOutput(output);
            broker.Start();

            Item item;
            for (int i = 0; i < 50; i++)
            {
                item = broker.getItem(output.ID);
                if (item.Type == ItemTypes.EyesData)
                {
                    Eyes eyes = (Eyes)item.Value;
                    Console.WriteLine("Timestamp:" + eyes.Timestamp + 
                        " Left" + eyes.PupilSizeLeft + " Right" + eyes.PupilSizeRight);
                }
            }

            broker.Stop();
        }
    }
}
