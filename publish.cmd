set pwd=%cd%
cd src
dotnet clean
dotnet publish -c Release -o %pwd%-publish\