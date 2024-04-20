using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program {
    // Represents an entry in the cell database
    public class Cell : IComparable {
        private int db_ind = 0; // Database index of the cell
        Object[] columnsArr;    // Array for storing various data columns from the file

        // Column headers
        private static string[] columnHeaders = {
            "OEM",
            "Model",
            "Launce Announced",
            "Launch Status",
            "Body Dimensions",
            "Body Weight",
            "SIM Type",
            "Display Type",
            "Display Size",
            "Display Resolution",
            "Features",
            "OS"
        };

        // Enumeration for Cell Attributes
        public enum Attributes {
            Oem,
            Model,
            LaunchAnnounced,
            LaunchStatus,
            BodyDimensions,
            BodyWeight,
            BodySim,
            DisplayType,
            DisplaySize,
            DisplayResolution,
            FeatureSensors,
            OS
        }

        // Constructor
        public Cell(int db_ind, string[] csvSource) {
            columnsArr = new Object[csvSource.Length];
            this.db_ind = db_ind;

            CleanAndIngest(csvSource)
        }

        // Get a specific column
        public Object GetColumn(Cell.Attributes attribute) {
            return columnsArr[(int)attribute];
        }

        // Get all attributes
        public Object[] GetColumns() {
            return columnsArr;
        }

        // Get the name of a column based on attribute
        public static String GetColumnName(Cell.Attributes attribute) {
            return columnHeaders[(int)attribute];
        }

        // Get the index of the cell in the database
        public int GetIndex() {
            return db_ind;
        }

        // Get string representation of the cell with verbose information
        public String ToStringVerbose() {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("\nIndex: {0}", db_ind));

            for (int i  = 0; i < columnsArr.Length; i++) {
                sb.Append(String.Format("\n{0}: {1}", columnHeaders[i], columnsArr[i]));
            }

            return sb.ToString();
        }

        // Get string representation of the cell
        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(db_ind);

            for(int i = 0; i < columnsArr.Length; i++) {
                sb.Append(" " + columnsArr[i]);
            }
            return sb.ToString();
        }

        // Helper function for cleaning and ingesting source data
        private void CleanAndIngest(string[] source) {
            Regex regex;
            Match match;

            // Clean and ingest source data into respective attributes
            if (source[(int)Attributes.Oem] != "") {
                columnsArr[(int)Attributes.Oem] = source[(int)Attributes.Oem];
            }

            if (source[(int)Attributes.Model] != "") {
                columnsArr[(int)Attributes.Model] = source[(int)Attributes.Model];
            }

            String launchAnnouncedSource = source[(int)Attributes.LaunchAnnounced];
            regex = new Regex("\"(?:[^\\\\\"]|\\\\\\\\|\\\\\")*\"", RegexOptions.None);
            match = regex.Match(launchAnnouncedSource);
                
            if (match.Success) {
                launchAnnouncedSource = match.Groups[0].Value.Replace("\"", "");
            }

            regex = new Regex("\\d{4}", RegexOptions.None);
            match = regex.Match(launchAnnouncedSource);
            int launchAnnouncement;
            Int32.TryParse(match.Value, out launchAnnouncement);
            columnsArr[(int)Attributes.LaunchAnnounced] = launchAnnouncement;

            String launchStatusSource = source[(int)Attributes.LaunchStatus];
                
            if (launchStatusSource == "Discontinued" || launchStatusSource == "Cancelled") {
                columnsArr[(int)(Attributes.LaunchStatus)] = launchStatusSource;
            }
            else {
                match = regex.Match(launchStatusSource);
                columnsArr[(int)(Attributes.LaunchStatus)] = match.Value;
            }

            if (source[(int)Attributes.BodyDimensions] != "-") {
                columnsArr[(int)Attributes.BodyDimensions] = source[(int)Attributes.BodyDimensions];
            }

            String bodyWeightSource = source[(int)Attributes.BodyWeight];
            float bodyWeight;
            float.TryParse(bodyWeightSource.Split(" ")[0], out bodyWeight);
            columnsArr[(int) Attributes.BodyWeight] = bodyWeight;

            String bodySimSource = source[(int)Attributes.BodySim];
            regex = new Regex("\"(?:[^\\\\\"]|\\\\\\\\|\\\\\")*\"", RegexOptions.None);
            match = regex.Match(bodySimSource);
                
            if (match.Success) {
                bodySimSource = match.Groups[0].Value.Replace("\"", "");
            }
                
            if (bodySimSource.Contains("SIM")) {
                columnsArr[(int)Attributes.BodySim] = bodySimSource;
            }

            String displayTypeSource = source[(int)Attributes.DisplayType];
            match = regex.Match(displayTypeSource);
                
            if (match.Success) {
                columnsArr[(int) Attributes.DisplayType] = match.Groups[0].Value.Replace("\"", "");
            }
            else if (displayTypeSource.Equals("-")) {
                columnsArr[(int)Attributes.DisplayType] = null;
            }
            else  {
                columnsArr[(int)Attributes.DisplayType] = displayTypeSource;
            }


            String displaySizeSource = source[(int)Attributes.DisplaySize];
            displaySizeSource = displaySizeSource.Split(" ")[0];
                
            if (displaySizeSource != "") {
                float displaySize;
                float.TryParse(displaySizeSource, out displaySize);
                columnsArr[(int)Attributes.DisplaySize] = displaySize;
            }

            String displayResolutionSource = source[(int)Attributes.DisplayResolution];
            match = regex.Match(displayResolutionSource);
                
            if (displayResolutionSource != "" || displayResolutionSource != "-") {
                columnsArr[(int)Attributes.DisplayResolution] = displayResolutionSource;
            }
                
            if (match.Success) {
                columnsArr[(int)Attributes.DisplayResolution] = match.Groups[0].Value.Replace("\"", "");
            }
            else {
                columnsArr[(int)Attributes.DisplayResolution] = displayResolutionSource;
            }

            String featuresSensorsSource = source[(int)Attributes.FeatureSensors];
            match = regex.Match(featuresSensorsSource);
                
            if (match.Success) {
                columnsArr[(int)(Attributes.FeatureSensors)] = match.Groups[0].Value.Replace("\"", "");
            }
            else {
                columnsArr[(int)(Attributes.FeatureSensors)] = featuresSensorsSource;
            }

            String osSource = source[(int)Attributes.OS];
            match = regex.Match(osSource);
                
            if (match.Success) {
                columnsArr[((int)Attributes.OS)] = match.Groups[0].Value.Replace("\"", "");
            }
            osSource = osSource.Split(",")[0];
                
            if (osSource != "") {
                columnsArr[(int)Attributes.OS] = osSource;
            }
        }

        // Compare two devices based on body weight
        public int CompareTo(object? obj) {
            Cell otherCell = (Cell)obj;
            float thisCellWeight = (float)columnsArr[(int)Attributes.BodyWeight];
            float otherCellWeight = (float)otherCell.GetColumn(Attributes.BodyWeight);
                
            if (thisCellWeight > otherCellWeight) {
                return 1;
            }
                
            if(thisCellWeight < otherCellWeight) {
                return -1;
            }
            
            return 0;
        }
    }

    // Represents a collection of Cell objects
    public class CellData {
        private int numCells = 0;   // Number of devices stored
        private Dictionary<int, Cell> cellData; // Dictionary used to store the devices

        // Constructor
        public CellData() {
            CellData = new Dictionary<int, Cell>(); // Initiallize Cell data dictionary
        }

        // Parse CSV file data using RegEx
        public void RegexCellData(string filePath) {
            string[] lines = File.ReadAllAllLines(filePath);    // Read all lines in the CSV file

            for (int i = 1; i < lines.Length; i++) {    // Skip the header, start from the second element
                RegexCellData innerStr = new RegexCellData(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.IgnoreCase);

                string[] cellDataFields = innerStr.Split(lines[i]); // Use RegEx to split CSV data
                AddCell(cellDataFields);    // Add Cell data to the database
            }
        }

        // Parse CSV file data using TextFieldParser
        public int GenerateCellData(string fileName) {
            int seen = 0;   // Counter for the number of cells seen

            TextFieldParser parser = new TextFieldParser(fileName);
            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");

            parser.ReadFields();    // Skip header
            
            seen++;

            while (!parser.EndOfData) {
                string[] cellDataFields = parser.ReadFields();
                AddCell(cellDataFields);    // Add Cell data to the database
                seen++;
            }

            return seen;
        }

        // Get a cell from the database by index
        public Cell GetCell(int index) {
            return CellData[index];
        }

        // Add a new cell to the database
        public int AddCell(string[] data) {
            Cell newCell = new Cell(numCells, data);
            CellData.Add(numCells, newCell);    // Add Cell to the Dictionary

            return numCells++;  // Increment Cell count and return the index
        }

        public bool DeleteCell(int index) {
            if (CellData.ContainsKey(index)) {
                CellData.Remove(index); // Remove Cell from the Dictionary
                
                numCells--;

                return true;
            }

            Console.WriteLine("Cell at selected index does not exist!")

            return false;
        }

        // Get the number of Cells in the database
        public int GetNumCells() {
            return numCells;
        }

        // Print all Cells in the database
        public void PrintCells() {
            foreach (KeyValuePair<int, Cell> cell in cellData) {
                Console.WriteLine(cell.Value.ToString());
            }
        }

        // Print all Cells from a specific company in the database
        public void PrintCellsByOEM(string company) {
            List<Cell> cells = new List<Cell();

            Console.WriteLine(String.Format(Searcing for Cells by OEM: \"{0}\"...", company));

            foreach(KeyValuePair<int, Cell> cell in cellData) {
                if (pair.Value.GetColumn(Cell.Attributes.Oem).Equals(company)) {
                    cells.Add(cell.Value);
                }

            }
            
            Console.WriteLine($"Devices found: {cells.Count}");

            foreach(Cell cell in cells) {
                Console.WriteLine(cell.ToStringVerbose());
            }
        }

        // Print certain weight statistics pertaining to the database
        public void PrintWeights() {
            int minInd = 0, int maxInd = 0;
            float sum = 0, mean = 0, maxWeight = 0, minWeight =int.MaxValue;

            List<Cell> cells = new List<Cell>();
            Dictionary<float, int> weights = new Dictionary<float, int><float, int>();

            foreach(KeyValuePair<int, Cell> cell in cellData) {
                Cell currentCell = cell.Value;

                float currentWeight = (float)currentCell.GetColumn(Cell.Attributes.BodyWeight);

                if (currentWeight != 0) {
                    if (currentWeight > maxWeight) {
                        maxWeight = currentWeight;
                        maxInd = currentCell.GetIndex();
                    }

                    if (currentWeight < minWeight) {
                        minWeight = currentWeight;
                        minInd = currentCell.GetIndex();
                    }

                    sum += currentWeight;

                    if (weights.ContainsKey(currentWeight)) {
                        weights[currentWeight] += 1;
                    }
                    else {
                        weights[currentWeight] = 1;
                    }

                    cells.Add(currentCell);
                }
            }

            cells.Sort((cell1, cell2) => cell1.CompareTo(cell2));

            mean = sum / cells.Count;

            StringBuilder sb  new StringBuilder();
            
            sb.Append(String.Format("Lightest: {0} {1}: {2} (g)", cellData[minInd].GetColumn(Cell.Attributes.Oem), cellData[minInd].GetColumn(Cell.Attributes.Model), cellData[minInd].GetColumn(Cell.Attributes.BodyWeight)));
            
            sb.Append(String.Format("\nHeaviest: {0} {1}: {2} (g)", cellData[maxInd].GetColumn(Cell.Attributes.Oem), cellData[maxInd].GetColumn(Cell.Attributes.Model), cellData[maxInd].GetColumn(Cell.Attributes.BodyWeight)));
            
            sb.Append(String.Format("\nAverage Weight: {0} (g)", mean));
            
            Console.WriteLine(sb.ToString());
        }

        // Returns a Dictionary of Cell data via an OEM query
        public Dictionary<string, List<Cell>> GetCellsByOEM() {
            Dictionary<string, List<Cell>> cells = new Dictionary<string, List<Cell>>();

            foreach(KeyValuePair<int, Cell> cell in cellData) {
                if(!cells.ContainsKey((string)cell.Value.GetColumn(Cell.Attributes.Oem))) {
                    List<Cell> newCells = new List<Cell>();
                    newCells.Add(cell.Value);
                    cells.Add((string)cell.Value.GetColumn(Cell.Attributes.Oem), newCells);
                }
                else {
                    List<Cell> usedCells = cells[(string)cell.Value.GetColumn(Cell.Attributes.Oem)];
                    usedCells.Add(cell.Value);
                }
            }

            return cells;
        }

        // Prints a List of Cells with a certain feature
        public void PrintUniqueColumns(Cell.Attributes column) {
            int mode = 0;
            string modeRes = "";
            string columnHeader = Cell.GetColumnName(column);
            
            Dictionary<string, int> cells = new Dictionary<string, int> ();

            foreach (KeyValuePair<int, Cell> cell in cellData) {
                if (!cells.ContainsKey((string)cell.Value.GetColumn(column))) {
                    cells.Add((string)cell.Value.GetColumn(column), 1);
                }
                else {
                    cells[(string)cell.Value.GetColumn(column)]++;
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, int> cell in cells) {
                sb.Append(String.Format("\n{0}: {1} results found: {2}", columnHeader, cell.Key, cell.Value));

                if (cell.Value > mode) {
                    mode = cell.Value;
                    modeRes = cell.Key;
                }
            }

            sb.Append(String.Format("\Most common {0} is {1} with {2} instances discovered!", columnHeader, modeRes, mode));

            Console.WriteLine(sb.ToString());
        }

        // Prints a List of devices released in a specified year
        public void PrintYearlyReleases(int year = 0) {
            int numDevices = 0, yearMaxCells = 0;

            SortedDictionary<int, int> release = new SortedDictionary<int, int>();

            foreach (KeyValuePair<int, Cell> cell in cellData) {
                Cell device = cell.Value;
                int announced = (int)device.GetColumn(Cell.Attributes.LaunchAnnounced);
                string status = (string)device.GetColumn(Cell.Attributes.LaunchStatus);

                if (announced > 0 && !status.Contains("Cancelled")) {
                    if (!release.ContainsKey(announced)) {
                        release.Add(announced, 1);
                    }
                    else {
                        release[announced]++;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            if (year != 0) {
                sb.Append(String.Format("\nDevices released in {0}: {1}", year, release[year]));
            }
            else {
                foreach (KeyValuePair<int, int> cell in release) {
                    if (cell.Value > numDevices) {
                        yearMaxCells = cell.Key;
                        numDevices = cell.Value;
                    }
                }

                sb.Append(String.Format("\nThe year with the most released devices is {0} with {1} releases.", yearMaxCells, numDevices));
            }

            Console.Writeine(sb.ToString());
        }
    }

    static string fileName = "cells.csv";

    public static void Main (string[] args) {

    }

    public static void MaxAvgWeightCell(CellData cellData) {

    }   

    public static void OneFeatureCells(CellData cellData)  {
        int data = cellData.GetNumCells();
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < data; i++) {
            Cell cell = cellData.GetCell(i);
            string features = (string)cell.GetColumn(Cell.Attributes.FeatureSensors);
            string[] featsArr = features.Split(',');

            if (featsArr.Length == 1) {
                cells.Add(cell);
            }
        }

        StringBuilder sb = new StringBuilder();

        sb.Append(String.Format("\nCell phones with one feature: {0}", cells.Count));
        Console.WriteLine(sb.ToString());
    }

    public static void DelayedCells(CellData cellData) {

    }

    public static void UnitTests() {

    }
}