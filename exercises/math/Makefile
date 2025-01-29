Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs math.dll
	mcs -target:exe -reference:math.dll -out:main.exe main.cs

math.dll : math.cs sfuns.dll
	mcs -target:library -reference:sfuns.dll -out:math.dll math.cs

sfuns.dll : sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs

clean:
	rm -f *.exe *.dll Out.txt
