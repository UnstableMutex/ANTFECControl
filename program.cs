using ANT_Managed_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static byte[] networkKey = { 0xB9, 0xA5, 0x21, 0xFB, 0xBD, 0x72, 0xC3, 0x45 };
        //   static byte[] networkKey = { 0xE8, 0xE4, 0x21, 0x3B, 0x55, 0x7A, 0x67, 0xC1 };
        //  static byte[] networkKey = { 0xA8, 0xA4, 0x23, 0xB9, 0xF5, 0x5E, 0x63, 0xC1 };

        static void Main(string[] args)
        {
            Console.WriteLine("Connecting to HRM, PRESS ENTER TO TERMINATE PROGRAM");

            var device = new ANT_Device();

            ANT_Common.enableDebugLogs();

            var ressnk = device.setNetworkKey(0, networkKey, 500);
            var caps = device.getDeviceCapabilities();
            ANT_Channel channel0 = device.getChannel(0);
            var acres = channel0.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 500);

            var schres = channel0.setChannelID(0, false, 120, 0, 500);

            var schperres = channel0.setChannelPeriod((ushort)8070, 500);
            channel0.setChannelSearchTimeout(15);

            var scfq = channel0.setChannelFreq(57, 500);
            var sps = channel0.setProximitySearch(3, 500); //Is set to limit the search range
            channel0.channelResponse += Channel_channelResponse;
            var oc = channel0.openChannel(500);
            Console.ReadKey();

        }
        private static void Channel_channelResponse(ANT_Response response)
        {
            //var rid = response.getMessageID();
            //Console.WriteLine(rid);
            try
            {
                var m = response.messageContents;
                string s = "";

                var d = response.timeReceived;
                var hr = response.messageContents[8].ToString();


                Console.WriteLine(hr);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("errror");
                //  throw;
            }

        }
    }
}
