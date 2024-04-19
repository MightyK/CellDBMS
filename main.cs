using System;
using System.IO;

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
            Moodel,
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
    }
}