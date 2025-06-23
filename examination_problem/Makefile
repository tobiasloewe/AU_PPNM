CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: Out.txt

Out.txt: main.exe
	$(RUN) $< >$@;

main.exe: main.cs integ2d.dll midpoint.dll quadtree.dll; $(MKEXE)

integ2d.dll: integ2d.cs; $(MKLIB)

midpoint.dll: midpoint.cs; $(MKLIB)

quadtree.dll: quadtree.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png