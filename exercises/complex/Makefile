CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 

Out.txt: main.exe
	mono main.exe > $@

main.exe: main.cs cmath.dll; $(MKEXE)

cmath.dll : complex.cs cmath.cs
	mcs -target:library -out:./cmath.dll $^

clean:
	$(RM) *.exe *.dll [Oo]ut*.txt