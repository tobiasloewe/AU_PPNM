CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: Out.txt

Out.txt: main.exe
	$(RUN) $< >$@;

main.exe: main.cs integrate2d.dll; $(MKEXE)

integrate2d.dll: integrate2d.cs; $(MKLIB)


clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.png