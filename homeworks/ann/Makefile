CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: Out.txt plot.results.png plot.losscurve.png

results.txt: Out.txt

results.txt: Out.txt

plot.results.png : results.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			plot  \
				"$<" index 0 using 1:2 with points lw 2 title "labels", \
				"$<" index 0 using 1:3 with points lw 2 title "predictions"\
		' | gnuplot

plot.firstderivative.png : results.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set title "First Derivative" ;\
			set xlabel "x" ;\
			set ylabel "y" ;\
			plot  \
				"$<" index 0 using 1:4 with points lw 2 title "labels", \
				"$<" index 0 using 1:5 with points lw 2 title "predictions" \
		' | gnuplot

plot.secondderivative.png : results.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set title "Second Derivative" ;\
			set xlabel "x" ;\
			set ylabel "y" ;\
			plot  \
				"$<" index 0 using 1:6 with points lw 2 title "labels", \
				"$<" index 0 using 1:7 with points lw 2 title "predictions" \
		' | gnuplot

plot.losscurve.png : rawlosses.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "epoch" ;\
			set ylabel "loss" ;\
			plot  \
				"$<" index 0 using 1:2 with linespoints lw 2 title "train loss", \
				"$<" index 0 using 1:3 with linespoints lw 2 title "val loss" \
		' | gnuplot


Out.txt: main.exe
	$(RUN) $< -samples 500 -lossfile rawlosses.txt -plotdatafile results.txt >$@;

main.exe: main.cs matrix.dll ann.dll; $(MKEXE)

ann.dll: ../libraries/ann.cs matrix.dll minimum.dll; $(MKLIB)

minimum.dll: ../libraries/minimum.cs matrix.dll qr.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png