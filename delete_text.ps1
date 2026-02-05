# Delete text from database
$dbPath = "G:\My Drive\FlowChartApp2\flowchart_v3.db"

# Load SQLite assembly
Add-Type -Path "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Data.SQLite\v4.0_1.0.118.0__db937bc2d44ff139\System.Data.SQLite.dll" -ErrorAction SilentlyContinue

try {
    $connectionString = "Data Source=$dbPath;Version=3;"
    $connection = New-Object System.Data.SQLite.SQLiteConnection($connectionString)
    $connection.Open()
    
    Write-Host "Searching for items with 'Proccess Flow' or 'مخطط العمليات'..." -ForegroundColor Cyan
    
    # Query FlowBoxes
    $cmd = $connection.CreateCommand()
    $cmd.CommandText = "SELECT Id, Name, Description, DescriptionArabic FROM FlowBoxes"
    $reader = $cmd.ExecuteReader()
    
    $idsToDelete = @()
    while ($reader.Read()) {
        $id = $reader["Id"]
        $name = $reader["Name"]
        $desc = if ($reader["Description"] -ne [DBNull]::Value) { $reader["Description"] } else { "" }
        $descAr = if ($reader["DescriptionArabic"] -ne [DBNull]::Value) { $reader["DescriptionArabic"] } else { "" }
        
        if ($name -like "*Proccess Flow*" -or $name -like "*مخطط العمليات*" -or 
            $desc -like "*Proccess Flow*" -or $desc -like "*مخطط العمليات*" -or
            $descAr -like "*Proccess Flow*" -or $descAr -like "*مخطط العمليات*") {
            
            Write-Host "Found FlowBox to delete: ID=$id, Name=$name" -ForegroundColor Yellow
            $idsToDelete += $id
        }
    }
    $reader.Close()
    
    # Delete found items
    foreach ($id in $idsToDelete) {
        # Delete connections
        $deleteConnCmd = $connection.CreateCommand()
        $deleteConnCmd.CommandText = "DELETE FROM FlowConnections WHERE SourceBoxId = $id OR TargetBoxId = $id"
        $connDeleted = $deleteConnCmd.ExecuteNonQuery()
        Write-Host "Deleted $connDeleted connection(s) for box ID $id" -ForegroundColor Green
        
        # Delete box
        $deleteBoxCmd = $connection.CreateCommand()
        $deleteBoxCmd.CommandText = "DELETE FROM FlowBoxes WHERE Id = $id"
        $boxDeleted = $deleteBoxCmd.ExecuteNonQuery()
        Write-Host "Deleted box ID $id" -ForegroundColor Green
    }
    
    if ($idsToDelete.Count -eq 0) {
        Write-Host "No matching items found in FlowBoxes" -ForegroundColor Green
    }
    
    $connection.Close()
    Write-Host "`nDone!" -ForegroundColor Cyan
    
} catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host "Trying alternate method without System.Data.SQLite..." -ForegroundColor Yellow
    
    # Alternative: Use sqlite3.exe if available
    $sqlite3 = "sqlite3.exe"
    if (Get-Command $sqlite3 -ErrorAction SilentlyContinue) {
        & $sqlite3 $dbPath "DELETE FROM FlowConnections WHERE SourceBoxId IN (SELECT Id FROM FlowBoxes WHERE Name LIKE '%Proccess Flow%' OR Description LIKE '%Proccess Flow%' OR DescriptionArabic LIKE '%مخطط العمليات%') OR TargetBoxId IN (SELECT Id FROM FlowBoxes WHERE Name LIKE '%Proccess Flow%' OR Description LIKE '%Proccess Flow%' OR DescriptionArabic LIKE '%مخطط العمليات%');"
        & $sqlite3 $dbPath "DELETE FROM FlowBoxes WHERE Name LIKE '%Proccess Flow%' OR Description LIKE '%Proccess Flow%' OR DescriptionArabic LIKE '%مخطط العمليات%';"
        Write-Host "Deleted using sqlite3.exe" -ForegroundColor Green
    } else {
        Write-Host "sqlite3.exe not found. Manual deletion required." -ForegroundColor Red
    }
}
