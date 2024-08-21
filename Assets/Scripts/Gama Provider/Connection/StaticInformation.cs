using System.Net;
using System;
public static class StaticInformation
{
    public static string endOfGame { get; set; }
    private static string connectionId;

    public static string getId() {
        if (connectionId == null || connectionId.Length == 0)
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            string lastIP = myIP.Split(".")[3];
            if ("103".Equals(lastIP))
            {
                connectionId = "green - village 4";
            }
            else if ("104".Equals(lastIP))
            {
                connectionId = "red - village 1";

            }
            else if ("106".Equals(lastIP))
            {
                connectionId = "yellow - village 2";
            }

            else if ("15".Equals(lastIP))
            {
                connectionId = "blue - village 3";
            }
            else
            {
                connectionId = "green - village 4";
            }
            // connectionId =   lastIP;// + lastIP;
        }
        return connectionId;
    }
}
