@echo off
SET srcDir=%cd%
set dstx=%srcDir%-%RANDOM%


FOR /F "tokens=* USEBACKQ" %%F IN (`git rev-parse HEAD`) DO (
SET master-sha=%%F
)


set dst=%dstx%\publish
set dsttmp=%dstx%\temp
set dst2=%dst%\RegexTesterBlazorClientSide\dist
md %dst2%
md %dsttmp%
git clone --separate-git-dir=%dst2%\.git  -l -b gh-pages . %dsttmp% 

cd src
dotnet clean

dotnet  publish -c Release -o %dst%

git --git-dir=%dst2%\.git --work-tree=%dst2% add . -f
git --git-dir=%dst2%\.git --work-tree=%dst2% status
git --git-dir=%dst2%\.git --work-tree=%dst2% commit -m "Built %master-sha%" --allow-empty
git --git-dir=%dst2%\.git --work-tree=%dst2% push origin gh-pages:gh-pages

rd /s /q %dstx%

gitk HEAD gh-pages

