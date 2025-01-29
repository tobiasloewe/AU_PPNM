sfuns.dll : sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs

math.exe : math.cs sfuns.dll
	mcs -target:exe -out:math.exe -reference:sfuns.dll math.cs

Out.txt : math.exe              # Out.txt depends on math.exe
	mono math.exe > Out.txt # run hello.exe, send output to Out.txt

math.exe : math.cs             # math.exe depends on math.cs
	mcs math.cs             # compile math.cs into math.exe

clean:                           # a phoney target, no dependencies
	rm -f Out.txt math.exe  # remove secondary files
