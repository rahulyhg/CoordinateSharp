﻿/*
 * The following program is used to automate certain aspects of the testing of CoordinateSharp.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CoordinateSharp;
using System.Drawing;
using System.Diagnostics;
namespace CoordinateSharp_TestProj
{
    class Program
    {
      
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select Test Module to Run (Enter Test Number):");
                Console.WriteLine();
                Console.WriteLine("1. Coordinate Initialzations");
                Console.WriteLine("2. Coordinate Conversions");
                Console.WriteLine("3. Coordinate Parsers");
                Console.WriteLine("4. Celestial");
                Console.WriteLine("5. Distance Initialization Tests");
                Console.WriteLine("6. Benchmarks");
                Console.WriteLine("7. GeoFence Tests");
                Console.WriteLine("8. ..Exit");
                Console.WriteLine();
                Console.Write("Select a Test Number: ");
                string t = Console.ReadLine();
                Console.WriteLine("*********************");
                Console.WriteLine();
                switch(t)
                {
                    case "1":
                        Coordinate_Initialization_Tests();
                        break;
                    case "2":
                        Coordinate_Convsersions_Tests();
                        break;
                    case "3":
                        Coordinate_Parsers_Tests();
                        break;
                    case "4":
                        Celestial_Tests();
                        break;
                    case "5":
                        Distance_Initialization_Tests();
                        break;
                    case "6":
                        Benchmark_Tests();
                        break;
                    case "7":
                        break;
                    case "8":                
                        return;
                    default:
                        Console.WriteLine();                      
                        Colorful.Console.WriteLine("Test choice invalid.", Color.Red);            
                        break;
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            
        }

        #region Coordinate Initialization
        static void Coordinate_Initialization_Tests()
        {
            //Check for errors with initialization as most calculations occur on load
            bool pass = true;
            try
            {
                Coordinate c = new Coordinate();
                c = new Coordinate(25, 25);
                c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0));

                EagerLoad eg = new EagerLoad();
                eg.Cartesian = false;
                eg.Celestial = false;
                eg.UTM_MGRS = false;

                c = new Coordinate(eg);
                c = new Coordinate(25, 25, eg);
                c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0), eg);
            }
            catch { pass = false; }
            Write_Pass("Coordinate Initialization Error Checks", pass);

            try
            {
                pass = true;
                Coordinate c = new Coordinate();
                CoordinatePart cp = new CoordinatePart(CoordinateType.Lat, c);
                cp = new CoordinatePart(CoordinateType.Long, c);
                cp = new CoordinatePart(25, CoordinateType.Lat, c);
                cp = new CoordinatePart(25, CoordinateType.Long, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.N, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.E, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.S, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.W, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.N, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.E, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.S, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.W, c);
            }
            catch { pass = false; }
            Write_Pass("CoordinatePart Initialization Error Checks", pass);
            try
            {
                pass = true;
                UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
                utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Write_Pass("UniversalTransverseMercator Initialization Error Checks", pass);
            try
            {
                pass = true;
                //Outputs 19T CE 51307 93264
                MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264);
                mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Write_Pass("UniversalTransverseMercator Initialization Error Checks", pass);
            try
            {
                pass = true;
                Coordinate c = new Coordinate();
                Cartesian cart = new Cartesian(c);
                cart = new Cartesian(345, -123, 2839);
            }
            catch { pass = false; }
            Write_Pass("Cartesian Initialization Error Checks", pass);
            Console.WriteLine();

            //Init Range Checks
            try
            {
                pass = true;
                EagerLoad eg = new EagerLoad();

                Coordinate c = new Coordinate(90, 180);            
                c = new Coordinate(-90, -180);
                c = new Coordinate(90, 180, new DateTime());
                c = new Coordinate(-90, -180, new DateTime());
                c = new Coordinate(90, 180, eg);
                c = new Coordinate(-90, -180, eg);
                c = new Coordinate(90, 180, new DateTime(), eg);
                c = new Coordinate(-90, -180, new DateTime(), eg);

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181); pass = false; }
                catch { }

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181, new DateTime()); pass = false; }
                catch {  }

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181, new DateTime(), eg); pass = false; }
                catch { }
            }
            catch { pass = false; }
            Write_Pass("Cordinate Initialization Range Checks", pass);

            pass = true;
            try
            {
                Coordinate c = new Coordinate();
                CoordinatePart cp = new CoordinatePart(90, CoordinateType.Lat, c);
                cp = new CoordinatePart(-90, CoordinateType.Lat, c);
                cp = new CoordinatePart(89, 59,  CoordinatesPosition.N, c);
                cp = new CoordinatePart(89, 59, CoordinatesPosition.S, c);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.N, c);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.S, c);
                cp = new CoordinatePart(180, CoordinateType.Long, c);
                cp = new CoordinatePart(-180, CoordinateType.Long, c);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.E, c);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.W, c);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.E, c);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.W, c);

                //Should fail
                try { cp = new CoordinatePart(91, CoordinateType.Lat, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-91, CoordinateType.Lat, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, CoordinateType.Long, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-181, CoordinateType.Long, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.N, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.S, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.N, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.S, c); pass = false; } catch { }


                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.E, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.W, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.E, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.W, c); pass = false; } catch { }
            }
            catch
            {
                pass = false;
            }
            Write_Pass("CordinatePart Initialization Range Checks", pass);
        }
        #endregion

        static void Coordinate_Convsersions_Tests()
        {
            Console.WriteLine();
            //GATHER CONVERSIONS
            //Conversion lists must end in //** to signify end of list
            List<List<string>> Conversions = new List<List<string>>();
            string[] coordStrings = File.ReadAllLines("Conversions.txt");
            List<string> cList = new List<string>();
            foreach (string c in coordStrings)
            {
                if (c == "//**")
                {
                    Conversions.Add(cList);
                    cList = new List<string>(); }
                else
                {
                    cList.Add(c);
                }
            }
            //Conversion coords to test
            List<double[]> coords = new List<double[]>();
            coords.Add(new double[] { 39.5768, 72.4859 });
            coords.Add(new double[] { -15.5768, 100.4859 });
            coords.Add(new double[] { 65.25, -15.1859 });
            coords.Add(new double[] { -80.6586, -152.49 });

            for (int x = 0; x<Conversions.Count;x++)
            {
                List<string> coordList = Conversions[x];
                double lat = coords[x][0];
                double lng = coords[x][1];
                //0 = Decimal / Signed
                //1 = Decimal Degree
                //2 = Degree Decimal Minute
                //3 = Degree Minutes Seconds
                //4 = UTM
                //5 = MGRS
                //6 = Cartesian
                Coordinate c = new Coordinate(lat, lng);
                bool pass = true;
                Coordinate rc = new Coordinate();
                for (int y = 0; y<7;y++)
                {
                    
                    switch(y)
                    {
                        case 0:
                            c.FormatOptions.Format = CoordinateFormatType.Decimal;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            break;
                        case 1:
                            c.FormatOptions.Format = CoordinateFormatType.Decimal_Degree;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            break;
                        case 2:
                            c.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            rc = new Coordinate();
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.DecimalMinute, c.Latitude.Position, rc);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.DecimalMinute, c.Longitude.Position, rc);
                            if(rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 3:
                            c.FormatOptions.Format = CoordinateFormatType.Degree_Minutes_Seconds;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            rc = new Coordinate();
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.Minutes, c.Latitude.Seconds, c.Latitude.Position, rc);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.Minutes, c.Longitude.Seconds, c.Longitude.Position, rc);
                            if (rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 4:
                            if (c.UTM.ToString() != coordList[y]) { pass = false; }
                            UniversalTransverseMercator utm = new UniversalTransverseMercator(c.UTM.LatZone, c.UTM.LongZone, c.UTM.Easting, c.UTM.Northing);
                            rc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >=.00001) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 5:
                            if (c.MGRS.ToString() != coordList[y]) { pass = false; }
                            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem(c.MGRS.LatZone, c.MGRS.LongZone, c.MGRS.Digraph, c.MGRS.Easting, c.MGRS.Northing);
                            rc = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .0001) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .0001) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

                            break;
                        case 6:
                            if (c.Cartesian.ToString() != coordList[y]) { pass = false; }
                            Cartesian cart = new Cartesian(c.Cartesian.X, c.Cartesian.Y, c.Cartesian.Z);
                            rc = Cartesian.CartesianToLatLong(cart);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...Cartesian Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...Cartesian Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        default:
                            break;
                    }
                }

                Write_Pass("Conversion Pass " + ((int)(x+1)).ToString() + ": ", pass);
                
            }
        }
        static void Coordinate_Parsers_Tests()
        {
            CoordinatePart cp;
            Coordinate coordinate;
      
            //Parse Coordinate Formats
            string[] coordStrings = File.ReadAllLines("Coordinates.txt");
            bool pass = true;
            string lastType = "";
            Console.WriteLine("Coordinate Parses...");
            foreach (string c in coordStrings)
            {
                if (c.Contains("\\"))
                {
                    if (lastType!="")
                    {
                        Write_Pass(lastType.Split('#')[0], pass);
                       
                    }
                    lastType = "";
                    pass = true;
                    lastType = c;             
                }
                else
                {
                    string[] cc = c.Split('#');
                    if (!Coordinate.TryParse(cc[0], out coordinate))
                    {
                        pass = false;
                    }
                    else
                    {
                        if(coordinate.ToString() != cc[1])
                        {

                            Debug.WriteLine("...MISMATCH: " + coordinate.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                }
            }
            if (lastType != "")
            {
                Write_Pass(lastType.Split(',')[0], pass);
            }
            //Attempt Forces Param
            pass = true;
            try
            {

                if (Coordinate.TryParse("95F, 54", out coordinate)) { pass = false; }//Intentional Fail
                if (Coordinate.TryParse("E 181 30, 56 76", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("N 95 45, E 45", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("95 87 46 78 D", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("W24 45, W45", out coordinate)) { pass = false; } //Intentional Fail
            }
            catch { pass = false; }
            Console.WriteLine();
            Write_Pass("\\\\Intentional Fails", pass);
            Console.WriteLine();
            //Parse CoordinatePart Formats
            Console.WriteLine("CoordinatePart Parses...");
            coordStrings = File.ReadAllLines("CoordinateParts.txt");
            pass = true;
            lastType = "";
            foreach (string c in coordStrings)
            {
                c.Trim();
                if (c.Contains("\\"))
                {
                    if (lastType != "")
                    {
                        Write_Pass(lastType.Split('#')[0], pass);
                    }
                    lastType = "";
                    pass = true;
                    lastType = c;
                }
                else
                {
                    string[] cc = c.Split('#');
                    if (!CoordinatePart.TryParse(cc[0], out cp))
                    {
                        pass = false;
                    }
                    else
                    {
                        if (cp.ToString() != cc[1])
                        {
                            Debug.WriteLine("...MISMATCH: " + cp.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                }
            }
            if (lastType != "")
            {
                Write_Pass(lastType.Split(',')[0], pass);
            }
            //Attempt Forces Param
            pass = true;
            try
            {

                if (CoordinatePart.TryParse("95", CoordinateType.Lat, out cp)) { pass = false; }//Intentional Fail
                if (CoordinatePart.TryParse("E181", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("N 95 45", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("95", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("WD24 45", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
            }
            catch { pass = false; }
            Console.WriteLine();
            Write_Pass("\\\\Intentional Fails", pass);
           
        }
        static void Celestial_Tests()
        {
            CelestialTests ct = new CelestialTests();
            ct.Populate_CelestialTests();
        }

        #region Distance Tests
        static void Distance_Initialization_Tests()
        {
            //Conversions should be equal to these numbers within .0001 tolerance

            double m = 1000; //meters
            double km = 1; //Kilometers
            double ft = 3280.84; //Feet
            double sm = 0.6213712; //Nautical Miles
            double nm = 0.5399565; //Statute Miles

            double[] distances = new double[] {m,km,ft,nm,sm };

           
            Distance d = new Distance(km);

            Write_Pass("Distance(double km)", Check_Distance(d, distances));
            d = new Distance(distances[0], DistanceType.Meters);
            Console.WriteLine();
            Write_Pass("Distance(double distance, DistanceType Meters)", Check_Distance(d, distances));
            d = new Distance(distances[1], DistanceType.Kilometers);
            Write_Pass("Distance(double distance, DistanceType Kilometers)", Check_Distance(d, distances));
            d = new Distance(distances[2], DistanceType.Feet);
            Write_Pass("Distance(double distance, DistanceType Feet)", Check_Distance(d, distances));
            d = new Distance(distances[3], DistanceType.NauticalMiles);
            Write_Pass("Distance(double distance, DistanceType Nautical Miles)", Check_Distance(d, distances));
            d = new Distance(distances[4], DistanceType.Miles);
            Write_Pass("Distance(double distance, DistanceType Statute Miles)", Check_Distance(d, distances));
            Console.WriteLine();

            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            //KILOMETERS Between specified points above should be as follows in defined tolerance .000001
            double kmSphere = 412.0367538058125; 
            double kmWGS84 = 412.1977393206501; //Default datum WGS84

            d = new Distance(c1, c2);
            if(System.Math.Abs(d.Kilometers - kmSphere) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2)", true); }
            d = new Distance(c1, c2, Shape.Sphere);
            if (System.Math.Abs(d.Kilometers - kmSphere) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", true); }
            d = new Distance(c1, c2, Shape.Ellipsoid);
            if (System.Math.Abs(d.Kilometers - kmWGS84) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmWGS84);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", true); }
        }     
        static bool Check_Distance(Distance d, double[] distances)
        {
            bool pass = true;
            //Round to avoid float point issues
            double m = d.Meters;
            double km = d.Kilometers;
            double ft = d.Feet;
            double sm = d.Miles;
            double nm = d.NauticalMiles;
            if (System.Math.Abs(m - distances[0]) > .0001) { Debug.WriteLine("...METERS MISMATCH: " + d.Meters + " - " + distances[0]); return false; }
            if (System.Math.Abs(km - distances[1]) > .0001) { Debug.WriteLine("...KILOMETERS MISMATCH: " + d.Kilometers + " - " + distances[1]); return false; }
            if (System.Math.Abs(ft - distances[2]) > .0001) { Debug.WriteLine("...FEET MISMATCH: " + d.Feet + " - " + distances[2]); return false; }
            if (System.Math.Abs(nm - distances[3]) > .0001) { Debug.WriteLine("...NAUTICAL MILES MISMATCH: " + d.NauticalMiles + " - " + distances[3]); return false; }
            if (System.Math.Abs(sm - distances[4]) > .001) { Debug.WriteLine("...STATUTE MILE MISMATCH: " + d.Miles + " - " + distances[4]); return false; }
            return pass;
           
        }
        #endregion
        static void Benchmark_Tests()
        {

            Coordinate tc = null;
            Console.WriteLine("Starting Benchmarks, this test may take a while to finish...");
            Console.WriteLine();
            //Benchmark Standard Object Initialization
            Benchmark(() => { tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0)); }, 100, "Standard Initialization");

            Benchmark(() => {
                tc = new Coordinate();
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N, tc);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W, tc);
                tc.GeoDate = new DateTime(2018, 7, 26, 15, 49, 0);

            }, 100, "Secondary Initialization");

            //Benchmark TryParse Object Initialization
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), out tc); }, 100, "TryParse() Initialization");

            //Benchmark with EagerLoad fully off
            Benchmark(() => {
                EagerLoad eg = new EagerLoad();
                eg.UTM_MGRS = false;
                eg.Celestial = false;
                eg.Cartesian = false;
                tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0), eg);
            }, 100, "EagerLoad Off Initialization");
            tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

            //Benchmack property change
            Random r = new Random();
            Benchmark(() => { tc.Latitude.DecimalDegree = r.Next(-90, 90); }, 100, "Property Change");

        }
        static void Benchmark(Action act, int iterations, string s)
        {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                act.Invoke();
            }
            sw.Stop();
            Console.Write(s + ": ");
            Colorful.Console.Write((sw.ElapsedMilliseconds / iterations).ToString() + " ms", Color.Lime);
            Console.WriteLine();
        }

        static void Write_Pass(string method, bool pass)
        {
            Console.Write(method + ": ");
            if (pass)
            { 
                Colorful.Console.Write("PASS", Color.Lime);
            }
            else
            {
                Colorful.Console.Write("FAILED", Color.Red);
            }
            Console.WriteLine();
        }
    }
}
