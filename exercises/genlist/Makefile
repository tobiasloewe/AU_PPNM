CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE)
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE)

Out.txt: main.exe
	cat input.txt | mono $< > $@

main.exe: main.cs genlist.dll; $(MKEXE)

genlist.dll: genlist.cs; $(MKLIB)

clean:
	rm -f *.exe *.dll Out.txt