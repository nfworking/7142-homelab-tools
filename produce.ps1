param(
    [string]$PreCon = ".\PreReqChk\PreReqChk.csproj",
    [string]$PreSel = ".\PreReqChkSel\PreReqChkSel.csproj",
    [string]$PreExMoe = ".\moeDEP\moeDEP.csproj",
    [string]$PreExPor = ".\mobDEP\mobDEP.csproj",
    [string]$PreConOut = "C:\users\tidal\projects\7142_Joiner_Tools\dist\PreCon",
    [string]$PreSelOut = "C:\users\tidal\projects\7142_Joiner_Tools\dist\PreSel",
    [string]$PreExMoeOut = "C:\users\tidal\projects\7142_Joiner_Tools\dist\PreExMoe",
    [string]$PreExPorOut = "C:\users\tidal\projects\7142_Joiner_Tools\dist\PreExPor",
    [string]$Runtime = "win-x64"
)

Write-Host("Compiling $PreCon")
dotnet publish $PreCon `
    -c Release `
    -r $Runtime `
    --self-contained `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o $PreConOut


Write-Host("Compiling $PreSel")
dotnet publish $PreSel `
    -c Release `
    -r $Runtime `
    --self-contained `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o $PreSelOut

Write-Host("Compiling $PreExMoe")
dotnet publish $PreExMoe `
    -c Release `
    -r $Runtime `
    --self-contained `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o $PreExMoeOut

Write-Host("Compiling $PreExPor")
dotnet publish $PreExPor `
    -c Release `
    -r $Runtime `
    --self-contained `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o $PreExPorOut