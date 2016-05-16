//This code is copyright (c) LeagueSharp 2015. Please do not remove this line.

using System;
using System.Diagnostics;
using System.IO;
using EloBuddy;
using LeagueSharp.Common;

namespace PortAIO.Utility.ChatLogger
{
    public static class Program
    {
        public static string LogFile;
        public static Stopwatch Stopwatch;
       public static void Main(/*string[] args*/)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                var AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var EloBuddyPath = AppDataPath + "\\EloBuddy\\";

                //Define the logfile location
                LogFile = EloBuddyPath + "\\Chat Logs\\" + DateTime.Now.ToString("yy-MM-dd") + " " + DateTime.Now.ToString("HH-mm-ss") + " - " + ObjectManager.Player.ChampionName + ".txt";
                
                //Create a stopwatch which we will use to emulate in-game time.
                Stopwatch = new Stopwatch();
                Stopwatch.Start();

                //Create the AppData Directory, if it doesn't exist.
                if (!Directory.Exists(EloBuddyPath + "\\Chat Logs\\"))
                {
                    Directory.CreateDirectory(EloBuddyPath + "\\Chat Logs\\");
                }

                //Show the user a message
                Chat.Print("The chat log for this game can be found at " + LogFile);
                Chat.Print(Game.IP + ":" + Game.Port);

                //Subscribe to OnChat to do the magic
                Chat.OnMessage += OnChat;
            };
        }

        private static void OnChat(AIHeroClient sender,ChatMessageEventArgs args)
        {
            if (!File.Exists(LogFile))
            {
                File.Create(LogFile);
            }

            using (var sw = new StreamWriter(LogFile, true))
            {
                //store the current stopwatch millisecond for accurate results
                long elapsedTime = Stopwatch.ElapsedMilliseconds;
                //compute elapsed minutes
                long elapsedMinutes = elapsedTime/60000;
                //create a variable to store the seconds in
                long elapsedSeconds = 0;
                //compute the elapsed seconds and store it in the variable previously created
                Math.DivRem(elapsedTime, 60000, out elapsedSeconds);
                elapsedSeconds /= 1000;
                
                //write everything to the stream
                sw.WriteLine("[" + elapsedMinutes + ":" + (elapsedSeconds < 10 ? "0" : "") + elapsedSeconds + "] " + args.Sender.Name + " (" + args.Sender.ChampionName + "): " + args.Message);
                //close the stream
                sw.Close();
            }
        }
    }
}
