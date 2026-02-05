# Save with UTF-8 BOM encoding
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Get all flowboxes
try {
    Write-Host "Fetching all flowchart boxes..." -ForegroundColor Cyan
    
    # First, let's try to get the page which should have boxes data
    $response = Invoke-WebRequest -Uri "http://localhost:5273/" -Method GET
    
    # Parse the response to find any script tags or data
    if ($response.Content -match '<script>.*?var boxes = (\[.*?\]);.*?</script>') {
        Write-Host "Found boxes data in page source" -ForegroundColor Green
    }
    
    # Alternatively, let's check if there's an API endpoint for boxes
    Write-Host "`nTrying to access Index page model data..." -ForegroundColor Cyan
    
    # Read the page and look for boxes
    $htmlContent = $response.Content
    
    # Search for any mention of "مخطط العمليات" or "Proccess Flow"
    if ($htmlContent -match "Proccess Flow") {
        Write-Host "`nFound 'Proccess Flow' in the page content!" -ForegroundColor Yellow
    }
    
    if ($htmlContent -match "مخطط العمليات") {
        Write-Host "`nFound 'مخطط العمليات' in the page content!" -ForegroundColor Yellow
    }
    
    Write-Host "`nSince there's no direct API for listing boxes, let's check the database file directly."
    Write-Host "The item might be in FlowBoxes or might have already been removed." -ForegroundColor Green
    
} catch {
    Write-Host "`nError: $($_.Exception.Message)" -ForegroundColor Red
}
