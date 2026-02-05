# Save with UTF-8 BOM encoding
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Get all legend items
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5273/api/Flow/legend" -Method GET
    
    # Display all legend items
    Write-Host "Current Legend Items:" -ForegroundColor Green
    Write-Host "===========================================" -ForegroundColor Green
    $response | ForEach-Object {
        Write-Host "`nID: $($_.id)" -ForegroundColor Cyan
        Write-Host "Title: $($_.title)" -ForegroundColor Yellow
        Write-Host "BG Color: $($_.backgroundColor)" -ForegroundColor Gray
        Write-Host "---"
    }
    
    # Try to find and delete items matching "Proccess" (note the typo)
    $itemsToDelete = $response | Where-Object { $_.title -match "Proccess" }
    
    if ($itemsToDelete) {
        foreach ($item in $itemsToDelete) {
            Write-Host "`nDeleting item:" -ForegroundColor Red
            Write-Host "ID: $($item.id) - Title: $($item.title)" -ForegroundColor Red
            
            # Delete the item
            $headers = @{
                "Content-Type" = "application/json"
            }
            $body = $item.id | ConvertTo-Json
            
            Invoke-RestMethod -Uri "http://localhost:5273/api/Flow/legend/delete" -Method POST -Body $body -Headers $headers
            
            Write-Host "Item deleted successfully!" -ForegroundColor Green
        }
    } else {
        Write-Host "`n`nNo items matching 'Proccess' found." -ForegroundColor Yellow
        Write-Host "Please review the list above and manually specify the ID if needed." -ForegroundColor Yellow
    }
} catch {
    Write-Host "`nError: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host $_.Exception.StackTrace -ForegroundColor Red
}
