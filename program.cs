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

           // AddHRHandler(device);
            AddCadenceHandler(device);
            AddSpeedHandler(device);
            AddSCHandler(device);
            Console.ReadKey();


        }

 private static void AddSCHandler(ANT_Device device)
        {
            ANT_Channel hrChannel = device.getChannel(3);
            var acres = hrChannel.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 500);

            var schres = hrChannel.setChannelID(0, false, 121, 0, 500);

            var schperres = hrChannel.setChannelPeriod((ushort)8086, 500);
            hrChannel.setChannelSearchTimeout(15);

            var scfq = hrChannel.setChannelFreq(57, 500);
            var sps = hrChannel.setProximitySearch(3, 500); //Is set to limit the search range
            hrChannel.channelResponse += Channel_SCResponse;
            var oc = hrChannel.openChannel(500);
        }

        private static void Channel_SCResponse(ANT_Response response)
        {
            var bytes = response.messageContents;
            string s=string.Empty;
            foreach (var item in bytes)
            {
                s += " " + item.ToString();
            }
            Console.WriteLine("SpeedC: " + s);
        }

        private static void AddSpeedHandler(ANT_Device device)
        {
            ANT_Channel hrChannel = device.getChannel(1);
            var acres = hrChannel.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 500);

            var schres = hrChannel.setChannelID(0, false, 123, 0, 500);

            var schperres = hrChannel.setChannelPeriod((ushort)8118, 500);
            hrChannel.setChannelSearchTimeout(15);

            var scfq = hrChannel.setChannelFreq(57, 500);
            var sps = hrChannel.setProximitySearch(3, 500); //Is set to limit the search range
            hrChannel.channelResponse += Channel_SpeedResponse;
            var oc = hrChannel.openChannel(500);
        }

        private static void Channel_SpeedResponse(ANT_Response response)
        {
            var bytes = response.messageContents;
            string s=string.Empty;
            foreach (var item in bytes)
            {
                s += " " + item.ToString();
            }
            Console.WriteLine("Speed: " + s);
        }

        private static void AddCadenceHandler(ANT_Device device)
        {
            ANT_Channel hrChannel = device.getChannel(2);
            var acres = hrChannel.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 500);

            var schres = hrChannel.setChannelID(0, false, 122, 0, 500);

            var schperres = hrChannel.setChannelPeriod((ushort)8102, 500);
            hrChannel.setChannelSearchTimeout(15);

            var scfq = hrChannel.setChannelFreq(57, 500);
            var sps = hrChannel.setProximitySearch(3, 500); //Is set to limit the search range
            hrChannel.channelResponse += Channel_CadResponse;
            var oc = hrChannel.openChannel(500);
        }
        private static void AddHRHandler(ANT_Device device)
        {
            ANT_Channel hrChannel = device.getChannel(0);
            var acres = hrChannel.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 500);

            var schres = hrChannel.setChannelID(0, false, 120, 0, 500);

            var schperres = hrChannel.setChannelPeriod((ushort)8070, 500);
            hrChannel.setChannelSearchTimeout(15);

            var scfq = hrChannel.setChannelFreq(57, 500);
            var sps = hrChannel.setProximitySearch(3, 500); //Is set to limit the search range
            hrChannel.channelResponse += Channel_HRResponse;
            var oc = hrChannel.openChannel(500);
        }





        private static void Channel_CadResponse(ANT_Response response)
        {
            //try
            //{
            if (response.messageContents.GetUpperBound(0)==2)
            {
                Console.WriteLine("error");
                return;
            }
            var usPreviousTime1024 = (byte)(response.messageContents[4]);
            usPreviousTime1024 = (byte)(usPreviousTime1024 + (byte)(response.messageContents[5]));
            var usPreviousEventCount = (byte)(response.messageContents[6]);
            usPreviousEventCount = (byte)(usPreviousEventCount + (byte)(response.messageContents[7]));

            Console.WriteLine("prevtime: " + usPreviousTime1024);
            Console.WriteLine("prevevents: " + usPreviousEventCount);
            //}
            //catch (Exception)
            //{

            //    Console.WriteLine("cad error");
            //}


//            If(usEventCount > usPreviousEventCount) Then
//            usEventDiff = usEventCount - usPreviousEventCount
//       Else
//       usEventDiff = (&HF000; -usPreviousEventCount + usEventCount + 1)
//ulAcumEventCount += usEventDiff
//       End If
       
//' Update cumulative time (1/1024s)
//       If(usTime1024 > usPreviousTime1024) Then
//       usTimeDiff1024 = usTime1024 - usPreviousTime1024
//       Else
//       usTimeDiff1024 = (&HFFFF; -usPreviousTime1024 + usTime1024 + 1)
//ulAcumTime1024 += usTimeDiff1024
//       End If
       


//' Calculate cadence (rpm)
//       If(usTimeDiff1024 > 0) Then
//       'ucCadence = ((usEventDiff * &HF000;) / (usTimeDiff1024)) ' // 1 min = 0xF000 = 60 * 1024 
//ucCadence = CULng((usEventDiff) * &HF000;) / (usTimeDiff1024)
//Else
//       End If
       
        }
        private static void Channel_HRResponse(ANT_Response response)
        {

            try
            {
                var m = response.messageContents;
                string s = "";

                var d = response.timeReceived;
                var hr = response.messageContents[8].ToString();


                Console.WriteLine("HR: " + hr);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("errror");
                //  throw;
            }

        }
    }
}
