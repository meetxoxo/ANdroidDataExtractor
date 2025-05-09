using System;
using System.Collections.Generic;
using System.Diagnostics;
using AndroidDataExtractor.Models;

namespace AndroidDataExtractor.Services
{
    public class ADBDataExtractor
    {
        private string RunAdbCommand(string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "adb",
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                return process.StandardOutput.ReadToEnd();
            }
        }

        public List<Contact> GetContacts()
        {
            string output = RunAdbCommand("shell content query --uri content://contacts/phones/");
            // Very basic mock parser (real parsing needs regex or structured parser)
            var contacts = new List<Contact>();
            foreach (var line in output.Split('\n'))
            {
                if (line.Contains("Number="))
                {
                    contacts.Add(new Contact
                    {
                        Name = "Unknown",
                        PhoneNumber = ExtractField(line, "Number="),
                        Email = ""
                    });
                }
            }
            return contacts;
        }

        public List<Message> GetMessages()
        {
            string output = RunAdbCommand("shell content query --uri content://sms/");
            var messages = new List<Message>();
            foreach (var line in output.Split('\n'))
            {
                if (line.Contains("address=") && line.Contains("body="))
                {
                    messages.Add(new Message
                    {
                        Sender = ExtractField(line, "address="),
                        MessageContent = ExtractField(line, "body="),
                        Timestamp = DateTime.Now.ToString() // placeholder
                    });
                }
            }
            return messages;
        }

        public List<CallLog> GetCallLogs()
        {
            string output = RunAdbCommand("shell content query --uri content://call_log/calls/");
            var callLogs = new List<CallLog>();
            foreach (var line in output.Split('\n'))
            {
                if (line.Contains("number=") && line.Contains("type=") && line.Contains("duration="))
                {
                    callLogs.Add(new CallLog
                    {
                        PhoneNumber = ExtractField(line, "number="),
                        CallType = ParseCallType(ExtractField(line, "type=")),
                        Duration = ExtractField(line, "duration=")
                    });
                }
            }
            return callLogs;
        }

        public DeviceInfo GetDeviceInfo()
        {
            string cpuInfo = RunAdbCommand("shell cat /proc/cpuinfo");
            string memInfo = RunAdbCommand("shell cat /proc/meminfo");
            return new DeviceInfo
            {
                CPUInfo = cpuInfo,
                MemoryInfo = memInfo
            };
        }

        private string ExtractField(string line, string key)
        {
            int start = line.IndexOf(key);
            if (start < 0) return "";
            start += key.Length;
            int end = line.IndexOf(' ', start);
            return end > start ? line.Substring(start, end - start).Trim() : line[start..].Trim();
        }

        private string ParseCallType(string type)
        {
            return type switch
            {
                "1" => "Incoming",
                "2" => "Outgoing",
                "3" => "Missed",
                _ => "Unknown"
            };
        }
    }
}
