# download latest nuget.exe
$nugeturl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nugetexe = "$($env:temp)\nuget.exe"
$client = new-object System.Net.WebClient
$client.DownloadFile($nugeturl, $nugetexe)

# create the nuspec file for the "Release" build
Invoke-Expression "$($nugetexe) Pack '.\PagedControl\PagedControl.csproj' -Properties Configuration=Release"

