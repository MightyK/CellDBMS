using System;
using System.IO;

class Program {
    // Represents an entry in the cell database
    public class Cell : IComparable {
        private int db_ind = 0; // Database index of the cell
        Object[] columnsArr;    // Array for storing various data columns from the file
    }
}