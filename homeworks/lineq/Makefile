CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: out.txt; cat $<

out.txt: main.exe
	$(RUN) $< -dims 5,5 > $@

main.exe: main.cs matrix.dll qr.dll; $(MKEXE)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : vector.cs matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*