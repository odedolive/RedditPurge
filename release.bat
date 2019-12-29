dotnet publish ./src/RedditPurgeApp
del release /q /f /s
md release
xcopy src\RedditPurgeApp\bin\Release\netcoreapp3.1\win-x64\chromedriver.exe release /y
xcopy src\RedditPurgeApp\bin\Release\netcoreapp3.1\win-x64\publish\*.exe release /y

