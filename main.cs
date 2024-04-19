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
        public Cell(int db_ind, string[] sourceFields) {
            columnsArr = new Object[sourceFields.Length];
            this.db_ind = db_ind;

            
        }

    }
}