@echo off
SET srcDir=%cd%
set dstx=%srcDir%-%RANDOM%


FOR /F "tokens=* USEBACKQ" %%F IN (`git rev-parse HEAD`) DO (
SET master-sha=%%F
)


set dst=%dstx%\publish
set dsttmp=%dstx%\temp
set dst2=%dst%\wwwroot
md %dst2%
md %dsttmp%
git branch gh-pages origin/gh-pages
git clone --separate-git-dir=%dst2%\.git  -l -b gh-pages . %dsttmp% 

cd src

dotnet clean

cd RegexTesterBlazorClientSide

dotnet  publish -c Release -o %dst%

cd %dst2%

del *.br /S
del *.gz /S
del *.log /S

git ls-files --deleted -z | xargs -0 git rm
git add -u -f :/
git add -f :/
git commit -m "Built %master-sha%" --allow-empty
git push origin gh-pages:gh-pages

cd ..
mklink /D /J regex-tester wwwroot
start http-server
start chrome -incognito http://localhost:8080/regex-tester
cd %srcDir%
gitk HEAD gh-pages
pause
rem rd /s /q %dstx%

