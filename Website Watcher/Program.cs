using System;
using System.Collections.Generic;
using System.Timers;
using System.Reflection.Emit;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;


namespace Website_Watcher
{
    class Watcher
    {
        public static StringBuilder currentSource = new StringBuilder();

        public static void Main()
        {
            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler( DisplayTimeEvent );
            myTimer.Interval = 60000;// 60000; //300 000
            myTimer.Start();
  
            while ( Console.Read() != 'q' )
            {
                ;    // do nothing...
            }
        }

        public static void DisplayTimeEvent( object source, ElapsedEventArgs e )
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create("http://www.bottomofthehill.com/calendar.html");

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            // print out page source

            if (sb.ToString() == currentSource.ToString())
                Console.WriteLine(DateTime.Now.ToShortTimeString() + " No Change");
            else
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + " Change!!!!!!!!!!!!!!!!!!!!");
                try
                {
                    SendEmail();
                } catch(Exception ex) {}
                currentSource = new StringBuilder(sb.ToString());
            }
        }

        public static void SendEmail()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("xxxxxxxx@gmail.com", "XXXXXXXXXXXXXXXX"),
                EnableSsl = true
            };
            client.Send("xxxxxxxxxxxxx@gmail.com", "xxxxxxxxxx@gmail.com", "Maybe Tickets", "Maybe Tickets");
        }
    }
}
