import os

os.system("dotnet pack ./GDExtension -o " + os.getcwd() + "/NugetSource/")
os.system("dotnet pack ./Generators -o " + os.getcwd() + "/NugetSource/")

os.system("dotnet nuget locals all --clear")

os.system("dotnet restore ./GDExtension")
os.system("dotnet restore ./Generators")
