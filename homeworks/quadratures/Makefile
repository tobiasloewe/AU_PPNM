CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono


all: out.txt;

out.txt: main.exe
	mono $< >$@;

main.exe: main.cs matrix.dll quadratures.dll genlist.dll; $(MKEXE)

quadratures.dll : ../libraries/quadratures.cs; $(MKLIB)

genlist.dll : ../libraries/genlist.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt