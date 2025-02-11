Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs epsilon.dll
	mcs -target:exe -reference:epsilon.dll -out:main.exe main.cs

epsilon.dll : epsilon.cs
	mcs -target:library -out:epsilon.dll epsilon.cs

clean:
	rm -f *.exe *.dll Out.txt
