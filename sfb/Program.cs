using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfb
{
    class Program
    {
        static void Main(string[] args)
        {
            
                if (args.Length < 1)
                {
                    PrintHelp();
                }
                else
                {
                    Console.WriteLine("[.] Opening file {0}...", args[0]);
                    Loadsfb(args[0]);
                }
            }

        static void PrintHelp()
            {
                Console.WriteLine("sfb.exe (PS3_DISC.SFB)");
            }

        public static char[] bytesToChar(byte[] b)
        {
            char[] c = new char[b.Length];
            for (int i = 0; i < b.Length; i++)
                c[i] = (char)b[i];
            return c;
        }

        public static byte[] charsToByte(char[] b)
        {
            byte[] c = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
                c[i] = (byte)b[i];
            return c;
        }

        static void Loadsfb(string file)
        {
            SFB sfb = new SFB();

            using (FileStream SourceStream = File.OpenRead(file))
            {

                sfb.load(SourceStream);

            }

            Console.WriteLine("[*] .SFB: {0}", new string(bytesToChar(sfb._SFB)));
            Console.WriteLine("[*] Version: {0}", BitConverter.ToString(sfb.Version).Replace("-", "").TrimStart('0'));
            // Console.WriteLine("[*] Un1: {0}", BitConverter.ToString(sfb.Un1).Replace("-", ""));
            Console.WriteLine("[*] HYBRID_FLAG: {0}", new string(bytesToChar(sfb.H_F)));
            Console.WriteLine("[*] Disc Content Data Offset: {0}", BitConverter.ToString(sfb.dcdo).Replace("-", "").TrimStart('0'));
            Console.WriteLine("[*] Disc Content Data Length: {0}", BitConverter.ToString(sfb.dcdl).Replace("-", "").TrimStart('0'));
            // Console.WriteLine("[*] Un2: {0}", BitConverter.ToString(sfb.Un2).Replace("-", ""));
            Console.WriteLine("[*] TITLE_ID: {0}", new string(bytesToChar(sfb.Tid)));
            Console.WriteLine("[*] Disc Title Data Offse: {0}", BitConverter.ToString(sfb.dtdo).Replace("-", "").TrimStart('0'));
            Console.WriteLine("[*] Disc Title Data Length: {0}", BitConverter.ToString(sfb.dtdl).Replace("-", "").TrimStart('0'));
            //Console.WriteLine("[*] Un3: {0}", BitConverter.ToString(sfb.Un3).Replace("-", ""));
            Console.WriteLine("[*] Disc Content: {0}", new string(bytesToChar(sfb.Flags)));
            Console.WriteLine("[*] Disc Title: {0}", new string(bytesToChar(sfb.Title)));
            //Console.WriteLine("[*] Un4: {0}", BitConverter.ToString(sfb.Un4).Replace("-", ""));

            string input;
            Console.WriteLine("Edit Flags Y/N");
            input = Console.ReadLine();
            if (input == "Y" || input == "y")
            {
                Console.WriteLine("[*] Disc Content: {0}", new string(bytesToChar(sfb.Flags)));
                Console.WriteLine("New Flags");
                byte[] nflags = new byte[0x20];
                string sflags = Console.ReadLine();
                byte[] nflags2 = charsToByte(sflags.ToCharArray());

                if (nflags2.Length <= 0x20)
                {
                    Console.WriteLine("New Flags =" + sflags);
                     int i = 0;
                    while (i <= nflags2.Length - 1)
                    {
                        nflags[i] = nflags2[i];
                        i++;
                    }
                    while (i > nflags2.Length - 1 && i <= nflags.Length - 1)
                    {
                        nflags[i] = 0;
                        i++;
                    }
                    sfb.Flags = nflags;
                    Console.WriteLine("[*] Disc Content: {0}", new string(bytesToChar(sfb.Flags)));

                }



                else
                {
                    Console.WriteLine("Error New Flags to long");
                }


            }

            Console.WriteLine("Edit TITLE_ID Y/N");
            input = Console.ReadLine();
            if (input == "Y" || input == "y")
            {
                Console.WriteLine("[*] Disc Title: {0}", new string(bytesToChar(sfb.Title)));
                Console.WriteLine("New TITLE_ID");
                byte[] nTitle = new byte[0x10];
                string sTitle = Console.ReadLine();
                byte[] nTitle2 = charsToByte(sTitle.ToCharArray());
                if (nTitle2.Length <= 0x10)
                {
                    Console.WriteLine("New TITLE_ID =" + sTitle);
                   int i = 0;
                    while (i <= nTitle2.Length - 1)
                    {
                        nTitle[i] = nTitle2[i];
                        i++;
                    }
                    while (i > nTitle2.Length - 1 && i <= nTitle.Length - 1)
                    {
                        nTitle[i] = 0;
                        i++;
                    }
                    sfb.Title = nTitle;


                    Console.WriteLine("[*] Disc Title: {0}", new string(bytesToChar(sfb.Title)));


                }

                else
                {
                    Console.WriteLine("Error New TITLE_ID to long");
                }


            }

            Console.WriteLine("Save Y/N");
            input = Console.ReadLine();
            if (input == "Y" || input == "y")
            {

                using (FileStream SourceStream = File.OpenWrite(file))
                {
                    sfb.write(SourceStream);
                }




            }


        }
    }
    
}

    public class SFB
    {

        public byte[] _SFB = new byte[4];
        public byte[] Version = new byte[4];
        public byte[] Un1 = new byte[0x18];
        public byte[] H_F = new byte[0x10];
        public byte[] dcdo = new byte[4];
        public byte[] dcdl = new byte[4];
        public byte[] Un2 = new byte[8];
        public byte[] Tid = new byte[0x10];
        public byte[] dtdo = new byte[4];
        public byte[] dtdl = new byte[4];
        public byte[] Un3 = new byte[0x1A8];
        public byte[] Flags = new byte[0x20];
        public byte[] Title = new byte[0x10];
        public byte[] Un4 = new byte[0x3D0];

        public void load(FileStream sfb)
        {

            sfb.Read(_SFB, 0, 4);
            sfb.Read(Version, 0, 4);
            sfb.Read(Un1, 0, 0x18);
            sfb.Read(H_F, 0, 0x10);
            sfb.Read(dcdo, 0, 4);
            sfb.Read(dcdl, 0, 4);
            sfb.Read(Un2, 0, 8);
            sfb.Read(Tid, 0, 0x10);
            sfb.Read(dtdo, 0, 4);
            sfb.Read(dtdl, 0, 4);
            sfb.Read(Un3, 0, 0x1A8);
            sfb.Read(Flags, 0, 0x20);
            sfb.Read(Title, 0, 0x10);
            sfb.Read(Un4, 0, 0x3D0);

        }

        public void write(FileStream sfb)
        {

            sfb.Write(_SFB, 0, 4);
            sfb.Write(Version, 0, 4);
            sfb.Write(Un1, 0, 0x18);
            sfb.Write(H_F, 0, 0x10);
            sfb.Write(dcdo, 0, 4);
            sfb.Write(dcdl, 0, 4);
            sfb.Write(Un2, 0, 8);
            sfb.Write(Tid, 0, 0x10);
            sfb.Write(dtdo, 0, 4);
            sfb.Write(dtdl, 0, 4);
            sfb.Write(Un3, 0, 0x1A8);
            sfb.Write(Flags, 0, 0x20);
            sfb.Write(Title, 0, 0x10);
            sfb.Write(Un4, 0, 0x3D0);

        }
    
}


