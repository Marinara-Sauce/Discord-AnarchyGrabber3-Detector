using System;
using System.IO;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace DiscordAnarchyGrabberDetector
{
    class Program
    {
        static string[] blacklist = { "mfa", "process.env.mfa", "modDir", "inject", "4n4rchy", "anarchy", "hook" }; //Phrases to ban from being in the index.js file
        static string[] dirBlacklist = { "4n4rchy" }; //Names of folders to ban from inside the desktop-core folder (Couldn't think of anything else)

        static string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        static string discordRoamingPath = roamingPath + @"\Discord\";
        static string discordLocalPath = localPath + @"\Discord\";

        static string discordIndexPath = discordRoamingPath + @"0.0.306\modules\discord_desktop_core\index.js";
        static string discord4n4chyPath = discordRoamingPath + @"0.0.306\modules\discord_desktop_core\4n4rchy";

        static void Main(string[] args)
        {
            bool running = true;

            Console.WriteLine(roamingPath);
            Console.WriteLine(localPath);

            while (running)
            {
                check();
                Thread.Sleep(10000);
            }

        }

        static void check()
        {
            bool identified = false;

            Console.WriteLine("Checking for modifications...");

            string indexText = File.ReadAllText(discordIndexPath);
            //Console.WriteLine("Contents of index file: " + indexText);

            //Check the blacklist
            for (int i = 0; i < blacklist.Length; i++)
            {
                if (indexText.Contains(blacklist[i]))
                {
                    identified = true;

                    //Modifies index.js back to original
                    Console.WriteLine("Identified malicious code, removing...");
                    File.WriteAllText(discordIndexPath, "module.exports = require('./core.asar');"); //Modifies text back to original
                }
            }

            //Check for presence of trojan directory
            if (Directory.Exists(discord4n4chyPath))
            {
                identified = true;
                Directory.Delete(discord4n4chyPath, true); //Removes the trojan directory
            }

            if (identified)
            {
                DialogResult res = MessageBox.Show("The AnarchyGrabber3 Detector has identified suspicious file modifications to the discord directory." +
                    " They have been reverted/removed, but it is highly recommended to uninstall and reinstall discord." +
                    "\nClicking \"Yes\" will cause this program to remove discord and all it's files" +
                    "\nClicking \"No\" will bypass this message, NOT RECOMMENDED!", "Potential Grabber Detected!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (res == DialogResult.Yes)
                {
                    //Double Check
                    DialogResult res2 = MessageBox.Show("All files in the discord directory will be removed, you will have to login again. Are you sure?", "Remove Discord?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (res2 == DialogResult.Yes)
                    {
                        //Start Discord Uninstaller
                        Console.WriteLine("Removing Discord...");

                        //Terminate it
                        foreach (var process in Process.GetProcessesByName("discord"))
                        {
                            process.Kill();
                        }

                        Thread.Sleep(5000);

                        //Remove Files
                        if (Directory.Exists(discordRoamingPath))
                            Directory.Delete(discordRoamingPath, true);

                        if (Directory.Exists(discordLocalPath))
                            Directory.Delete(discordLocalPath, true);

                        //Bring user to discord website for easy reinstall
                        MessageBox.Show("Discord has been removed!\n" +
                            "Some files may be left over. Navigate to: " + discordLocalPath + " and " + discordRoamingPath + " and delete those directories.\n" +
                            "Opening up discord.com for easy reinstall...", "Reinstall Discord", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start("https://discord.com");
                        MessageBox.Show("This program will now terminate. Relaunch it after restarting discord!", "Reinstall Discord", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                }
            }
        }
    }
}
