using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        string dbPath = @"G:\My Drive\FlowChartApp2\flowchart_v3.db";
        string connectionString = $"Data Source={dbPath};Version=3;";
        
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            
            Console.WriteLine("Searching for items containing 'Proccess Flow'...");
            
            // Find boxes to delete
            using (var cmd = new SQLiteCommand("SELECT Id, Name FROM FlowBoxes WHERE Name LIKE '%Proccess Flow%' OR Description LIKE '%Proccess Flow%' OR DescriptionArabic LIKE '%Proccess Flow%'", connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                   Console.WriteLine($"Found box to delete: ID={id}, Name={name}");
                    
                    // Delete connections
                    using (var deleteConnCmd = new SQLiteCommand($"DELETE FROM FlowConnections WHERE SourceBoxId = {id} OR TargetBoxId = {id}", connection))
                    {
                        int connDeleted = deleteConnCmd.ExecuteNonQuery();
                        Console.WriteLine($"  Deleted {connDeleted} connection(s)");
                    }
                    
                    // Delete box
                    using (var deleteBoxCmd = new SQLiteCommand($"DELETE FROM FlowBoxes WHERE Id = {id}", connection))
                    {
                        int boxDeleted = deleteBoxCmd.ExecuteNonQuery();
                        Console.WriteLine($"  Deleted box");
                    }
                }
            }
            
            connection.Close();
            Console.WriteLine("\nDone!");
        }
    }
}
