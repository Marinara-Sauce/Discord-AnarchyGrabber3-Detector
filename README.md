# Discord AnarchyGrabber3 Detector
 A basic program that checks for the existance of the AnarchyGrabber3 trojan, and removes it
 
# What is AnarchyGrabber3?
 AnarchyGrabber3 is a trojan that was released last week. It's process is simple yet destructive. It modifies the index.js file inside discord to download a trojan that is injected into discord. The process can then steal the user's username and password, and spread the malware to the users friends list.
 More information on this is here: https://www.bleepingcomputer.com/news/security/discord-client-turned-into-a-password-stealer-by-updated-malware/
 
# How does this work?
 This program simply checks for disallowed words inside the index.js file, and resets the file accordingly. It also checks for the presence of directories created by Anarchy, and deletes them accordingly.
 If any of these are detected the program will prompt the user if they want to uninstall Discord. If clicking yes, it removes everything inside the discord roaming directory.
 
# Pre-reqs
 This is a very basic program. It was written in C# with Visual Studio Community 2019, Version 16.4.5
 Runs using the .NET Framework, Version 4.7.2
