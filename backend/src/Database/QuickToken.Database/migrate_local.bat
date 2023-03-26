@echo off

dotnet ef migrations add Initial -- --sqlite

pause >nul