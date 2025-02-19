CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

data.txt: main.exe
	mono $< > $@

plot.erf.svg : data.txt erf.data Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key left ;\
		plot  \
			"$<" index 0 with lines title "erf(x)" \
			,"erf.data" with points pointtype 4 title "tabulated erf(x)" \
	' | tee log.erf.gpi | gnuplot
	
sfuns.dll : sfuns.cs ; $(MKLIB)
main.exe : main.cs sfuns.dll; $(MKEXE)

clean:	
	$(RM) *.exe *.dll [Oo]ut* [Ll]og*