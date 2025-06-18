CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: main.exe sfuns.dll plot.lngamma.svg plot.gamma.svg plot.erf.svg

data.txt: main.exe
	mono $< > $@

plot.lngamma.svg : data.txt lngamma.data Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "x" ;\
		set ylabel "log(Î“(x))" ;\
		plot  \
			"$<" index 2 with lines title "log(gamma(x))" \
			,"lngamma.data" with points title "reference" \
	' | tee log.lngamma.gpi | gnuplot
	
plot.gamma.svg : data.txt factorials.data Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "x" ;\
		set ylabel "Î“(x)" ;\
		plot [-5:][-20:20] \
			"$<" index 1 with lines title "gamma(x)" \
			,"factorials.data" with points title "factorials" \
	' | tee log.gamma.gpi | gnuplot

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
	$(RM) *.exe *.dll [Oo]ut* [Ll]og* *.svg