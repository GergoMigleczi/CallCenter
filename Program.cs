using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CallCenter
{
    struct Call
    {
        public int startHour;
        public int startMinute;
        public int startSecond;
        public int endHour;
        public int endMinute;
        public int endSecond;

        public Call(int startHour, int startMinute, int startSecond, int endHour, int endMinute, int endSecond)
        {
            this.startHour = startHour;
            this.startMinute = startMinute;
            this.startSecond = startSecond;
            this.endHour = endHour;
            this.endMinute = endMinute;
            this.endSecond = endSecond;
        }

    }
    class CallCenter
    {
        static List<Call> calls = new List<Call>();

        //Task1: make a method that converts hour:minute:second into seconds
        static int ConvertIntoSeconds(int h, int m, int s)
        {
            int ConvertIntoSeconds = h * 3600 + m * 60 + s;
            return ConvertIntoSeconds;
        }

        //Task 2: Read and store the data of hivas.txt
        /*
         txt file looks like:
            7 57 36 7 59 59  ((hour minute second hour minute second)
       call (start) (end))

            7 58 5 8 1 39
            7 58 33 7 58 47
            8 0 1 8 4 17
            8 0 21 8 2 13
            ....
            ..

         */
        static void Task2()
        {
            StreamReader sr = new StreamReader("hivas.txt");

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split();
                int startHour = int.Parse(line[0]);
                int startMinute = int.Parse(line[1]);
                int startSecond = int.Parse(line[2]);
                int endHour = int.Parse(line[3]);
                int endMinute = int.Parse(line[4]);
                int endSecond = int.Parse(line[5]);

                Call item = new Call(startHour, startMinute, startSecond, endHour, endMinute, endSecond);
                calls.Add(item);
            }

            sr.Close();
        }

        //Task 3: Print the number of calls in each hour. Do not print the hours with zero calls.
        static void Task3()
        {
            Console.WriteLine("Task 3");
            int numberOfCalls;
            List<int> hoursWithCalls = new List<int>();

            foreach (Call item in calls)
            {
                if (!hoursWithCalls.Contains(item.startHour))
                {
                    hoursWithCalls.Add(item.startHour);
                }
            }
            hoursWithCalls.Sort();
            foreach (int hour in hoursWithCalls)
            {
                numberOfCalls = 0;
                foreach (Call item in calls)
                {
                    if (hour == item.startHour)
                        numberOfCalls++;
                }
                Console.WriteLine($"{hour} hour: {numberOfCalls} calls");
            }
        }

        //Task 4: Print the longest call contained by the txt regardless if it was recieved or not
        static void Task4()
        {
            Console.WriteLine("Task 4");

            int numberOfCalls = 0;
            int longestCall = 0;
            int callSerialNumber = 0;
            foreach (Call item in calls)
            {
                numberOfCalls++;
                if (ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) - ConvertIntoSeconds(item.startHour, item.startMinute, item.startSecond) > longestCall)
                {
                    longestCall = ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) - ConvertIntoSeconds(item.startHour, item.startMinute, item.startSecond);
                    callSerialNumber = numberOfCalls;
                }
            }
            Console.WriteLine($"Number {callSerialNumber} was the longest call. It was {longestCall} second long.");
        }

        //Task 5: Ask the user to type in a time between working hours. (The task sheet states that working hours are between 8 and 12)
        //        Print the number of the caller who was on the line at that time, and print how many people were waiting to be answered
        static void Task5()
        {
            Console.WriteLine("Task 5");
            Console.Write("Type in a time! With space between each (hour minute second) ");
            string[] time = Console.ReadLine().Split();

            int numberOfCalls = 0;
            int callSerialNumber = 0;
            int startHour = 0;
            int startMinute = 0;
            int startSecond = 0;

            int endHour = 0;
            int endMinute = 0;
            int endSecond = 0;
            int waitingInLine = 0;
            bool someoneWasInLine = false;
            foreach (Call item in calls)
            {
                numberOfCalls++;
                if (ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) > ConvertIntoSeconds(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])) && ConvertIntoSeconds(item.startHour, item.startMinute, item.startSecond) < ConvertIntoSeconds(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])))
                {
                    someoneWasInLine = true;
                    callSerialNumber = numberOfCalls;
                    startHour = item.startHour;
                    startMinute = item.startMinute;
                    startSecond = item.startSecond;

                    endHour = item.endHour;
                    endMinute = item.endMinute;
                    endSecond = item.endSecond;

                    break;

                }
            }
            foreach (Call item in calls)
            {
                if (ConvertIntoSeconds(item.startHour, item.startMinute, item.startSecond) > ConvertIntoSeconds(startHour, startMinute, startSecond) && ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) <= ConvertIntoSeconds(endHour, endMinute, endSecond))
                {
                    waitingInLine++;
                }
            }
            if (someoneWasInLine)
            {
                Console.WriteLine($"People on hold: {waitingInLine}.\nNumber {callSerialNumber}. was on call at that time.");
            }
            else
            {
                Console.WriteLine("Nobody was on call");
            }

        }

        //Task 6: Print the caller's serial number who was on call for the last time of the day
        //        The shift ends at 12:00
        //        Also print how long was the last caller waiting to get in line
        static void Task6()
        {
            Console.WriteLine("Task 6");

            int numberOfCalls = 0;
            int lastCaller = 0;
            int waitingTime = 0;
            int previousInLine = 0;
            foreach (Call item in calls)
            {
                numberOfCalls++;
                if (item.startHour <= 11 && ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) > previousInLine)
                {
                    lastCaller = numberOfCalls;
                    waitingTime = previousInLine - ConvertIntoSeconds(item.startHour, item.startMinute, item.startSecond);
                }
                if (ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) > previousInLine)
                {
                    previousInLine = ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond);
                }
            }
            Console.WriteLine($"Number {lastCaller} was the last caller on call. They waited {waitingTime} second");
        }

        //Task 7: List the answered calls in the sikeres.txt
        //        Working hours between 8 and 12
        static void Task7()
        {
            StreamWriter sw = new StreamWriter("sikeres.txt");

            int numberOfCalls = 0;
            int previousInLine = 0;
            foreach (Call item in calls)
            {
                numberOfCalls++;

                if (item.endHour >= 8 && item.startHour <= 11 && ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond) > previousInLine)
                {
                    sw.WriteLine($"{numberOfCalls} {item.startHour} {item.startHour} {item.startSecond} {item.endHour} {item.endMinute} {item.endSecond}");
                    previousInLine = ConvertIntoSeconds(item.endHour, item.endMinute, item.endSecond);
                }

            }

            sw.Flush();
            sw.Close();
        }
        static void Main(string[] args)
        {
            Task2();
            Task3();
            Console.WriteLine();
            Task4();
            Console.WriteLine();
            Task5();
            Console.WriteLine();
            Task6();
            Task7();
            Console.ReadKey();
        }
    }
}
