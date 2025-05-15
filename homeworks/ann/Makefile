CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: Out.txt

Out.txt: main.exe
	$(RUN) $< >$@;

main.exe: main.cs matrix.dll ann.dll; $(MKEXE)

ann.dll: ../libraries/ann.cs matrix.dll minimum.dll; $(MKLIB)

minimum.dll: ../libraries/minimum.cs matrix.dll qr.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png