CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
#TIME = time --format "$$nsize %e %U" --append --output $@

all: Out.txt plot.fit.svg plot.fiterror.svg; cat $<

plot.fit.svg : plotdata.txt Makefile
		echo ' \
			set terminal svg; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set title "Least Squares Fit of ThX Data"; \
			set xlabel "time t [days]" ;\
			set ylabel "ThX [a.u.]" ;\
			plot  \
				"$<" index 0 using 1:2:3 with yerrorlines title "Rutherford Data", \
				"$<" index 0 using 1:4 with lines title "LS Fit" \
		' | gnuplot

plot.fiterror.svg : plotdata.txt Makefile
		echo ' \
			set terminal svg; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set title "Least Squares Fits comparison"; \
			set xlabel "time t [days]" ;\
			set ylabel "ThX [a.u.]" ;\
			plot  \
				"$<" index 0 using 1:4 with lines title "best Fit", \
				"$<" index 0 using 1:5 with lines title "Fit error ++", \
				"$<" index 0 using 1:6 with lines title "Fit error --", \
				"$<" index 0 using 1:7 with lines title "Fit error -+", \
				"$<" index 0 using 1:8 with lines title "Fir error +-" \
		' | gnuplot

Out.txt: main.exe
	mono $< -fitdata decaydata.txt -dims 12,4 >$@;

main.exe: main.cs matrix.dll jacobi.dll qr.dll fit.dll; $(MKEXE)

jacobi.dll: jacobi.cs matrix.dll; $(MKLIB)

fit.dll: fit.cs qr.dll matrix.dll; $(MKLIB)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../matrix/vector.cs ../matrix/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* plotdata.txt *.gpi *.svg
